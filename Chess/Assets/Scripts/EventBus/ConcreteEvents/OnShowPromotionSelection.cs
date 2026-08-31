using UnityEngine;

public class OnShowPromotionSelection : IEvent
{
    public Vector2 position { get; private set; }

    public OnShowPromotionSelection(Vector2 _pos)
    {
        position = _pos;
    }
}
