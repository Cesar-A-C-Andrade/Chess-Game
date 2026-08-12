using System;
using System.Collections.Generic;
using UnityEngine;

public class Rook : Piece
{
    private Dictionary<string, Vector2> directions = new Dictionary<string, Vector2>
    {
        { "up", new Vector2(0, 1) },
        { "down", new Vector2(0, -1) },
        { "left", new Vector2(-1, 0) },
        { "right", new Vector2(1, 0) }
    };

    private bool isWhite;
    public Coordinates position;

    public bool CanAttack(Coordinates target)
    {
        if(target.x == position.x || target.y == position.y)
        {
            return true;
        }
        return false;
    }

    public Coordinates[] GenerateMoves(Board board)
    {
        List<Coordinates> moves = new List<Coordinates>();
        foreach (Coordinates move in GetMovesInDirection("up", board))
        {
            moves.Add(move);
        }
        foreach (Coordinates move in GetMovesInDirection("down", board))
        {
            moves.Add(move);
        }
        foreach (Coordinates move in GetMovesInDirection("left", board))
        {
            moves.Add(move);
        }
        foreach (Coordinates move in GetMovesInDirection("right", board))
        {
            moves.Add(move);
        }
        return moves.ToArray();
    }

    public Coordinates GetPosition()
    {
        return position;
    }

    public bool IsWhite()
    {
        return isWhite;
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
        Piece piece = new Rook();
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
            if(piece != null)
            {
                if(piece.IsWhite() != isWhite)
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
