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
    
    public List<Bunny> donatedBunnies = new List<Bunny>();
    
    private string[] customerNames = { "Clzy", "Payu", "Hachi" };
    private static int currentCustomerIndex = 0;
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
        customerRequestText.text = GameManager.Instance.currentCultRequest.flavorText;
    }

    public void GiveToCult(Bunny bunny)
    {
        donatedBunnies.Add(bunny);
        bunny.bunnyManager.bunnies.Remove(bunny.gameObject);
        GameManager.Instance.bunnyList.Remove(bunny.gameObject);
        bunny.gameObject.SetActive(false);
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

    public void SellBunny(Bunny bunny)
    {
        float happiness = 0;
        int statCount = 0;
        requestQueue.TryPeek(out CustomerRequest currentCustomerRequest);
        if (currentCustomerRequest.cutenessRequirement != 0)
        {
            happiness = Mathf.Clamp(((bunny.cutenessStat / currentCustomerRequest.cutenessRequirement) * 100), 0, 100);
            statCount++;
        }

        if (currentCustomerRequest.playfulnessRequirement != 0)
        {
            happiness += Mathf.Clamp(((bunny.playfulnessStat / currentCustomerRequest.playfulnessRequirement) * 100), 0, 100);
            statCount++;
        }

        if (currentCustomerRequest.friendlinessRequirement != 0)
        {
            happiness += Mathf.Clamp(((bunny.friendlinessStat / currentCustomerRequest.friendlinessRequirement) * 100), 0, 100);
            statCount++;
        }
        
        happiness /= statCount;

        BunnyManager bunnyManager = bunny.bunnyManager;

        if (happiness >= 80)
        {
            customerRequestText.text = perfectReaction[Random.Range(0, perfectReaction.Count)];
            bunnyManager.bunnies.Remove(bunny.gameObject);
            GameManager.Instance.bunnyList.Remove(bunny.gameObject);
            bunny.gameObject.SetActive(false);
        }
        else if (happiness >= 50)
        {
            customerRequestText.text = goodReaction[Random.Range(0, goodReaction.Count)];
            bunnyManager.bunnies.Remove(bunny.gameObject);
            GameManager.Instance.bunnyList.Remove(bunny.gameObject);
            bunny.gameObject.SetActive(false);
        }
        else if (happiness >= 20)
        {
            customerRequestText.text = mehReaction[Random.Range(0, mehReaction.Count)];
            Vector2 pos = bunny.transform.position;
            pos.x = Mathf.Clamp(pos.x, minXBounds, maxXBounds);
            bunny.transform.position = pos;
            
        }
        else if (happiness >= 10)
        {
            customerRequestText.text = badReaction[Random.Range(0, badReaction.Count)];
            Vector2 pos = bunny.transform.position;
            pos.x = Mathf.Clamp(pos.x, minXBounds, maxXBounds);
            bunny.transform.position = pos;
        }

        requestQueue.Dequeue();

        Invoke(nameof(CustomerLeave), 2f);
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
