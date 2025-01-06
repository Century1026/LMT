using UnityEngine;

public class PageManagerMemo : MonoBehaviour
{
    public GameObject pagePrompt; // Reference to this (Other Page)
    public GameObject pageTask; // Reference to the Grid Page
    public MainMemo mainMemo;
    public PromptMemo promptMemo;
    public int trialCount = 0; // Counter for button clicks
    public int countMax; // Maximum number of clicks

    public void OnButtonClick()
    {
        countMax = (int)Mathf.Pow(mainMemo.gridLength, 2);
        Destroy(promptMemo.imageContainer.transform.GetChild(0).gameObject);
        pagePrompt.SetActive(false); // Hide this page
        pageTask.SetActive(true);  // Show the Grid Page
        trialCount++;
    }
}