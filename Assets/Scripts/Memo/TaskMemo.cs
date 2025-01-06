using UnityEngine;
using System.Collections.Generic;

public class TaskMemo : MonoBehaviour
{
    public MainMemo mainMemo;
    
    void OnEnable()
    {
        mainMemo.IconGenerate(mainMemo.currentGrid);
        StartCoroutine(mainMemo.ReturnToOtherPage());
    }
}