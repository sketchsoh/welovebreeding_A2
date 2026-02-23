using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class Bunny : MonoBehaviour
{
    private const float minXBounds = -0.4f;
    private const float maxXBounds = 6.5f;
    private const float minYBounds = -1.15f;
    private const float maxYBounds = 3f;
    
    private BunnyManager bunnyManager;
    public List<TraitType> bunnyTraits;
    private Vector2 currentMoveDirection;
    private Rigidbody2D rb2d;
    
    [Header("Bunny Parts")]
    [SerializeField] public SpriteRenderer bunnyTail;
    [SerializeField] public SpriteRenderer bunnyBody;
    [SerializeField] public SpriteRenderer bunnyHead;
    [SerializeField] public SpriteRenderer bunnyEars;

    [Header("Bunny Stats")]
    [SerializeField] public float cutenessStat;
    [SerializeField] public float playfulnessStat;
    [SerializeField] public float friendlinessStat;
    
    [Header("Other Stats")]
    [SerializeField] public string bunnyName;
    [SerializeField] public int bunnyFertility;
    [SerializeField] public float bunnyAge = 1;
    [SerializeField] public BunnyType bunnyType;
    [SerializeField] private float bunnySpeed;
    [SerializeField] public Color bunnyColor;
    
    private bool held = false;
    private float waitingTime;
    private float movingTime;
    private bool moving;
    public bool canBreed;

    private const int defaultTailLayer = 7;
    private const int defaultBodyLayer = 8;
    private const int defaultHeadLayer = 10;
    private const int defaultEarsLayer = 9;

    private const int heldTailLayer = 17;
    private const int heldBodyLayer = 18;
    private const int heldHeadLayer = 20;
    private const int heldEarsLayer = 19;


    public void InitializeBunny(
        string newName, 
        BunnyType type, 
        Color color,
        int fertility,
        Sprite Tail, 
        Sprite Body, 
        Sprite Head, 
        Sprite Ears ,
        List<TraitType> traits
        )
    {
        bunnyColor = color;
        bunnyTail.sprite = Tail;
        bunnyTail.color = bunnyColor;
        bunnyBody.sprite = Body;
        bunnyBody.color = bunnyColor;
        bunnyHead.sprite = Head;
        bunnyHead.color = bunnyColor;
        bunnyEars.sprite = Ears;
        bunnyEars.color = bunnyColor;
        bunnyFertility = fertility;
        bunnyTraits = traits;
        cutenessStat = 50f;
        playfulnessStat = 50f;
        friendlinessStat = 50f;
        SettleTraits();
        bunnyName = newName;
        gameObject.name = newName;
        bunnyType = type;
        held = false;
        rb2d = GetComponent<Rigidbody2D>();
        waitingTime = Random.Range(1f, 4f);
        movingTime = 0f;
        moving = false;
        canBreed = true;
        bunnyManager = FindAnyObjectByType<BunnyManager>();
        transform.localScale = new Vector3(0.75f, 0.75f, 1f);
        if (bunnyAge == 0)
        {
            canBreed = false;
            transform.localScale *= 0.75f;
        }
    }

    private void SettleTraits()
    {
        foreach (TraitType trait in bunnyTraits)
        {
            cutenessStat += trait.cutenessAdjustment;
            playfulnessStat += trait.playfulnessAdjustment;
            friendlinessStat += trait.friendlinessAdjustment;
        }
    }

    private void Update()
    {
        Collider2D bunnyCollider = GetComponent<Collider2D>();
        if (held)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            bunnyCollider.enabled = false;
            transform.position = mousePos;
            bunnyTail.sortingOrder = heldTailLayer;
            bunnyHead.sortingOrder = heldHeadLayer;
            bunnyBody.sortingOrder = heldBodyLayer;
            bunnyEars.sortingOrder = heldEarsLayer;
            return;
        }
        else
        {
            bunnyCollider.enabled = true;
            bunnyTail.sortingOrder = defaultTailLayer;
            bunnyHead.sortingOrder = defaultHeadLayer;
            bunnyBody.sortingOrder = defaultBodyLayer;
            bunnyEars.sortingOrder = defaultEarsLayer;
        }

        if (waitingTime > 0 && !moving)
        {
            waitingTime -= Time.deltaTime;
        }
        else if (!moving  && waitingTime <= 0)
        {
            ChooseDirection();
            moving = true;
            movingTime = Random.Range(2f, 3f);
        }

        if (moving && movingTime > 0)
        {
            movingTime -= Time.deltaTime;
            MoveAround();
        }
        else if (moving && movingTime <= 0)
        {
            waitingTime = Random.Range(1f, 4f);
            moving = false;
        }
    }

    private void MoveAround()
    {
        if (currentMoveDirection.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (currentMoveDirection.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        rb2d.linearVelocity = currentMoveDirection * (bunnySpeed * Time.deltaTime);
    }

    private void ChooseDirection()
    {
        currentMoveDirection = new  Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }

    private void OnMouseDown()
    {
        held = true;
        bunnyManager.ShowStatsPanel(bunnyName, cutenessStat, playfulnessStat, friendlinessStat, bunnyTraits);
    }

    private void OnMouseUp()
    {
        held = false;
        if (bunnyAge == 0) return;
        if (!canBreed) return;
        foreach (GameObject bunny in bunnyManager.bunnies)
        {
            if (bunny == gameObject) continue;
            if (bunny.GetComponent<Bunny>().bunnyAge == 0) continue;
            if (!bunny.GetComponent<Bunny>().canBreed) continue;
            if (Vector3.Distance(transform.position, bunny.transform.position) < 0.5f) // tolerance
            {
                Debug.Log(name + " is breeding with " + bunny.name);
                canBreed = false;
                bunnyManager.BreedBunnies(this, bunny.GetComponent<Bunny>());
                bunny.GetComponent<Bunny>().canBreed = false;
                break;
            }
        }
    }
}
