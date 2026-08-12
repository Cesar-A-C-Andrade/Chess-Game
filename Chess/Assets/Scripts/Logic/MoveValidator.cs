using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class MoveValidator
{
    private Dictionary<string, Vector2> directions = new Dictionary<string, Vector2>
    {
        { "up", new Vector2(0, 1) },
        { "down", new Vector2(0, -1) },
        { "left", new Vector2(-1, 0) },
        { "right", new Vector2(1, 0) },
        { "diagUpLeft", new Vector2(-1, 1) },
        { "diagUpRight", new Vector2(1, 1) },
        { "diagDownLeft", new Vector2(-1, -1) },
        { "diagDownRight", new Vector2(1, -1) }
    };
    private AttackDetector detector = new AttackDetector();

    public Coordinates[] ValidateMoves(Piece piece, Coordinates[] moves, GameState currentState)
    {
        Board board = currentState.board;
        bool isWhiteTurn = currentState.isWhiteTurn;
        Coordinates piecePosition = piece.GetPosition();
        List<Coordinates> validMoves = new List<Coordinates>();

        foreach (Coordinates move in moves)
        {
            if (board.IsValidCoordinate(move))
            {
                board.MovePiece(piecePosition, move);
                if (!(detector.IsHouseUnderAttack(board, board.GetKingPosition(isWhiteTurn), isWhiteTurn)))
                {
                    validMoves.Add(move);
                }
                board.MovePiece(move, piecePosition);
            }
        }

        return validMoves.ToArray();
        
    }

    private Coordinates GetEnPassantCoordinate(Move lastMovement, Pawn pawn)
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

        if(Mathf.Abs(lastCoordinate.x - newCoordinate.x) != 2)
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
}

