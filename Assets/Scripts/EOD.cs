using LitMotion;
using UnityEngine;
using UnityEngine.UI;

public class EOD : MonoBehaviour
{
    public Slider publicSlider;
    public Slider cultSlider;
    void Start()
    {
        LSequence.Create()
            .Append(LMotion
                .Create(GameManager.Instance.prevPublicPerception, GameManager.Instance.currPublicPerception, 0.5f)
                .Bind(x => publicSlider.value = x))
            .Append(LMotion
                .Create(GameManager.Instance.prevCultPerception, GameManager.Instance.currCultPerception, 0.5f)
            .Bind(x => cultSlider.value = x))
            .Run();
    }

}
