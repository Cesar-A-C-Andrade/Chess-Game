using UnityEngine;

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
        Piece[] pieces = isWhiteTurn ? board.GetPiecesByColor("White") : board.GetPiecesByColor("Black");
        foreach (Piece piece in pieces)
        {
            Coordinates piecePosition = piece.GetPosition();
            foreach (Coordinates move in piece.GenerateMoves(board))
            {
                board.MovePiece(piecePosition, move);
                board.PrintBoard();
                currentState.board = board;
                if (!(XequeChecker(currentState)))
                {
                    return false;
                }
                board.MovePiece(move, piecePosition);
            }
            //Test En Passant
            if (piece is Pawn)
            {
                Coordinates enPassant = specialMoveManager.GetEnPassantCoordinate(currentState.lastMovement, piece as Pawn);
            }
        }
        

        return true;
    }
}
