using UnityEngine;

public class OnPieceSelectedEvent : IEvent
{
    public BoardPosition[] possibleMovesCoordinates {  get; private set; }

    public OnPieceSelectedEvent(BoardPosition[] coordinates)
    {
        possibleMovesCoordinates = coordinates;
    }
}
