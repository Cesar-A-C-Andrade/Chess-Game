using UnityEngine;
using UnityEngine.UI;

public class XequeManager
{

    private AttackDetector detector = new AttackDetector();
    private SpecialMoveManager specialMoveManager = new SpecialMoveManager();
    public bool XequeChecker(GameState currentState)
    {
        Board board = currentState.board;
        bool isWhiteTurn = currentState.isWhiteTurn;

        if (detector.IsHouseUnderAttack(board, board.GetKingPosition(isWhiteTurn), isWhiteTurn))
        {
            return true;
        }

        return false;
    }


    public bool IsXequeMate(GameState currentState)
    {
        Board board = currentState.board;
        bool isWhiteTurn = currentState.isWhiteTurn;
        Move possibleMove;
        Piece[] pieces = isWhiteTurn ? board.GetPiecesByColor("White") : board.GetPiecesByColor("Black");
        foreach (Piece piece in pieces)
        {
            Coordinates piecePosition = piece.GetPosition();
            foreach (Coordinates move in piece.GenerateMoves(board))
            {
                possibleMove = board.MovePiece(piecePosition, move);
                currentState.board = board;
                if (!(XequeChecker(currentState)))
                {
                    return false;
                }
                board.UndoMove(possibleMove);
            }
            //Test En Passant
            if (piece is Pawn)
            {
                Coordinates enPassant = specialMoveManager.GetEnPassantCoordinate(currentState.lastMovement, piece as Pawn);
                if (enPassant.x != -1)
                {
                    possibleMove = specialMoveManager.MakeEnPassant(piece as Pawn, currentState.lastMovement, board);
                    currentState.board = board;
                    if (!(XequeChecker(currentState)))
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
