using UnityEngine;

public class PromptMemo : MonoBehaviour
{
    public MainMemo mainMemo;
    public GameObject imageContainer;

    void OnEnable()
    {
        mainMemo.IconGenerate(imageContainer);
    }
}