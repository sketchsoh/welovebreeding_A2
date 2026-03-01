using System;
using System.Collections.Generic;
using LitMotion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BunnyManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject statsPanel;
    [SerializeField] private TextMeshProUGUI bunnyName;
    [SerializeField] private TextMeshProUGUI bunnyFertility;
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
    public List<GameObject> bunnies {private set; get;}
    
    private const float minXBounds = -0.4f;
    private const float maxXBounds = 6.5f;
    private const float minYBounds = -1.15f;
    private const float maxYBounds = 3f;

    void Start()
    {
        traitList = new TraitData();
        bunnies = new List<GameObject>();
        ReadFromJson();
        // GenerateStartingBunnies();
        GameManager.Instance.bManager = this;
        GameManager.Instance.cManager = FindFirstObjectByType<CustomerManager>();
        HideStatsPanel();
        GameManager.Instance.StartDay();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
            
    }

    public void ShowStatsPanel(string name, int fertility, float cutenessStat, float playfulnessStat, float affectionStat, List<TraitType> traits)
    {
        statsPanel.SetActive(true);
        bunnyName.text = name;
        LMotion.Create(0f, cutenessStat / 100f, 0.25f)
            .WithEase(Ease.InSine)
            .Bind(x => bunnyCuteness.value = x);
        LMotion.Create(0f, playfulnessStat / 100f, 0.25f)
            .WithEase(Ease.InSine)
            .Bind(x => bunnyPlayfulness.value = x);
        LMotion.Create(0f, affectionStat / 100f, 0.25f)
            .WithEase(Ease.InSine)
            .Bind(x => bunnyFriendliness.value = x);
        // bunnyCuteness.value = cutenessStat / 100;
        // bunnyPlayfulness.value = playfulnessStat / 100;
        // bunnyFriendliness.value = affectionStat / 100;
        string traitListString = "";
        foreach (TraitType trait in traits)
        {
            string cutenessString = (trait.cutenessAdjustment >= 0)? "+" + trait.cutenessAdjustment : trait.cutenessAdjustment.ToString();
            string playfulnessString = (trait.playfulnessAdjustment >=0) ? "+" + trait.playfulnessAdjustment : trait.playfulnessAdjustment.ToString();
            string friendlinessString = (trait.friendlinessAdjustment >= 0) ? "+" + trait.friendlinessAdjustment : trait
                .friendlinessAdjustment.ToString();
            traitListString += "C:" + cutenessString + " P:" + playfulnessString + " F:" + friendlinessString + "    " + trait.traitName + "\n";
        }
        traitPanel.text = traitListString;
        bunnyFertility.text = "Fertility: " + fertility;
    }

    public void HideStatsPanel()
    {
        statsPanel.SetActive(false);
    }

    private void GenerateBunny()
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
        bunnies.Add(newBunny.gameObject);
        GameManager.Instance.bunnyList.Add(newBunny.gameObject);
        List<TraitType> newTraitList = new List<TraitType>();
        int traitCount = Random.Range(1, 3);
        newTraitList.Add(traitList.traitList.FindAll(x => x.personalitySet == 0)[Random.Range(0, traitList.traitList.FindAll(x => x.personalitySet == 0).Count)]);
        for (int i = 0; i < traitCount; i++)
        {
            List<TraitType> personalityTraits = traitList.traitList.FindAll(x => x.personalitySet > 0);
            TraitType newTrait = personalityTraits[Random.Range(0, personalityTraits.Count)];
            while (newTraitList.Contains(newTrait))
            {
                newTrait = personalityTraits[Random.Range(0, personalityTraits.Count)];
            }
            newTraitList.Add(newTrait);
        }
        string newBunnyName = bunnyNames[Random.Range(0, bunnyNames.Count)];
        bunnyNames.Remove(newBunnyName);
        newBunny.bunnyAge = 1f;
        int fertility = Random.Range(1, 5);
        newBunny.InitializeBunny(newBunnyName, selectedBunnyType, bunnyColor, fertility, bunnyTail, bunnyBody, bunnyHead, bunnyEar, newTraitList);
        newBunny.transform.position = new Vector3(Random.Range(minXBounds, maxXBounds), Random.Range(minYBounds, maxYBounds));
    }

    public void PlaceBunny(List<GameObject> bunnyList)
    {
        foreach (GameObject bunny in bunnyList)
        {
            Instantiate(bunny);
        }
    }

    public void GenerateStartingBunnies()
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

    public List<Bunny> BreedBunnies(Bunny daddy, Bunny mommy)
    {
        List<Bunny> newBunnies = new List<Bunny>();
        int averageFertility = (daddy.bunnyFertility + mommy.bunnyFertility) / 2;
        for (int i = 0; i < averageFertility; i++)
        {
            newBunnies.Add(PopOutBunny(daddy, mommy));
        }
        return newBunnies;
    }

    private Bunny PopOutBunny(Bunny daddy, Bunny mommy)
    {
        List<TraitType> daddyTraits = daddy.bunnyTraits;
        List<TraitType> mommyTraits = mommy.bunnyTraits;
        List<TraitType> babyTraits = new List<TraitType>();
        int randSeed = Random.Range(0, 2);
        babyTraits = randSeed == 0 ? GenerateTraits(daddyTraits, mommyTraits) : GenerateTraits(mommyTraits, daddyTraits);
        randSeed = Random.Range(0, 2);
        Color babyColor = randSeed == 0 ? daddy.bunnyColor : mommy.bunnyColor;
        randSeed = Random.Range(0, 2);
        BunnyType newBunnyType = randSeed == 0 ? daddy.bunnyType : mommy.bunnyType;
        string newBunnyName = bunnyNames[Random.Range(0, bunnyNames.Count)];
        bunnyNames.Remove(newBunnyName);
        Sprite bunnyHead = null;
        Sprite bunnyBody = null;
        Sprite bunnyEar = null;
        Sprite bunnyTail = null;
        switch (newBunnyType)
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
        Bunny newBunny = Instantiate(bunnyPrefab, transform.position, Quaternion.identity).GetComponent<Bunny>();
        bunnies.Add(newBunny.gameObject);
        newBunny.bunnyAge = 0f;
        int minFertility = Math.Min(daddy.bunnyFertility, mommy.bunnyFertility);
        int maxFertility = Math.Max(daddy.bunnyFertility, mommy.bunnyFertility);
        int fertility = Random.Range(minFertility, maxFertility + 1 );
        newBunny.InitializeBunny(newBunnyName,newBunnyType, babyColor, fertility, bunnyTail, bunnyBody, bunnyHead, bunnyEar, babyTraits);
        newBunny.transform.position = (daddy.transform.position + mommy.transform.position) / 2;
        return newBunny;
    }

    private List<TraitType> GenerateTraits(List<TraitType> dominantTrait, List<TraitType> submissiveTrait)
    {
        List<TraitType> babyTraits = new List<TraitType>();
        foreach (TraitType trait in dominantTrait)
        {
            if (babyTraits.Contains(trait)) continue;
            if (babyTraits.Count >= 5) break;
            if (Random.Range(0, 11) <= 5)
            {
                babyTraits.Add(trait);
            }
        }
        
        foreach (TraitType trait in submissiveTrait)
        {
            if (babyTraits.Contains(trait)) continue;
            if (trait.personalitySet != 0)
            {
                if (babyTraits.FindAll(x => x.personalitySet == trait.personalitySet).Count > 0) continue;
            }
            if (babyTraits.Count >= 5) break;
            if (Random.Range(0, 11) <= 5)
            {
                babyTraits.Add(trait);
            }
        }
        return babyTraits;
    }

    private static List<string> bunnyNames = new List<string>()
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