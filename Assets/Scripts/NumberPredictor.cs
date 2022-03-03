using System.IO;
using UnityEngine;

public class NumberPredictor : MonoBehaviour
{
    string[][][] CodisArry;
    string path;
    // Start is called before the first frame update
    void Start()
    {
        CodisArry = new string[26][][];
        for (int row = 0; row < 26; row ++)
        {
            CodisArry[row] = new string[4][];
            for (int r = 0; r < 4; r++)
            {
                CodisArry[row][r] = new string[4];
            }
        }
        //path of file
        //path = Application.dataPath + "/NumberCoDis.txt";
        path = Application.dataPath + "/X_Y_test.txt";
        string[] readText = File.ReadAllLines(path);

        for (int i = 0; i < readText.Length; i += 4)
        {
            for (int row = 0; row < 4; row++)
            {
               
                CodisArry[(int)i / 4][row] = readText[i + row].Split(":")[1].Split(",");
            }
            //Debug.Log("Here");
            //DebugArray(CodisArry[(int)i / 4]);
        }

    }

    void DebugArray(string[][] Arry)
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

    // Update is called once per frame
    public string[][][] GetCoDis()
    {
        return CodisArry;
    }
    
}
