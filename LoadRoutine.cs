using System.IO;
using UnityEngine;

public class LoadRoutine : MonoBehaviour
{
    [SerializeField]
    private DailyGoal dg;

    public float loadedExtensionAngle;
    public float loadedFlexionAngle;
    public float loadedRadialAngle;
    public float loadedUlnarAngle;
    public float loadedPronationAngle;
    public float loadedSupinationAngle;
    private int extensionGoal;
    private int radialGoal;
    private int pronationGoal;
    private int fingerGoal;
    public string csvFileName;
    private string[][] csvData;
    private string filePath = "C:\\Unity Projects\\Wrist Rehabilitation Framework Final\\Wrist Rehabilitation Framework v2\\LoadFile\\LoadRoutine.csv";

    private void Start()
    {
        dg.extensionTarget = PlayerPrefs.GetInt("ExtensionGoalToday");
        dg.radialTarget = PlayerPrefs.GetInt("RadialGoalToday");
        dg.pronationTarget = PlayerPrefs.GetInt("PronationGoalToday");
        dg.fingerTarget = PlayerPrefs.GetInt("FingerGoalToday");
    }
    //Load the threshold settings from a CSV file provided by healthcare professionals
    public void LoadCsv()
    {
        // Open the CSV file
        StreamReader reader = new StreamReader(filePath);

        // Read the CSV file line by line
        string fileContent = reader.ReadToEnd();
        string[] lines = fileContent.Split('\n');

        // Store the values in a two-dimensional array
        csvData = new string[lines.Length][];
        for (int i = 0; i < lines.Length; i++)
        {
            csvData[i] = lines[i].Split(',');
        }

        loadedExtensionAngle = float.Parse(csvData[1][1]);
        loadedFlexionAngle = float.Parse(csvData[1][3]);
        loadedRadialAngle = float.Parse(csvData[2][1]);
        loadedUlnarAngle = float.Parse(csvData[2][3]);
        loadedPronationAngle = float.Parse(csvData[3][1]);
        loadedSupinationAngle = float.Parse(csvData[3][3]);
        extensionGoal = int.Parse(csvData[6][2]);
        radialGoal = int.Parse(csvData[7][2]);
        pronationGoal = int.Parse(csvData[8][2]);
        fingerGoal = int.Parse(csvData[9][2]);


        //get the exercise goal from saved file
        PlayerPrefs.SetInt("ExtensionGoalToday", extensionGoal);
        dg.extensionTarget = PlayerPrefs.GetInt("ExtensionGoalToday");
        PlayerPrefs.SetInt("RadialGoalToday", radialGoal);
        dg.radialTarget = PlayerPrefs.GetInt("RadialGoalToday");
        PlayerPrefs.SetInt("PronationGoalToday", pronationGoal);
        dg.pronationTarget = PlayerPrefs.GetInt("PronationGoalToday");
        PlayerPrefs.SetInt("FingerGoalToday", fingerGoal);
        dg.fingerTarget = PlayerPrefs.GetInt("FingerGoalToday");


        // Close the CSV file
        reader.Close();
    }

    public void PrintCsvData()
    {
        for (int i = 0; i < csvData.Length; i++)
        {
            for (int j = 0; j < csvData[i].Length; j++)
            {
                Debug.Log(csvData[i][j]+"i: " + i + "j: "+j);
            }
        }
    }

}
