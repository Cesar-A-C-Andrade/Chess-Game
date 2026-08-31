using UnityEngine;

public class OnHouseSelectedEvent : IEvent
{
    public BoardPosition houseSelectedCoordinates { get; private set; }

    public OnHouseSelectedEvent(BoardPosition coordinates)
    {
        houseSelectedCoordinates = coordinates;
    }
}
