using TMPro;
using UnityEngine;

public class ClickCounter : MonoBehaviour
{
    public DualCountdownTimer countdownTimer;
    public TextMeshProUGUI counterText;
    private int clickCount = 0;

    public void IncrementClickCount()
    {
        if (!countdownTimer.isTimer1Active)
        {
            clickCount++;
            counterText.text = "Clicks: " + clickCount;
        }
    }
}