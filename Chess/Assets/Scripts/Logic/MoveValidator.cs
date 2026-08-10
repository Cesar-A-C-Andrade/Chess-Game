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

    public Coordinates[] ValidateMoves(Piece piece, Coordinates[] moves, GameState currentState)
    {
        Board board = currentState.board;
        bool isWhiteTurn = currentState.isWhiteTurn;
        Coordinates piecePosition = piece.GetPosition();
        List<Coordinates> validMoves = new List<Coordinates>();

        foreach (Coordinates move in moves)
        {
            board.MovePiece(piecePosition, move);
            if (!(IsHouseUnderAttack(currentState, board.GetKingPosition(isWhiteTurn))))
            {
                validMoves.Add(move);
            }
            board.MovePiece(move, piecePosition);
        }
        if(piece is Pawn)
        {
            Coordinates enPassantCoordinate = GetEnPassantCoordinate(currentState.lastMovement, piece as Pawn);
            if (enPassantCoordinate.x != -1)
            {
                validMoves.Add(enPassantCoordinate);
            }
        }


        return validMoves.ToArray();
        
    }



    public bool IsHouseUnderAttack(GameState currentState, Coordinates house)
    {
        bool isWhiteTurn = currentState.isWhiteTurn;
        if(CheckIfAnyPieceCanAttack(GetPiecesInHorsePositions(house, currentState.board), house, isWhiteTurn))
        {
            return true;
        }
        if (CheckIfAnyPieceCanAttack(GetPiecesInPawnPositions(house, isWhiteTurn, currentState.board), house, isWhiteTurn))
        {
            return true;
        }
        if (CheckIfAnyPieceCanAttack(GetPiecesInKingPositions(house, currentState.board), house, isWhiteTurn))
        {
            return true;
        }
        if (CheckIfAnyPieceCanAttack(GetSlidingPieces(house, currentState.board), house, isWhiteTurn))
        {
            return true;
        }
        return false;
    }

    private Piece[] GetPiecesInHorsePositions(Coordinates position, Board board)
    {
        Coordinates[] moves = new Coordinates[8];
        moves[0] = new Coordinates(position.x + 2, position.y + 1);
        moves[1] = new Coordinates(position.x + 2, position.y - 1);
        moves[2] = new Coordinates(position.x - 2, position.y + 1);
        moves[3] = new Coordinates(position.x - 2, position.y - 1);
        moves[4] = new Coordinates(position.x + 1, position.y + 2);
        moves[5] = new Coordinates(position.x + 1, position.y - 2);
        moves[6] = new Coordinates(position.x - 1, position.y + 2);
        moves[7] = new Coordinates(position.x - 1, position.y - 2);
        return board.GetPiecesAt(moves);
    }

    private Piece[] GetPiecesInPawnPositions(Coordinates position, bool isWhite, Board board)
    {
        Coordinates[] moves = new Coordinates[2];
        int direction = isWhite ? 1 : -1;
        moves[0] = new Coordinates(position.x + direction, position.y - 1);
        moves[1] = new Coordinates(position.x + direction, position.y + 1);
        return board.GetPiecesAt(moves);
    }

    private Piece[] GetPiecesInKingPositions(Coordinates position, Board board)
    {
        Coordinates[] moves = new Coordinates[8];
        moves[0] = new Coordinates(position.x + 1, position.y);
        moves[1] = new Coordinates(position.x - 1, position.y);
        moves[2] = new Coordinates(position.x, position.y + 1);
        moves[3] = new Coordinates(position.x, position.y - 1);
        moves[4] = new Coordinates(position.x + 1, position.y + 1);
        moves[5] = new Coordinates(position.x + 1, position.y - 1);
        moves[6] = new Coordinates(position.x - 1, position.y + 1);
        moves[7] = new Coordinates(position.x - 1, position.y - 1);
        return board.GetPiecesAt(moves);
    }

    private Piece[] GetSlidingPieces(Coordinates position, Board board)
    {
        List<Piece> pieces = new List<Piece>();
        Piece rightDirPiece = GetPieceInDirection(position, "right", board);
        Piece leftDirPiece = GetPieceInDirection(position, "left",board);
        Piece downDirPiece = GetPieceInDirection(position,"down",board);
        Piece upDirPiece = GetPieceInDirection(position, "up", board);
        Piece diagUpLeftPiece = GetPieceInDirection(position, "diagUpLeft", board);
        Piece diagUpRightPiece = GetPieceInDirection(position,"diagUpRight", board);
        Piece diagDownLeft = GetPieceInDirection(position, "diagDownLeft", board);
        Piece diagDownRight = GetPieceInDirection(position, "diagDownRight", board);
        return new Piece[] {rightDirPiece, leftDirPiece, downDirPiece, upDirPiece, diagDownLeft, diagDownRight, diagUpLeftPiece, diagUpRightPiece};
    }

    private bool CheckIfAnyPieceCanAttack(Piece[] pieces, Coordinates position, bool isWhiteTurn)
    {

        foreach (Piece piece in pieces)
        {
            if (piece != null && IsPieceEnemy(piece, isWhiteTurn) && piece.CanAttack(position)) {  return true; }
        }

        return false;
    }

    private bool IsPieceEnemy(Piece piece, bool isWhiteTurn)
    {
        if (piece == null)
        {
            return false;
        }
        if (isWhiteTurn && !piece.IsWhite())
        {
            return true;
        }
        if (!isWhiteTurn && piece.IsWhite())
        {
            return true;
        }
        return false;
    }

    private Piece GetPieceInDirection(Coordinates position, String direction, Board board)
    {
        Piece piece = null;
        Vector2 dir = directions[direction];
        for (int i = 1; i < 8; i++)
        {
            dir = dir * i;
            if(position.x + dir.x >= 0 && position.y + dir.y >= 0 && position.x + dir.x < 8 && position.y + dir.y < 8)
            {
                piece = board.GetPieceAt(new Coordinates((int)(position.x + dir.x), (int)(position.y + dir.y)));
            }
            
        }
        return piece;
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

