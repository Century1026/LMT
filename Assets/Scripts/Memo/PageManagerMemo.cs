using UnityEngine;

public class PageManagerMemo : MonoBehaviour
{
    public GameObject pagePrompt; // Reference to this (Other Page)
    public GameObject pageTask; // Reference to the Grid Page
    private int clickCount = 0; // Counter for button clicks
    public int maxClicks; // Maximum number of clicks to stop the loop
    public MainMemo mainMemo;

    public void OnButtonClick()
    {
        if (mainMemo != null)
        {
            int repeats = mainMemo.gridLength; // Assuming levelSelection determines maxRepeats
            maxClicks = (int)Mathf.Pow(repeats, 2); // Square of levelSelection
        }
        else
        {
            Debug.LogError("mainMemo script not found. Defaulting maxRepeats to 5.");
            maxClicks = 4;
        }

        clickCount++;
        if (clickCount <= maxClicks)
        {
            pagePrompt.SetActive(false); // Hide this page
            pageTask.SetActive(true);  // Show the Grid Page
        }
        else
        {
            Debug.Log("Reached maximum clicks. Stopping loop.");
            // Optionally disable the button or handle the end of the loop
            this.enabled = false; // Disable this script to prevent further clicks
        }
    }
}
