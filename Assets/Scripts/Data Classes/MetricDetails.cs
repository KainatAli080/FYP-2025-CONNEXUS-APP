using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Stores the extracted details for each metric
// Being used for Confidence Building Easy For now
// MetricDetails.cs (if you want separate files)
public class MetricDetails
{
    public int Score; // 0-10 scale
    public string Metric; // e.g., "Too Soft" for Volume
    public string Description; // Explanation from API
}
