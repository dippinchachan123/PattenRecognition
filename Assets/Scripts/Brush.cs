using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brush : MonoBehaviour
{
    public Transform brush;
    Transform pointParent;
    TextFile textfile;
    //References
    bool ref1 = false;
    float ref2 = 0;


    // Start is called before the first frame update
    void Start()
    {

        pointParent = GameObject.FindGameObjectWithTag("Brush").transform;
        textfile = GetComponent<TextFile>();
    }

    // Update is called once per frame
    void Update()
    {

        if (PressedButton(0,ref ref1))
        {
            if (Timer(0.01f,ref ref2))
            {
                Vector3 pos = Input.mousePosition;
                pos = Camera.main.ScreenToWorldPoint(pos);
                textfile.CreateFile(pos.x.ToString() + "," + pos.y.ToString());
                Transform point = Instantiate(brush, new Vector3(pos.x, pos.y, 0), Quaternion.identity);
                point.name = "Point" + pointParent.childCount.ToString();
                point.parent = pointParent;
            }
            
        }

        
            
        
    }
    
    bool PressedButton(int buttonNo,ref bool ref1)
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
    bool Timer(float time,ref float reference)
    {
        reference += Time.deltaTime;
        if (reference > time)
        {
            reference = 0;
            return true;
        }
        return false;
    }
}
