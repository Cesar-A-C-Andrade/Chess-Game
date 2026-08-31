using UnityEngine;

public class OnPawnPromoted : IEvent
{

    public string pieceType {  get; private set; }
    public OnPawnPromoted(string pieceType)
    {
        this.pieceType = pieceType;
    }
}
