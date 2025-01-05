using UnityEngine;

public class OtherPageController : MonoBehaviour
{
    public GameObject gridPage; // Reference to the Grid Page
    public GameObject otherPage; // Reference to this (Other Page)
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
            otherPage.SetActive(false); // Hide this page
            gridPage.SetActive(true);  // Show the Grid Page
        }
        else
        {
            Debug.Log("Reached maximum clicks. Stopping loop.");
            // Optionally disable the button or handle the end of the loop
            this.enabled = false; // Disable this script to prevent further clicks
        }
    }
}
