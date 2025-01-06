using UnityEngine;
using System.Collections.Generic;

public class PromptMemo : MonoBehaviour
{
    public GameObject imagePrefab;
    public GameObject imageContainer;
    public PageManagerMemo pageManager;
    public MainMemo mainMemo;
    private int trialCount;

    void OnEnable()
    {
        trialCount = pageManager.trialCount;
        IconsDisplay(mainMemo.trial);
    }

    void IconsDisplay(Dictionary<string, GameObject> iconPositionPairs)
    {
        var keys = new List<string>(iconPositionPairs.Keys);
        mainMemo.IconGenerate(imagePrefab, imageContainer, keys[trialCount]);
    }
}