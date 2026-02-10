using UnityEngine;
using UnityEngine.UI;

public class CustomerManager : MonoBehaviour
{
    [SerializeField] private Image customerSprite;
    public Sprite hachiSprite;
    public Sprite payuSprite;
    public Sprite clzySprite;
    public Sprite cultSprite;

    private void AssignCustomerSprite(string customerName)
    {
        customerSprite.sprite = Resources.Load<Sprite>(customerName);
        customerSprite.SetNativeSize();
    }
}
