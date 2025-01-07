using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class DifficultySlider : MonoBehaviour
{
    public TextMeshProUGUI difficultyText;
    public Slider slider;
    void Start()
    {
        slider.wholeNumbers = true;
        slider.minValue = 0;
        slider.maxValue = 2;

        UpdateDifficulty();
    }

    public void UpdateDifficulty()
    {
        switch ((int)slider.value)
        {
            case 0:
                difficultyText.text = "Easy";
                difficultyText.color = Color.green;
                break;
            case 1:
                difficultyText.text = "Medium";
                difficultyText.color = Color.yellow;
                break;
            case 2:
                difficultyText.text = "Hard";
                difficultyText.color = Color.red;
                break;
        }
    }
}