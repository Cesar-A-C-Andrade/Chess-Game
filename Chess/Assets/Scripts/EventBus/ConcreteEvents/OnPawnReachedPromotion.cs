using UnityEngine;

public class OnPawnReachedPromotion : IEvent
{

    public BoardPosition pawnPosition;

    public OnPawnReachedPromotion(BoardPosition pawnPosition)
    {
        this.pawnPosition = pawnPosition;
    }
}
