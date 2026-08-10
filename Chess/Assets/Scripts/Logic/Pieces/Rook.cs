using UnityEngine;

public class Rook : Piece
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

    public Coordinates GetPosition()
    {
        throw new System.NotImplementedException();
    }

    public bool IsWhite()
    {
        return isWhite;
    }

    public void SetColor(bool isWhite)
    {
        this.isWhite = isWhite;
    }

    public void SetPosition(Coordinates position)
    {
        throw new System.NotImplementedException();
    }
}
