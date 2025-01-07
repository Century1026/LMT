using UnityEngine;

public class TaskMemo : MonoBehaviour
{
    public MainMemo mainMemo;
    
    void OnEnable()
    {
        mainMemo.IconGenerate(mainMemo.currentGrid);
        StartCoroutine(mainMemo.ReturnToOtherPage());
    }
}