using UnityEngine;

public class SpecialMoveManager
{
    public Coordinates GetEnPassantCoordinate(Move lastMovement, Pawn pawn)
    {
        Coordinates lastCoordinate = lastMovement.lastCoordinate;
        Coordinates newCoordinate = lastMovement.newCoordinate;
        Coordinates enPassantCoordinate = new Coordinates(-1, -1);
        if (!(lastMovement.piece is Pawn))
        {
            return enPassantCoordinate;
        }
        if (lastCoordinate.x < 0 || newCoordinate.x < 0)
        {
            return enPassantCoordinate;
        }
        int lastPawnDirection = lastMovement.piece.IsWhite() ? 1 : -1;

        if (Mathf.Abs(lastCoordinate.x - newCoordinate.x) != 2)
        {
            return enPassantCoordinate;
        }

        enPassantCoordinate.x = lastCoordinate.x + (1 * lastPawnDirection);
        enPassantCoordinate.y = lastCoordinate.y;

        if (!pawn.CanAttack(enPassantCoordinate))
        {
            return new Coordinates(-1, -1);
        }
        return enPassantCoordinate;
    }

    public void MakeEnPassant(Board board)
    {

    }
}
