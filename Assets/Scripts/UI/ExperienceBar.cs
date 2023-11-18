using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExperienceBar : MonoBehaviour
{
    public Image expFillBar;
    public TextMeshProUGUI levelText;

    private void OnEnable()
    {
        EventHandler.UpdateExpBar += AtUpdateExpBar;
    }

    private void OnDisable()
    {
        EventHandler.UpdateExpBar -= AtUpdateExpBar;
    }

    private void AtUpdateExpBar(int level, float expPercentage)
    {
        levelText.text = level + "Lv";

        expFillBar.fillAmount = expPercentage;
    }
}
