using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DashboardScript : MonoBehaviour
{
    [SerializeField] private string userPoints;
    [SerializeField] private TextMeshProUGUI pointsTextField;

    [SerializeField] private TextMeshProUGUI dailyTaskTextfield;
    public List<string> possibleTasks = new List<string>();
    private string currentTask = "";
    private bool taskActive = false;

    void Start()
    {
        initializeTasks();
        StartCoroutine(assignTaskForTwoMinutes());

        FirebaseFunctions.instance
            .GetUserPoints((points) => {
            pointsTextField.text = points.ToString();
        });
    }

    // To set the tasks first
    void initializeTasks()
    {
        possibleTasks.Clear();

        // Confidence Building
        possibleTasks.Add("Stand in front of a mirror and say your full name confidently.");
        possibleTasks.Add("Write down one thing you’re proud of today.");
        possibleTasks.Add("Walk through a space with your head up for 30 seconds.");

        // Decision Making
        possibleTasks.Add("Choose what to eat today without asking anyone.");
        possibleTasks.Add("Say 'no' to something today without overexplaining.");
        possibleTasks.Add("Pick a small task and commit to it today.");

        // Group Interaction
        possibleTasks.Add("Send a message or emoji in a group chat.");
        possibleTasks.Add("Contribute once in a group setting, even with a nod.");
        possibleTasks.Add("Share a small opinion in a safe space online.");
    }

    // Assigning daily task for 2 min instead of 24 hours for testing purpoases
    IEnumerator assignTaskForTwoMinutes()
    {
        if (taskActive)
            yield break; // Prevent multiple assignments

        taskActive = true;

        // Pick a random task
        int randomIndex = Random.Range(0, possibleTasks.Count);
        currentTask = possibleTasks[randomIndex];
        Debug.Log("Assigned Task: " + currentTask);

        // Displaying task in UI here
        dailyTaskTextfield.text = currentTask.ToString();

        // Wait for 2 minutes
        yield return new WaitForSeconds(120f);

        Debug.Log("Task duration ended.");
    }
}
