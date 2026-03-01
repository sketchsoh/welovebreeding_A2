using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int day;
    public CultRequest currentCultRequest;
    public CurrentCustomer currentCustomer;
    public List<GameObject> bunnyList =  new List<GameObject>();
    public CustomerManager cManager;
    public BunnyManager bManager;
    public int bunniesSold;
    public float prevPublicPerception;
    public float prevCultPerception;
    public float currPublicPerception;
    public float currCultPerception;
    public List<CultRequest> cultRequestList = new List<CultRequest>
    {
        new CultRequest("We require the Worrywart. Fear is a faithful thing.", "Worrywart", 2),
        new CultRequest("Bring the Serene. Calm is devotion.", "Serene", 2),
        new CultRequest("Bring the Jolly. Joy feeds the circle.",  "Jolly", 3),
        new CultRequest("We want the Emo. The heavy-hearted are honest.", "Emo", 3),
        new CultRequest("We demand the Destructive. Let chaos prove loyalty.", "Destructive", 1),
        new CultRequest("We require the Pristine. Cleanliness is a ritual.", "Pristine", 2),
        new CultRequest("Bring the Clumsy. The stumbling are blessed.", "Clumsy", 2),
        new CultRequest("We want the Recluse. The hidden ones hear us best.", "Recluse", 2),
        new CultRequest("Bring the Social Butterfly. Gatherers strengthen the circle.", "Social Butterfly", 3),
        new CultRequest("We demand the Glutton. Hunger is sacred.", "Glutton", 1),
        new CultRequest("Bring those On a Diet. Restraint demonstrates devotion.", "On A Diet", 2),
        new CultRequest("Bring the Lazy. Stillness is a kind of worship.", "Lazy", 2),
        new CultRequest("We require the Hyper. Fast blood. Quick faith.", "Hyper", 1),
        new CultRequest("Bring the Needy. Attachment binds the offering.",  "Needy", 2),
        new CultRequest("We want the Independent. Strong wills please us.", "Independent", 2)
    };

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }

        if (Instance != this)
        {
            Destroy(gameObject);
        }
        InitGameManager();
    }

    private void InitGameManager()
    {
        day = 1;
        currCultPerception = 20f;
        currPublicPerception = 20f;
        prevPublicPerception = currPublicPerception;
        prevCultPerception = currCultPerception;
    }

    public void StartDay()
    {
        prevCultPerception = currCultPerception;
        prevPublicPerception = currPublicPerception;
        if (day % 3 == 1)
        {
            currentCultRequest = cultRequestList[Random.Range(0, cultRequestList.Count)];
            cManager.CultArrive(CurrentCustomer.CultStart);
        }
        else
        {
            cManager.AssignNewCustomer();
        }
        

        if (day == 1)
        {
            bManager.GenerateStartingBunnies();
        }
        else
        {
            bManager.PlaceBunny(bunnyList);
            foreach (GameObject bunny in bunnyList)
            {
                bunny.GetComponent<Bunny>().AgeBunnies();
            }
        }
    }
    
    public void EndDay()
    {
        day++;
        SceneManager.LoadScene("EOD");
    }

    public void EndGame()
    {
        currentCultRequest = null;
        bunnyList.Clear();
        day = 1;
        bunniesSold = 0;
    }
}

public enum CurrentCustomer
{
    CultStart,
    CultCollect,
    Customer
}