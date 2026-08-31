using UnityEngine;
using UnityEngine.UI;

public class XequeManager
{

    private AttackDetector detector = new AttackDetector();
    private SpecialMoveManager specialMoveManager = new SpecialMoveManager();
    public bool XequeChecker(Board board, bool isWhiteTurn)
    {

        if (detector.IsHouseUnderAttack(board, board.GetKingPosition(isWhiteTurn), isWhiteTurn))
        {
            return true;
        }

        return false;
    }


    public bool IsXequeMate(Board board, bool isWhiteTurn, Move lastMovement)
    {
        Move possibleMove;
        Piece[] pieces = isWhiteTurn ? board.GetPiecesByColor("White") : board.GetPiecesByColor("Black");
        foreach (Piece piece in pieces)
        {
            BoardPosition piecePosition = piece.GetPosition();
            foreach (BoardPosition move in piece.GenerateMoves(board))
            {
                possibleMove = board.MovePiece(piecePosition, move);
                if (!(XequeChecker(board, isWhiteTurn)))
                {
                    return false;
                }
                board.UndoMove(possibleMove);
            }
            //Test En Passant
            if (piece is Pawn)
            {
                BoardPosition enPassant = specialMoveManager.GetEnPassantCoordinate(lastMovement, piece as Pawn);
                if (enPassant.x != -1)
                {
                    possibleMove = specialMoveManager.MakeEnPassant(piece as Pawn, lastMovement, board);
                    if (!(XequeChecker(board, isWhiteTurn)))
                    {
                        return false;
                    }
                    board.UndoMove(possibleMove);
                }
            }
        }
        

        return true;
    }
}
