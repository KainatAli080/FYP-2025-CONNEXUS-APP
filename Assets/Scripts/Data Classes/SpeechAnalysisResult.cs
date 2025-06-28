using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// data class
// Stores the final structured data after processing
public class SpeechAnalysisResult
{
    public int ConfidenceScore { get; set; }  // Extracted confidence score
    public string OverallFeedback { get; set; }  // Extracted overall feedback
    public Dictionary<string, MetricDetails> Metrics { get; set; }  // Processed metric details
}
