using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;

public class FileReader : MonoBehaviour
{
    string line;
    string [] numberString = new string[20];
    int temp = 0;
    string[] exercise = new string[20];
    public float loadedExtensionAngle;
    public float loadedFlexionAngle;
    public float loadedRadialAngle;
    public float loadedUlnarAngle;
    public float loadedPronationAngle;
    public float loadedSupinationAngle;

    // Start is called before the first frame update
    public void ReadFile()
    {
        var fileName = "C:\\Unity Projects\\Wrist Rehabilitation Framework\\LoadFile\\Threshold_Angle.txt";
        using FileStream streamFile = File.OpenRead(fileName);
        using var readStream = new StreamReader(streamFile);
        temp = 0;

        while ((line = readStream.ReadLine()) != null)
        {
            exercise[temp] = line;
            temp++;
        }

        numberString[0] = Regex.Match(exercise[0], @"\d+").Value;
        loadedExtensionAngle = float.Parse(numberString[0]);

        numberString[1] = Regex.Match(exercise[1], @"\d+").Value;
        loadedFlexionAngle = float.Parse(numberString[1]);

        numberString[2] = Regex.Match(exercise[2], @"\d+").Value;
        loadedRadialAngle = float.Parse(numberString[2]);

        numberString[3] = Regex.Match(exercise[3], @"\d+").Value;
        loadedUlnarAngle = float.Parse(numberString[3]);

        numberString[4] = Regex.Match(exercise[4], @"\d+").Value;
        loadedPronationAngle = float.Parse(numberString[4]);

        numberString[5] = Regex.Match(exercise[5], @"\d+").Value;
        loadedSupinationAngle = float.Parse(numberString[5]);

    }
}
