using UnityEngine;

public class Bishop : Piece
{
    private bool isWhite;

    public bool CanAttack(Coordinates target)
    {
        throw new System.NotImplementedException();
    }


    public Coordinates[] GenerateMoves(Board board)
    {
        throw new System.NotImplementedException();
    }

    public bool IsWhite()
    {
        return isWhite;
    }

    public Coordinates GetPosition()
    {
        throw new System.NotImplementedException();
    }

    public void SetColor(bool isWhite)
    {
        this.isWhite = isWhite;
    }

    public void SetPosition(Coordinates position)
    {
        throw new System.NotImplementedException();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
