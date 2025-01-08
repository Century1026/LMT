using UnityEngine;

public class TaskMemo : MonoBehaviour
{
    public MainMemo mainMemo;
    public GameObject imageContainer;
    
    void OnEnable()
    {
        imageContainer = mainMemo.currentGrid;
        mainMemo.IconGenerate(imageContainer);
        StartCoroutine(mainMemo.ReturnToOtherPage());
    }
}