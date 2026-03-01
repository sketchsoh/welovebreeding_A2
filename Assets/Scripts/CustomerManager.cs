using System;
using System.Collections.Generic;
using LitMotion;
using LitMotion.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CustomerManager : MonoBehaviour
{
    private const float minXBounds = -0.4f;
    private const float maxXBounds = 6.5f;
    private const float minYBounds = -1.15f;
    private const float maxYBounds = 3f;
    [SerializeField] private Image customerSprite;
    [SerializeField] private TextMeshProUGUI customerName;
    [SerializeField] private TextMeshProUGUI customerRequestText;
    public Sprite hachiSprite;
    public Sprite payuSprite;
    public Sprite clzySprite;
    public Sprite cultSprite;
    public string prevCustomerName = "";
    public GameObject nextButton;
    public List<Bunny> donatedBunnies = new List<Bunny>();
    public TextMeshProUGUI dayText;
    
    private string[] customerNames = { "Clzy", "Payu", "Hachi" };
    private static int currentCustomerIndex = 0;

    private string[] cultCollectionLines =
    {
        "Do not greet us. Present the offerings.",
        "Pick carefully. This is not reversible",
    };

    private string[] cultReceiveGoodLines =
    {
        "Suitable.",
        "Up to par.",
        "Standard.",
    };

    private string[] cultReceiveBadLines =
    {
        "It will not feed the quota.",
        "These are not the right ones.",
        "Not Suitable.",
        "Below Standard"
    };
    private string[] cultHappyLines =
    {
        "The quotas are met. The shop lives to see another cycle.",
        "The public is pleased. The circle is quiet. For now."
    };

    private string[] cultAngryLines =
    {
        "You have failed to meet the quota.",
        "Do better next cycle.",
        "This will be noted."
    };
    
    // private Dictionary<int, CustomerRequest> customerList = new Dictionary<int, CustomerRequest>();
    private Queue<CustomerRequest> requestQueue = new Queue<CustomerRequest>();
    private List<CustomerRequest> customerRequestList = new List<CustomerRequest>
        {
            new CustomerRequest("I want one that’s always bouncing around. Like way too much.", 83f, 52f, 48f),
            new CustomerRequest("I’m not here for the X-Factor, but the ‘Aww’ factor! Where can I find one?", 45f, 78f, 55f),
            new CustomerRequest("Looking for a pet that completes the ‘E’ to my ‘I’ introversion.", 50f, 58f, 70f),
            new CustomerRequest("I’m just a chill guy, looking for something calm, gentle and comforting.", 42f, 60f, 75f),
            new CustomerRequest("I want one that looks just like a plush toy, like my favourite Jelly Cats.", 48f, 85f, 62f),
            new CustomerRequest("Something independent. I need my personal space.", 52f, 0f, 40f),
            new CustomerRequest("I want a pet that will always stick close to me! An emotional support animal of sort.", 60f, 65f, 82f)
        };

    private List<string> perfectReaction = new List<string>
    {
        "THIS is the one. I can feel it.",
        "You understood the assignment!",
        "I would die for this rabbit.",
        "It’s like you read my mind."
    };

    private List<string> goodReaction = new List<string>
    {
        "Okay well…this is actually not too bad.",
        "Not exactly what I imagined, but I like it.",
        "You know what? I can work with this." ,
        "I’m…pleasantly surprised.",
    };

    private List<string> mehReaction = new List<string>
    {
        "Hmm. It’s cute but it’s not really what I asked for.",
        "I think we’re…a few pages apart but almost there.",
        "This rabbit had a different destiny.",
        "I think I’ll pass…for now."
 
    };

    private List<string> badReaction = new List<string>
    {
        "This is exactly the opposite of what I wanted.",
        "You must be trolling me.",
        "I can’t take this home. It would ruin my life.",
        "No. Absolutely not. Next!"
    };

    public void CultArrive(CurrentCustomer currentCustomer)
    {
        AssignCustomerSprite("Cult");
        customerName.text = "Cult";
        GameManager.Instance.currentCustomer = currentCustomer;
        LMotion.Create(-1250f, -550f, 0.5f)
            .WithEase(Ease.InOutElastic)
            .BindToAnchoredPosition3DX(customerSprite.GetComponent<RectTransform>());
        if (currentCustomer == CurrentCustomer.CultStart)
        {
            customerRequestText.text = GameManager.Instance.currentCultRequest.flavorText;
        }
        else
        {
            customerRequestText.text = cultCollectionLines[Random.Range(0, cultCollectionLines.Length)];
            nextButton.SetActive(true);
        }
    }

    public void GiveToCult(Bunny bunny)
    {
        donatedBunnies.Add(bunny);
        bunny.bunnyManager.bunnies.Remove(bunny.gameObject);
        GameManager.Instance.bunnyList.Remove(bunny);
        bunny.gameObject.SetActive(false);
        string currentRequestTrait = GameManager.Instance.currentCultRequest.traitRequirement;
        if (bunny.bunnyTraits.Find(x => x.traitName == currentRequestTrait) != null)
        {
            customerRequestText.text = cultReceiveGoodLines[Random.Range(0, cultReceiveGoodLines.Length)];
        }
        else
        {
            customerRequestText.text = cultReceiveBadLines[Random.Range(0, cultReceiveBadLines.Length)];
        }
        if (GameManager.Instance.currentCultRequest.AmountRequirement <= donatedBunnies.FindAll(x => x.bunnyTraits.FindAll(x => x.traitName == GameManager.Instance.currentCultRequest.traitRequirement).Count > 0).Count)
        {
            SettleCult();
        }
    }

    public void SettleCult()
    {
        int bunnyCount = 0;
        foreach (Bunny bunny in donatedBunnies)
        {
            if (bunny.bunnyTraits.Find(x => x.traitName == GameManager.Instance.currentCultRequest.traitRequirement) != null)
            {
                bunnyCount++;
            }
        }

        if (bunnyCount >= GameManager.Instance.currentCultRequest.AmountRequirement)
        {
            customerRequestText.text = cultHappyLines[Random.Range(0, cultHappyLines.Length)];
            nextButton.SetActive(false);
            GameManager.Instance.currCultPerception += 20f;
        }
        else
        {
            customerRequestText.text = cultAngryLines[Random.Range(0, cultAngryLines.Length)];
            
            nextButton.SetActive(false);
        }
        
        Invoke(nameof(CustomerLeave), 1.5f);
    }

    private void EndCult()
    {
        GameManager.Instance.day = 4;
        SceneManager.LoadScene("EOD", LoadSceneMode.Additive);
    }

    private void AssignCustomerSprite(string customerName)
    {
        switch (customerName)
        {
            case "Clzy":
                customerSprite.sprite = clzySprite;
                break;
            case "Cult":
                customerSprite.sprite = cultSprite;
                break;
            case "Payu":
                customerSprite.sprite = payuSprite;
                break;
            case "Hachi":
                customerSprite.sprite = hachiSprite;
                break;
        }
        customerSprite.SetNativeSize();
    }

    public void AssignNewCustomer()
    {
        GameManager.Instance.currentCustomer = CurrentCustomer.Customer;
        string cName = customerNames[Random.Range(0, customerNames.Length)];
        while (cName == prevCustomerName)
        {
            cName = customerNames[Random.Range(0, customerNames.Length)];
        }
        prevCustomerName = cName;
        AssignCustomerSprite(cName);
        customerName.text = cName.ToUpper();
            CustomerRequest newCustomerRequest = Request();
            // customerList.Add(++currentCustomerIndex, newCustomerRequest);
            requestQueue.Enqueue(newCustomerRequest);
            customerRequestText.text = newCustomerRequest.flavorText;

        LMotion.Create(-1250f, -550f, 0.5f)
            .WithEase(Ease.InOutElastic)
            .BindToAnchoredPosition3DX(customerSprite.GetComponent<RectTransform>());

    }

    private CustomerRequest Request()
    {
        CustomerRequest newCustomerRequest = customerRequestList[Random.Range(0, customerRequestList.Count)];
        return newCustomerRequest;
    }

    private void CustomerLeave()
    {
        LMotion.Create(-550f, -1250f, 0.5f)
            .WithEase(Ease.InOutElastic)
            .WithOnComplete((() => GameManager.Instance.EndDay()))
            .BindToAnchoredPosition3DX(customerSprite.GetComponent<RectTransform>());
        customerRequestText.text = "";
        customerName.text = "";
    }

    // Changed by payu. Values are originally 100 for the clamp
    public void SellBunny(Bunny bunny)
    {
        float happiness = 0;
        int statCount = 0;
        float lowestStat = 120f; //added by payu
        if (!requestQueue.TryPeek(out CustomerRequest currentCustomerRequest))
        {
            Debug.LogWarning("Sell attempted but no customer request exists.");
            return;
        }
        if (currentCustomerRequest.cutenessRequirement != 0)
        {
            float score = Mathf.Clamp(
                (bunny.cutenessStat / currentCustomerRequest.cutenessRequirement) * 100,
                0, 120);

            happiness += score;
            lowestStat = Mathf.Min(lowestStat, score);
            statCount++;
        }

        if (currentCustomerRequest.playfulnessRequirement != 0)
        {
            float score = Mathf.Clamp(
                (bunny.playfulnessStat / currentCustomerRequest.playfulnessRequirement) * 100,
                0, 120);

            happiness += score;
            lowestStat = Mathf.Min(lowestStat, score);
            statCount++;
        }

        if (currentCustomerRequest.friendlinessRequirement != 0)
        {
            float score = Mathf.Clamp(
                (bunny.friendlinessStat / currentCustomerRequest.friendlinessRequirement) * 100,
                0, 120);

            happiness += score;
            lowestStat = Mathf.Min(lowestStat, score);
            statCount++;
        }
        
        float average = happiness / statCount;
        happiness = (average * 0.7f) + (lowestStat * 0.3f); 
        // Changed by payu. Originally these two lines are just
        //happiness /= statCount;

        BunnyManager bunnyManager = bunny.bunnyManager;

        if (happiness >= 80)
        {
            customerRequestText.text = perfectReaction[Random.Range(0, perfectReaction.Count)];
            bunnyManager.bunnies.Remove(bunny.gameObject);
            GameManager.Instance.bunnyList.Remove(bunny);
            GameManager.Instance.bunniesSold++;
            GameManager.Instance.currPublicPerception = GameManager.Instance.prevPublicPerception + 20f;
            bunny.gameObject.SetActive(false);
        }
        else if (happiness >= 50)
        {
            customerRequestText.text = goodReaction[Random.Range(0, goodReaction.Count)];
            bunnyManager.bunnies.Remove(bunny.gameObject);
            GameManager.Instance.bunnyList.Remove(bunny);
            GameManager.Instance.bunniesSold++;
            GameManager.Instance.currPublicPerception = GameManager.Instance.prevPublicPerception + 10f;
            bunny.gameObject.SetActive(false);
        }
        else if (happiness >= 20)
        {
            customerRequestText.text = mehReaction[Random.Range(0, mehReaction.Count)];
            Vector2 pos = bunny.transform.position;
            pos.x = Mathf.Clamp(pos.x, minXBounds, maxXBounds);
            GameManager.Instance.currPublicPerception = GameManager.Instance.prevPublicPerception - 10f;
            bunny.transform.position = pos;
            
        }
        else if (happiness >= 10)
        {
            customerRequestText.text = badReaction[Random.Range(0, badReaction.Count)];
            Vector2 pos = bunny.transform.position;
            pos.x = Mathf.Clamp(pos.x, minXBounds, maxXBounds);
            GameManager.Instance.currPublicPerception = GameManager.Instance.prevPublicPerception - 15f;
            bunny.transform.position = pos;
        }

        requestQueue.Dequeue();

        Invoke(nameof(CustomerLeave), 1.5f);
    }

    public void NextButtonSettle()
    {
        if (GameManager.Instance.currentCustomer == CurrentCustomer.Customer)
        {
            return;
        }

        if (GameManager.Instance.currentCustomer == CurrentCustomer.CultStart)
        {
            AssignNewCustomer();
            nextButton.SetActive(false);
        }

        if (GameManager.Instance.currentCustomer == CurrentCustomer.CultCollect)
        {
            SettleCult();
        }
    }
}

public class CustomerRequest
{
    public string flavorText;
    public float playfulnessRequirement = 0;
    public float cutenessRequirement = 0;
    public float friendlinessRequirement = 0;

    public CustomerRequest(string flavor, float playfulness, float cuteness, float friendliness)
    {
        flavorText = flavor;
        playfulnessRequirement = playfulness;
        cutenessRequirement = cuteness;
        friendlinessRequirement = friendliness;
    }
}

public class CultRequest
{
    public string flavorText;
    public string traitRequirement;
    public int AmountRequirement;

    public CultRequest(string flavor, string trait, int amount)
    {
        flavorText = flavor;
        traitRequirement = trait;
        AmountRequirement = amount;
    }
}
