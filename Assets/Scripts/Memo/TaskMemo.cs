using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TaskMemo : MonoBehaviour
{
    public GameObject imagePrefab;
    public GameObject pagePrompt;
    public GameObject pageTask;
    public PageManagerMemo pageManager;
    public MainMemo mainMemo;
    public float displayDuration = 1f;
    private int trialCount;
    
    void OnEnable()
    {
        trialCount = pageManager.trialCount;
        IconsDisplay(mainMemo.trial);
        StartCoroutine(ReturnToOtherPage());
    }

    void IconsDisplay(Dictionary<string, GameObject> iconPositionPairs)
    {
        var keys = new List<string>(iconPositionPairs.Keys);
        mainMemo.IconGenerate(imagePrefab, iconPositionPairs[keys[trialCount]], keys[trialCount]);
    }

    IEnumerator ReturnToOtherPage()
    {
        yield return new WaitForSeconds(displayDuration);
        Destroy(mainMemo.gridCells[trialCount].transform.GetChild(0).gameObject);
        pageTask.SetActive(false);  // Hide the Grid Page
        if (trialCount < pageManager.countMax - 1)
            pagePrompt.SetActive(true); // Show the Other Page
        else
            Debug.Log("Reached maximum clicks. Stopping loop.");
    }
}