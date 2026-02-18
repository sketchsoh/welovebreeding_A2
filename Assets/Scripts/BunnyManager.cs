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
    [SerializeField] private int startingBunnyCount = 5;

    [Header("Bunny Sprites")]
    [Header("Gradient")]
    [SerializeField] private Sprite[] gradientHeadSprites;
    [SerializeField] private Sprite[] gradientBodySprites;
    [SerializeField] private Sprite[] gradientEarSprites;
    [SerializeField] private Sprite[] gradientTailSprites;
    [Space]
    [Header("Normal")]
    [SerializeField] private Sprite[] normalHeadSprites;
    [SerializeField] private Sprite[] normalBodySprites;
    [SerializeField] private Sprite[] normalEarSprites;
    [SerializeField] private Sprite[] normalTailSprites;
    [Space]
    [Header("Spotted")]
    [SerializeField] private Sprite[] spottedHeadSprites;
    [SerializeField] private Sprite[] spottedBodySprites;
    [SerializeField] private Sprite[] spottedEarSprites;
    [SerializeField] private Sprite[] spottedTailSprites;

    [SerializeField] private GameObject bunnyPrefab;
    
    private const float minXBounds = -0.4f;
    private const float maxXBounds = 6.5f;
    private const float minYBounds = -1.15f;
    private const float maxYBounds = 3f;

    void Start()
    {
        GenerateStartingBunnies();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GenerateBunny();
        }
    }
    

    public void ShowStatsPanel(string name, float cutenessStat, float playfulnessStat, float affectionStat)
    {
        statsPanel.SetActive(true);
        bunnyName.text = name;
        bunnyCuteness.value = cutenessStat / 100;
        bunnyPlayfulness.value = playfulnessStat / 100;
        bunnyFriendliness.value = affectionStat / 100;
    }

    public void HideStatsPanel()
    {
        statsPanel.SetActive(false);
    }

    public void GenerateBunny()
    {
        int rand =  Random.Range(0, 3);
        BunnyType selectedBunnyType = (BunnyType)rand;
        Sprite bunnyHead = null;
        Sprite bunnyBody = null;
        Sprite bunnyEar =  null;
        Sprite bunnyTail =  null;
        switch (selectedBunnyType)
        {
            case BunnyType.Normal:
                bunnyHead = normalHeadSprites[Random.Range(0, normalHeadSprites.Length)];
                bunnyBody = normalBodySprites[Random.Range(0, normalBodySprites.Length)];
                bunnyEar = normalEarSprites[Random.Range(0, normalEarSprites.Length)];
                bunnyTail = normalTailSprites[Random.Range(0, normalTailSprites.Length)];
                break;
            case BunnyType.Spotted:
                bunnyHead = spottedHeadSprites[Random.Range(0, normalHeadSprites.Length)];
                bunnyBody = spottedBodySprites[Random.Range(0, spottedBodySprites.Length)];
                bunnyEar = spottedEarSprites[Random.Range(0, spottedEarSprites.Length)];
                bunnyTail = spottedTailSprites[Random.Range(0, spottedTailSprites.Length)];
                break;
            case BunnyType.Gradient:
                bunnyHead = gradientHeadSprites[Random.Range(0, gradientHeadSprites.Length)];
                bunnyBody = gradientBodySprites[Random.Range(0, gradientBodySprites.Length)];
                bunnyEar = gradientEarSprites[Random.Range(0, gradientEarSprites.Length)];
                bunnyTail = gradientTailSprites[Random.Range(0, gradientTailSprites.Length)];
                break;
        }

        Color bunnyColor = Random.ColorHSV(0.0f, 1.0f, 0.3f, 0.75f, 0.75f, 1.0f);
        Bunny newBunny = Instantiate(bunnyPrefab, transform.position, Quaternion.identity).GetComponent<Bunny>();
        float cutenessStat = Random.Range(0f, 100f);
        float playfulnessStat = Random.Range(0f, 100f);
        float friendlinessStat = Random.Range(0f, 100f);
        newBunny.InitializeBunny("Test", selectedBunnyType, bunnyColor, bunnyTail, bunnyBody, bunnyHead, bunnyEar, cutenessStat, playfulnessStat, friendlinessStat);
        newBunny.transform.position = new Vector3(Random.Range(minXBounds, maxXBounds), Random.Range(minYBounds, maxYBounds));
    }

    private void GenerateStartingBunnies()
    {
        for (int i = 0; i < startingBunnyCount; i++)
        {
            GenerateBunny();
        }
    }
}

public enum BunnyType
{
    Gradient,
    Normal,
    Spotted
}
