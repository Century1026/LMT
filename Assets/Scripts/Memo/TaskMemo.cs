using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TaskMemo : MonoBehaviour
{
    public MainMemo mainMemo;
    private int trialCount;
    
    void OnEnable()
    {
        trialCount = mainMemo.trialCount;
        var keys = new List<string>(mainMemo.trial.Keys);
        mainMemo.IconGenerate(mainMemo.trial[keys[trialCount]]); //get the grid container
        StartCoroutine(mainMemo.ReturnToOtherPage());
    }
}