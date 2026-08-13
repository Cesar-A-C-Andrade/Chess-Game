using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class MoveValidator
{
    private AttackDetector detector = new AttackDetector();

    public Coordinates[] ValidateMoves(Piece piece, Coordinates[] moves, GameState currentState)
    {
        Board board = currentState.board;
        bool isWhiteTurn = currentState.isWhiteTurn;
        Coordinates piecePosition = piece.GetPosition();
        List<Coordinates> validMoves = new List<Coordinates>();

        foreach (Coordinates move in moves)
        {
            Coordinates validatedMove = ValidateMove(piece, move, currentState);
            if (validatedMove.IsEmpty()) { continue; }
            validMoves.Add(validatedMove);
        }

        return validMoves.ToArray();
        
    }

    public Coordinates ValidateMove(Piece piece, Coordinates move, GameState currentState)
    {
        Board board = currentState.board;
        bool isWhiteTurn = currentState.isWhiteTurn;
        Coordinates piecePosition = piece.GetPosition();
        Coordinates validMoves = new Coordinates(-1, -1);
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

