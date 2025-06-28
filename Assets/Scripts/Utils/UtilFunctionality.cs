using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq; // Required for Sum()

public class UtilFunctionality
{
    // Writing all calculation functions here 

    public int calculateIntroversionScore(int[] answers, List<int> multiAnswerQuestion)
    {
        if (answers == null || answers.Length < 7)
        {
            Debug.LogError("Answers are empty.");
            return -1;
        }

        int introversionScore = 0;

        // Comfort Questions (c1, c2, c3) - Lower confidence = Higher introversion
        introversionScore += answers[2]; // "How comfortable are you with using VR technology alone?"
        Debug.Log("Introversion Score Q1: " + introversionScore);
        introversionScore += answers[3]; // "How confident are you in your ability to manage stressful situations on your own?"
        Debug.Log("Introversion Score Q2: " + introversionScore);
        introversionScore += answers[4]; // "What do you do when you begin to feel anxious?"
        Debug.Log("Introversion Score Q3: " + introversionScore);

        // Triggers Questions (c4, c5, c6) - Higher struggle = Higher introversion
        introversionScore += answers[5]; // "How often do you feel overwhelmed when speaking in front of people?"
        Debug.Log("Introversion Score Q4: " + introversionScore);
        introversionScore += answers[6]; // "When asked to make a decision under time pressure, how do you react?"
        Debug.Log("Introversion Score Q5: " + introversionScore);

        // Physical Symptoms Question (c6) - Multi-select, sum of all selected options
        int multiQuestionScore = multiAnswerQuestion.Sum();
        Debug.Log("Introversion Score Q6: " + multiQuestionScore);
        introversionScore += multiQuestionScore; // "Do you experience any of the following physical symptoms when you're anxious?"

        Debug.Log("Total Introversion Score: " + introversionScore);
        return introversionScore;
    }

    // --------------------------------------------------------------------------------------------------- //
    // -----------------------------   DASHBOARD FUNCTIONS   --------------------------------------------- //
    // --------------------------------------------------------------------------------------------------- //

    public float calculateConfidencePercentage(float previousScore, float currentScore, int totalScenariosPreviously)
    {
        // This function is supposed to return the new average confidence score
        // It multiplies the old average by total attempted scenarios to generate the previous average score
        // Adds the new average and divides by the new total scenarios to get new average score
        float oldAvg = previousScore * totalScenariosPreviously;
        float newTotal = oldAvg + currentScore;
        return newTotal / (totalScenariosPreviously + 1);
    }

    public float calculateClarityPercentage(float previousScore, float currentScore, int totalScenariosPreviously)
    {
        if (totalScenariosPreviously == 0)
            return currentScore * 10f;  // since currentScore is out of 10, this returns % format


        // This functions calculates the percentage of overall clarity in Users voice
        // Clarity score is given out of 10 so have to convert it
        // PreviousScore would be percent so we'll have to convert it to average score, multiple by total to get totalScore previously,
        // add our current, find new avg, and multiply by 100 to get percentage
        float oldAvg = previousScore / 10f; // would convert 80% to 8 score average
        float oldAvgTotal = oldAvg * totalScenariosPreviously;  // would give us old avg ie 8*10 scenarios = 80
        float newAvgTotal = oldAvgTotal + currentScore;
        float newAvgScore = newAvgTotal / (totalScenariosPreviously + 1);

        return newAvgScore * 10f;   // convertint to percentage before returning
    }

    public float calculateBlockerPercentage(int totalScenariosBlocked, int totalScenariosAttempted)
    {
        if (totalScenariosAttempted == 0) 
            return 0;
        return (float)totalScenariosBlocked / (float)totalScenariosAttempted * 100f;
    }

}
