using System;
using System.Collections.Generic;using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BunnyManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject statsPanel;
    [SerializeField] private TextMeshProUGUI bunnyName;
    [SerializeField] private Image bunnyIcon;
    [SerializeField] private Slider bunnyCuteness;
    [SerializeField] private Slider bunnyPlayfulness;
    [SerializeField] private Slider bunnyFriendliness;
    [SerializeField] private TextMeshProUGUI traitPanel;
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

    private TraitData traitList;
    [SerializeField] private TextAsset traitData;
    
    private const float minXBounds = -0.4f;
    private const float maxXBounds = 6.5f;
    private const float minYBounds = -1.15f;
    private const float maxYBounds = 3f;

    void Start()
    {
        traitList = new TraitData();
        ReadFromJson();
        GenerateStartingBunnies();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GenerateBunny();
        }
    }
    

    public void ShowStatsPanel(string name, float cutenessStat, float playfulnessStat, float affectionStat, List<TraitType> traits)
    {
        statsPanel.SetActive(true);
        bunnyName.text = name;
        bunnyCuteness.value = cutenessStat / 100;
        bunnyPlayfulness.value = playfulnessStat / 100;
        bunnyFriendliness.value = affectionStat / 100;
        string traitListString = "";
        foreach (TraitType trait in traits)
        {
            traitListString += trait.traitName + "\n";
        }
        traitPanel.text = traitListString;
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

        Color bunnyColor = Random.ColorHSV(0.0f, 1.0f, 0.1f, 0.4f, 0.7f, 1.0f);
        Bunny newBunny = Instantiate(bunnyPrefab, transform.position, Quaternion.identity).GetComponent<Bunny>();
        List<TraitType> newTraitList = new List<TraitType>();
        newTraitList.Add(traitList.traitList[Random.Range(0, traitList.traitList.Count)]);
        newTraitList.Add(traitList.traitList[Random.Range(0, traitList.traitList.Count)]);
        newTraitList.Add(traitList.traitList[Random.Range(0, traitList.traitList.Count)]);
        string newBunnyName = bunnyNames[Random.Range(0, bunnyNames.Count)];
        bunnyNames.Remove(newBunnyName);
        newBunny.InitializeBunny(newBunnyName, selectedBunnyType, bunnyColor, bunnyTail, bunnyBody, bunnyHead, bunnyEar, newTraitList);
        newBunny.transform.position = new Vector3(Random.Range(minXBounds, maxXBounds), Random.Range(minYBounds, maxYBounds));
    }

    private void GenerateStartingBunnies()
    {
        for (int i = 0; i < startingBunnyCount; i++)
        {
            GenerateBunny();
        }
    }

    private void ReadFromJson()
    {
        traitList = JsonUtility.FromJson<TraitData>(traitData.text);
    }

    private List<string> bunnyNames = new List<string>()
    {
        "USAGI",
        "BABY",
        "CARRIE", 
        "WEI", 
        "JUN", 
        "HAO", 
        "ADRIAN", 
        "LORENZO", 
        "YUKI", 
        "CRESPY", 
        "CREME", 
        "OLIVE", 
        "HAMMY", 
        "EARS",
        "MOLLY", 
        "POPPY", 
        "MADDIE", 
        "RUE", 
        "LEXIE", 
        "GEORGE", 
        "MIKE", 
        "KEVIN", 
        "SOO", 
        "PRINCE", 
        "TAMMY", 
        "LIVIE",
        "MUSH", 
        "KAT", 
        "DAWG", 
        "SANA"
    };
}

public enum BunnyType
{
    Gradient,
    Normal,
    Spotted
}

[Serializable]
public class TraitType
{
    public string traitName = "";
    public int personalitySet = 0;
    public float playfulnessAdjustment = 0.0f;
    public float cutenessAdjustment = 0.0f;
    public float friendlinessAdjustment = 0.0f;
}

[Serializable]
public class TraitData
{
    public List<TraitType> traitList =  new List<TraitType>();
}