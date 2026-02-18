using System;
using UnityEngine;
using Object = UnityEngine.Object;

public class Bunny : MonoBehaviour
{
    private const float minXBounds = -0.4f;
    private const float maxXBounds = 6.5f;
    private const float minYBounds = -1.15f;
    private const float maxYBounds = 3f;
    
    private BunnyManager bunnyManager;
    
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
    [SerializeField] public float bunnyAge;
    [SerializeField] public BunnyType bunnyType;
    [SerializeField] public Color bunnyColor;
    
    private bool held = false;
    
    public void InitializeBunny(
        string bunnyName, 
        BunnyType type, 
        Color color,
        Sprite Tail, 
        Sprite Body, 
        Sprite Head, 
        Sprite Ears, 
        float cuteness, 
        float playfulness, 
        float friendliness
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
        cutenessStat = cuteness;
        playfulnessStat = playfulness;
        friendlinessStat = friendliness;
        this.bunnyName = bunnyName;
        gameObject.name = bunnyName;
        bunnyAge = 0;
        bunnyType = type;
        held = false;
        bunnyManager = FindAnyObjectByType<BunnyManager>();
    }

    private void Update()
    {
        if (held)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mousePos;
        }
    }

    private void OnMouseDown()
    {
        held = true;
        bunnyManager.ShowStatsPanel(bunnyName, cutenessStat, playfulnessStat, friendlinessStat);
    }

    private void OnMouseUp()
    {
        held = false;
    }
}
