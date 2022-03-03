using UnityEngine;
using System.IO;

public class TextFile : MonoBehaviour
{
    public void Start()
    {
        CreateFile("########################################################################################################################");
    }
    public void CreateFile(string str)
    {
        //path of file
        string path = Application.dataPath + "/X_Y.txt";
        //if file doesn't exist
        if (!File.Exists(path))
        {
            File.Create(path);
        }
        //adding in file
        File.AppendAllText(path,"\n" + str);
    }
}
