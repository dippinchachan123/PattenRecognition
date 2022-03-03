using System.IO;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NumberAI : MonoBehaviour
{
    List<float> Points_x = new List<float>();
    List<float> Points_y = new List<float>();
    public Transform brush;
    Transform pointParent;
    [SerializeField]
    TMP_InputField Txtmesh;
    [SerializeField]
    TextMeshProUGUI[] CodisTextMesh; 
    
    //References
    bool pressed = false;
    bool ref1 = false;
    float ref2 = 0;


    // Start is called before the first frame update
    void Start()
    {

        pointParent = GameObject.FindGameObjectWithTag("Brush").transform;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PressedButton(0,ref ref1))
        {
            if (Timer(0.01f, ref ref2))
            {
                Vector3 pos = Input.mousePosition;
                pos = Camera.main.ScreenToWorldPoint(pos);
                Transform point = Instantiate(brush, new Vector3(pos.x, pos.y, 0), Quaternion.identity);
                point.name = "Point" + pointParent.childCount.ToString();
                point.parent = pointParent;
                int len = Points_y.Count;
                if (len > 0)
                {
                    if (!(Mathf.Abs(pos.x - Points_x[len - 1]) == 0 && Mathf.Abs(pos.y - Points_y[len - 1]) == 0))
                    {
                        Points_x.Add(pos.x);
                        Points_y.Add(pos.y);
                    }
                        
                        
                }
                else
                {
                    Points_x.Add(pos.x);
                    Points_y.Add(pos.y);
                }
                
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            
            float[] Codis_arry = new float[3];//Array for C0 - dis values(We have 3 formulas yet,So,3 values)
            float[] Points_Arry_x = Points_x.ToArray();//Array of X-coordinate
            float[] Points_Arry_y = Points_y.ToArray();//Array of Y-coordinate
            Points_Arry_x = Normalisation(Points_Arry_x);
            Points_Arry_y = Normalisation(Points_Arry_y);
            ClearParent(pointParent);
            Points_x.Clear();
            Points_y.Clear();
            NumberPredictor comp = GetComponent<NumberPredictor>();
            string index = indextoAplha(Predictor(CoDistance(Points_Arry_x,Points_Arry_y), comp.GetCoDis()));
            Txtmesh.text += index; 
        }
    }

    public void CreateFile(float[] x,float[] y)
    {
        //path of file
        string path = Application.dataPath + "/X_Y_test2.txt";
        //if file doesn't exist
        if (!File.Exists(path))
        {
            File.Create(path);
        }
        //adding in file
        File.AppendAllText(path,"\n" + "################################################################");
        for (int i = 0; i < x.Length; i++)
        {
            File.AppendAllText(path, "\n" + x[i].ToString() + "," + y[i].ToString());
        }
        
    }

    float[][] CoDistance(float[] x, float[] y) {

        float[][] Arry = ZeroArray(20,20);
        float[][] CodisArry = ZeroArray(4,4);
       
        for(int i = 0;i < x.Length; i++)
        {
            int n = 10 - (int)y[i];
            int m = 10 + (int)x[i];
            if (10 - (int)y[i] == 20)
            {
                n = 19;
            }
            if (10 + (int)x[i] == 20)
            {
                m = 19;
            }
            Arry[n][m] = (int)1;
        }
        //DebugArray(Arry);
        for (int row = 0; row < 20; row++)
        {
            for (int column = 0; column < 20; column++)
            {
                CodisArry[(int)row / 5][(int)column / 5] += Arry[row][column];
            }
        }
        //DebugArray(CodisArry);
        return CodisArry;
    }

    void DebugArray(float[][] Arry)
    {
        Debug.Log("############################");
        string r;
        for (int row = 0; row < Arry.Length; row++)
        {
            r = "";
            for (int column = 0; column < Arry[0].Length; column++)
            {
                r += Arry[row][column].ToString() + ","; 
            }
            
            Debug.Log(r);
        }
        Debug.Log("############################");
    }
    float[][] ZeroArray(int row,int column)
    {
        float[][] Arry = new float[row][];
        for (int rows = 0; rows < row; rows++)
        {
            Arry[rows] = new float[column];
        }
        for (int r = 0; r < row; r++)
        {
            for (int c = 0; c < column; c++)
            {
                Arry[r][c] = 0;
            }
        }
        return Arry;
    }
    int Predictor(float[][] CodisArry,string[][][] CodisData)
    {
       
        float[] RMSE = new float[26];
        for (int i = 0; i < CodisData.Length; i++)
        {
            RMSE[i] = ComputingRMSE(CodisArry,CodisData[i]);
            
        }
        return GetAlphaMinRmse(RMSE);
    }
    public void InsertionSort(float[] input)
    {
        for (int i = 0; i < input.Length; i++)
        {
            var item = input[i];
            var currentIndex = i;
            while (currentIndex > 0 && input[currentIndex - 1] > item)
            {
                input[currentIndex] = input[currentIndex - 1];
                currentIndex--;
            }
            input[currentIndex] = item;
        }
    }
    int GetAlphaMinRmse(float[] RmseArry)
    {
        int min_index = 0;
        float min = RmseArry[0];
        for (int i = 0; i < RmseArry.Length; i++)
        {
            if (RmseArry[i] < min)
            {
                min = RmseArry[i];
                min_index = i;
            }
        }
        float[] Rmse_or = RmseArry;
        
        InsertionSort(RmseArry);
        float median = RmseArry[2];

        
        for (int i = 0;i < 5; i++)
        {
            Debug.Log("RMSE"+ (i+1) + " : " + RmseArry[i].ToString());
        }
        Debug.Log("#########################################");
        float probability = ((RmseArry[1] - RmseArry[0])/(median - RmseArry[0]));
        CodisTextMesh[1].text = "P(Word = " + indextoAplha(min_index) + ")";
        CodisTextMesh[0].text = probability.ToString();

        return min_index;
    }

    float ComputingRMSE(float[][] Target, string[][] data)
    {
        float sum = 0;
        for (int r = 0; r < Target.Length; r++)
        {
            for (int c = 0; c < Target[0].Length; c++)
            {
                sum += Mathf.Pow(Target[r][c] - float.Parse(data[r][c]), 2);
            }
        }
        return sum;
    }
    float[] Normalisation(float[] ary)
    {
        float[] Arry = new float[ary.Length];
        for (int i = 0;i < ary.Length; i++){
            Arry[i] = (20 * (ary[i] - Min_Max(ary)) / (Min_Max(ary, 1) - Min_Max(ary))) - 10;
        }
        return Arry;
    }
    float Min_Max(float[] ary,int m = 0)
    {
        float min = ary[0];
        float max = ary[0];
        for (int i = 0;i < ary.Length; i++)
        {
            if (ary[i] > max)
            {
                max = ary[i];
            }
            if (ary[i] < min)
            {
                min = ary[i];
            }
        }
        if (m == 0)
        {
            return min;
        }
        else
        {
            return max;
        }
    }
    bool PressedButton(int buttonNo, ref bool ref1)
    {
        if (Input.GetMouseButtonDown(buttonNo))
        {
            ref1 = true;
        }
        if (Input.GetMouseButtonUp(buttonNo))
        {
            ref1 = false;
        }
        return ref1;
    }
    bool Timer(float time, ref float reference)
    {
        reference += Time.deltaTime;
        if (reference > time)
        {
            reference = 0;
            return true;
        }
        return false;
    }
    string indextoAplha(int index)
    {
        string[] Alpha = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
        return Alpha[index];
    }

    void ClearParent(Transform parent)
    {
        for(int i = 0;i < parent.childCount; i++)
        {
            Destroy(parent.GetChild(i).gameObject);
        }
    }
}
