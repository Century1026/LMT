using UnityEngine;
using TMPro;

public class DualCountdownTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText1;
    public TextMeshProUGUI timerText2;
    public float countdownTime1 = 5f;
    public float countdownTime2 = 10f;
    public bool isTimer1Active = true;
    public PageManager PageManager;

    void Update()
    {
        timerText2.text = "Time remaining: " + $"<color=red>{countdownTime2}</color>";
        if (isTimer1Active)
        {
            if (countdownTime1 > 0)
            {
                countdownTime1 -= Time.deltaTime;
                timerText1.text = $"Start in {Mathf.Max(0, countdownTime1):F1}";
            }
            else
            {
                isTimer1Active = false;
                timerText1.text = "Go";
                timerText1.color = Color.green;
            }
        }
        else
        {
            if (countdownTime2 > 0)
            {
                countdownTime2 -= Time.deltaTime;
                timerText2.text = "Time remaining: " + $"<color=red>{Mathf.Max(0, countdownTime2):F1}</color>";
            }
            else
            {
                PageManager.NavigateToNextPage();
            }
        }
    }
}
