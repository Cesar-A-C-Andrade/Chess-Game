using UnityEngine;

public class OnPieceSelectedEvent : IEvent
{
    public Coordinates[] possibleMovesCoordinates {  get; private set; }

    public OnPieceSelectedEvent(Coordinates[] coordinates)
    {
        possibleMovesCoordinates = coordinates;
    }
}
