using TMPro;
using UnityEngine;

public class TextPromptManager : MonoBehaviour
{
    public TextMeshProUGUI promptText;
    public PageManager PageManager;
    public string[] prompts;
    private int currentPromptIndex = 0;

    public void ShowNextPrompt()
    {
        if (currentPromptIndex < prompts.Length)
        {
            promptText.text = prompts[currentPromptIndex];
            currentPromptIndex++;
        }
        else
        {
            PageManager.NavigateToNextPage();
        }
    }
}