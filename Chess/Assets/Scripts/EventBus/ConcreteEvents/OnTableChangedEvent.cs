using UnityEngine;

public class OnTableChangedEvent : IEvent
{
    public string[,] data { get; private set; }
    public bool[,] colors { get; private set; }

    public OnTableChangedEvent(string[,] data, bool[,] colors)
    {
        this.data = data;
        this.colors = colors;
    }
}
