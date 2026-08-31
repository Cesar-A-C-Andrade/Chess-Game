using UnityEngine;

public class PromotionSelection : MonoBehaviour
{

    Event<OnPawnPromoted> promotionFinished = new Event<OnPawnPromoted>();

    void Start()
    {
        gameObject.SetActive(false);
        EventBus.instance.AddBroadCaster(promotionFinished);
        EventBus.instance.Subscribe<OnShowPromotionSelection>(HandleShowPromotionSelection);
    }


    public void HandleShowPromotionSelection(OnShowPromotionSelection data)
    {
        transform.position = data.position;
        gameObject.SetActive(true);
    }

    public void OnPromotionButtonPressed(string pieceType)
    {
        gameObject.SetActive(false);
        EventBus.instance.Invoke<OnPawnPromoted>(new OnPawnPromoted(pieceType));
    }
    
}