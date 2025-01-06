using UnityEngine;

public class PromptMemo : MonoBehaviour
{
    public GameObject imageContainer;
    public MainMemo mainMemo;

    void OnEnable()
    {
        mainMemo.IconGenerate(imageContainer);
    }
}