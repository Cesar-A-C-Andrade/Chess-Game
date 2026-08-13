using System;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : Piece
{
    private Dictionary<string, Vector2> directions = new Dictionary<string, Vector2>
    {
        { "diagUpLeft", new Vector2(-1, 1) },
        { "diagUpRight", new Vector2(1, 1) },
        { "diagDownLeft", new Vector2(-1, -1) },
        { "diagDownRight", new Vector2(1, -1) }
    };


    private bool isWhite;
    public Coordinates position;
    public bool CanAttack(Coordinates target)
    {
        if (target.x - target.y == position.x - position.y || target.x + target.y == position.x + position.y)
        {
            return true;
        }
        return false;
    }


    public Coordinates[] GenerateMoves(Board board)
    {
        List<Coordinates> moves = new List<Coordinates>();
        foreach (Coordinates move in GetMovesInDirection("diagUpLeft", board))
        {
            moves.Add(move);
        }
        foreach (Coordinates move in GetMovesInDirection("diagUpRight", board))
        {
            moves.Add(move);
        }
        foreach (Coordinates move in GetMovesInDirection("diagDownLeft", board))
        {
            moves.Add(move);
        }
        foreach (Coordinates move in GetMovesInDirection("diagDownRight", board))
        {
            moves.Add(move);
        }
        return moves.ToArray();
    }

    public bool IsWhite()
    {
        return isWhite;
    }

    public Coordinates GetPosition()
    {
        return position;
    }

    public void SetColor(bool isWhite)
    {
        this.isWhite = isWhite;
    }

    public void SetPosition(Coordinates position)
    {
        this.position = position;
    }

    public Piece DuplicatePiece()
    {
        Piece piece = new Bishop();
        piece.SetColor(isWhite);
        piece.SetPosition(position);
        return piece;
    }

    private Coordinates[] GetMovesInDirection(String direction, Board board)
    {
        Piece piece = null;
        Vector2 dir = directions[direction];
        List<Coordinates> moves = new List<Coordinates>();
        for (int i = 1; i < 8; i++)
        {
            dir = directions[direction] * i;
            if (position.x + dir.x < 0 || position.y + dir.y < 0 || position.x + dir.x >= 8 || position.y + dir.y >= 8)
            {
                break;
            }
            Coordinates move = new Coordinates((int)(position.x + dir.x), (int)(position.y + dir.y));
            if (position.x + dir.x >= 0 && position.y + dir.y >= 0 && position.x + dir.x < 8 && position.y + dir.y < 8)
            {
                piece = board.GetPieceAt(move);
            }
            if (piece != null)
            {
                if (piece.IsWhite() != isWhite)
                {
                    moves.Add(move);
                }
                break;
            }
            moves.Add(move);
        }
        return moves.ToArray();
    }
}
