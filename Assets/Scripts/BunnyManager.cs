using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BunnyManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject statsPanel;
    [SerializeField] private TextMeshProUGUI bunnyName;
    [SerializeField] private Image bunnyIcon;
    [SerializeField] private Slider bunnyCuteness;
    [SerializeField] private Slider bunnyPlayfulness;
    [SerializeField] private Slider bunnyFriendliness;

    public void ShowStatsPanel(string name, float cutenessStat, float playfulnessStat, float affectionStat)
    {
        statsPanel.SetActive(true);
        bunnyName.text = name;
        bunnyCuteness.value = cutenessStat;
        bunnyPlayfulness.value = playfulnessStat;
        bunnyFriendliness.value = affectionStat;
    }

    public void HideStatsPanel()
    {
        statsPanel.SetActive(false);
    }
}
