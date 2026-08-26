using UnityEngine;

public class OnHouseSelectedEvent : IEvent
{
    public Coordinates houseSelectedCoordinates { get; private set; }

    public OnHouseSelectedEvent(Coordinates coordinates)
    {
        houseSelectedCoordinates = coordinates;
    }
}
