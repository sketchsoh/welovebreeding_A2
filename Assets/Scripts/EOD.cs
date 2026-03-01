using LitMotion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EOD : MonoBehaviour
{
    public Slider publicSlider;
    public Slider cultSlider;
    public TextMeshProUGUI soldText;
    void Start()
    {
        soldText.text = "RABBITS SOLD: " + GameManager.Instance.bunniesSold;
        LSequence.Create()
            .Append(LMotion
                .Create(GameManager.Instance.prevPublicPerception / 100f, GameManager.Instance.currPublicPerception / 100f, 0.5f)
                .Bind(x => publicSlider.value = x))
            .Append(LMotion
                .Create(GameManager.Instance.prevCultPerception / 100f, GameManager.Instance.currCultPerception / 100f, 0.5f)
            .Bind(x => cultSlider.value = x))
            .Run();
    }

    public void ButtonPress()
    {
        GameManager.Instance.GoNextDay();
    }
}
