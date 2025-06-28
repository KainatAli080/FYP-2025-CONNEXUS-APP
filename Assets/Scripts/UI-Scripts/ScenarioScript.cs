using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScenarioScript : MonoBehaviour
{
    [Header("UI References Confidence Easy")]
    public ToggleGroup toggleGroup;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI metricText;
    public TextMeshProUGUI descriptionText;

    [Header("UI References Decision Medium")]
    public ToggleGroup toggleGroup_DM;
    public TextMeshProUGUI scoreText_DM;
    public TextMeshProUGUI metricText_DM;
    public TextMeshProUGUI descriptionText_DM;

    private SpeechAnalysisResult speechResult;

    [SerializeField] private AppNavigationScript _NavScript;

    public async void retrieveMostRecentData_ConfidenceEasy()
    {
        speechResult = await FirebaseFunctions.instance.RetrieveConfidenceEasyData();
        _NavScript.fromConfidenceEasyToConfidenceEasyResult();

        if (speechResult == null)
        {
            Debug.LogError("No results Retrieved");
            return;
        }
        Debug.Log($"Metrics dictionary contains {speechResult.Metrics.Count} items.");
        UpdateUI_ConfidenceEasy("Feedback"); // Default to confidence on load
    }

    //public async void retrieveMostRecentData_DecisionMedium()
    //{
    //    speechResult = await FirebaseFunctions.instance.RetrieveDecisionMediumData();
    //    _NavScript.fromDecisionMediumToDecisionMediumResult();

    //    if (speechResult == null)
    //    {
    //        Debug.LogError("No results Retrieved");
    //        return;
    //    }
    //    Debug.Log($"Metrics dictionary contains {speechResult.Metrics.Count} items.");
    //    UpdateUI_DecisionMedium("Feedback"); // Default to confidence on load
    //}

    public void OnToggleChanged()
    {
        Toggle activeToggle = toggleGroup.GetFirstActiveToggle();
        if (activeToggle != null)
        {
            string selectedMetric = activeToggle.name; // Name of the toggle should match dictionary key
            UpdateUI_ConfidenceEasy(selectedMetric);
        }
    }

    private void UpdateUI_ConfidenceEasy(string key)
    {
        if (speechResult == null) return;

        if (key == "Feedback")
        {
            scoreText.text = $"Score: {speechResult.ConfidenceScore}/100";
            metricText.text = "Overall Confidence Feedback";
            descriptionText.text = speechResult.OverallFeedback;
        }
        else if (speechResult.Metrics.TryGetValue(key, out MetricDetails metric))
        {
            scoreText.text = $"Score: {metric.Score}/10";
            metricText.text = metric.Metric;
            descriptionText.text = metric.Description;
        }
    }

    //private void UpdateUI_DecisionMedium(string key)
    //{
    //    if (speechResult == null) return;

    //    if (key == "Feedback")
    //    {
    //        scoreText_DM.text = $"Score: {speechResult.ConfidenceScore}/100";
    //        metricText_DM.text = "Overall Confidence Feedback";
    //        descriptionText_DM.text = speechResult.OverallFeedback;
    //    }
    //    else if (speechResult.Metrics.TryGetValue(key, out MetricDetails metric))
    //    {
    //        scoreText.text = $"Score: {metric.Score}/10";
    //        metricText.text = metric.Metric;
    //        descriptionText.text = metric.Description;
    //    }
    //}
}
