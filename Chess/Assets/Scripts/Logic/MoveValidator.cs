using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class MoveValidator
{
    private AttackDetector detector = new AttackDetector();

    public void ValidateMoves(Piece piece, BoardPosition[] moves, GameState currentState, ref List<BoardPosition> validMovesList)
    {
        Board board = currentState.board;
        bool isWhiteTurn = currentState.isWhiteTurn;
        BoardPosition piecePosition = piece.GetPosition();

        foreach (BoardPosition move in moves)
        {
            BoardPosition validatedMove = ValidateMove(piece, move, currentState);
            if (validatedMove.IsEmpty()) { continue; }
            validMovesList.Add(validatedMove);
        }
        return;
    }

    public BoardPosition ValidateMove(Piece piece, BoardPosition move, GameState currentState)
    {
        Board board = currentState.board;
        bool isWhiteTurn = currentState.isWhiteTurn;
        BoardPosition piecePosition = piece.GetPosition();
        BoardPosition validMoves = new BoardPosition(-1, -1);
        if (move.IsEmpty()) return validMoves;
        board.MovePiece(piecePosition, move);
        if (!(detector.IsHouseUnderAttack(board, board.GetKingPosition(isWhiteTurn), isWhiteTurn)))
        {
            validMoves = move;
        }
        board.MovePiece(move, piecePosition);
        return validMoves;
    }
}

