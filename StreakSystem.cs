using System;
using UnityEngine;
using UnityEngine.UI;

public class StreakSystem : MonoBehaviour
{

    public Text streakText;
    public DailyGoal dg;
    public bool hasRun = false;

    private int streakCount = 0;
    private DateTime lastCompletedDate;

    private const string STREAK_COUNT_KEY = "StreakCount";
    private const string LAST_COMPLETED_DATE_KEY = "LastCompletedDate";

    //Implementation of streak system
    private void Start()
    {
        // Load the streak count and last completed date from PlayerPrefs
        streakCount = PlayerPrefs.GetInt(STREAK_COUNT_KEY, 0);
        long ticks = 0;
        lastCompletedDate = new DateTime(ticks);
        if (long.TryParse(PlayerPrefs.GetString(LAST_COMPLETED_DATE_KEY, "0"), out ticks))
        {
            lastCompletedDate = new DateTime(ticks);
        }
        else
        {
            // Set the last completed date to epoch time if it cannot be parsed
            lastCompletedDate = new DateTime(0);
        }
    }
    private void Update()
    {
        // Check if tasks were completed today

        if (dg.exerciseComplete)
        {
            // Check if the last completed date was yesterday
            if (lastCompletedDate.Date == DateTime.Today.AddDays(-1))
            {
                if (!hasRun)
                {
                    // Increment the streak count
                    streakCount++;
                    hasRun = true;
                }
                
            }
            else if (lastCompletedDate.Date < DateTime.Today.AddDays(-1))
            {
                // Reset the streak count to 1 if the last completed date was before yesterday
                streakCount = 1;
            }

            // Save the streak count and last completed date to PlayerPrefs
            PlayerPrefs.SetInt(STREAK_COUNT_KEY, streakCount);
            PlayerPrefs.SetString(LAST_COMPLETED_DATE_KEY, DateTime.Now.Ticks.ToString());
            PlayerPrefs.Save();
            
        }
        else
        {
            // Reset the streak count to 0 if tasks were not completed for three consecutive days
            if (DateTime.Today.Subtract(lastCompletedDate.Date).Days >= 3)
            {
                streakCount = 0;

                // Save the streak count and last completed date to PlayerPrefs
                PlayerPrefs.SetInt(STREAK_COUNT_KEY, streakCount);
                PlayerPrefs.SetString(LAST_COMPLETED_DATE_KEY, DateTime.Now.Ticks.ToString());
                PlayerPrefs.Save();
            }
        }

        streakText.text = "Streak: " + streakCount;
    }
}
