using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MemoPageManager : MonoBehaviour
{
    public GameObject otherPage;
    public GameObject gridPage;
    public Button otherPageButton; // Assign the button from the Other Page
    private int loopCount;

    void Start()
    {
        GridTrialGenerator gridTrialGenerator = Object.FindFirstObjectByType<GridTrialGenerator>();
        if (gridTrialGenerator != null)
        {
            int repeats = gridTrialGenerator.levelSelection; // Assuming levelSelection determines maxRepeats
            loopCount = (int)Mathf.Pow(repeats + 1, 2); // Square of levelSelection
        }
        else
        {
            Debug.LogError("GridTrialGenerator script not found. Defaulting maxRepeats to 5.");
            loopCount = 4;
        }
        
        otherPage.SetActive(true);
        gridPage.SetActive(false);

        // Add a listener to the button
        otherPageButton.onClick.AddListener(OnOtherPageButtonClicked);
    }

    void OnOtherPageButtonClicked()
    {
        StartCoroutine(TransitionPages());
    }

    IEnumerator TransitionPages()
    {
        for (int i = 0; i < loopCount; i++)
        {
            // Show the Grid Page
            otherPage.SetActive(false);
            gridPage.SetActive(true);
            yield return new WaitForSeconds(1f);

            // Show the Other Page
            gridPage.SetActive(false);
            otherPage.SetActive(true);
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0)); // Wait for button click
        }

        // End of the loop logic
        Debug.Log("Page loop finished.");
    }
}
