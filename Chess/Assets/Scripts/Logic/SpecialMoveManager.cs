using UnityEngine;

public class SpecialMoveManager
{
    public Coordinates GetEnPassantCoordinate(Move lastMovement, Pawn pawn)
    {
        Coordinates lastCoordinate = lastMovement.lastCoordinate;
        Coordinates newCoordinate = lastMovement.newCoordinate;
        Coordinates enPassantCoordinate = new Coordinates(-1, -1);
        if (!(lastMovement.pieceMoved is Pawn))
        {
            return enPassantCoordinate;
        }
        if (lastCoordinate.x < 0 || newCoordinate.x < 0)
        {
            return enPassantCoordinate;
        }
        int lastPawnDirection = lastMovement.pieceMoved.IsWhite() ? 1 : -1;

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

    public Move MakeEnPassant(Pawn pawn, Move lastMovement, Board board)
    {
        Coordinates enPassant = GetEnPassantCoordinate(lastMovement, pawn);
        Coordinates lastPawnPosition = lastMovement.pieceMoved.GetPosition();
        Move enPassantMove = new Move(pawn, lastMovement.pieceMoved, pawn.GetPosition(), enPassant);
        board.MovePiece(pawn.GetPosition(), enPassant);
        board.RemovePieceAt(lastPawnPosition);
        return enPassantMove;
    }

    public bool IsSpecialMove(Piece piece, Move lastMovement, Coordinates to)
    {
        if (piece == null) return false;
        if (!(piece is Pawn)) return false;
        if (!(to.Equals(GetEnPassantCoordinate(lastMovement, piece as Pawn)))) return false;
        return true;
    }
}
