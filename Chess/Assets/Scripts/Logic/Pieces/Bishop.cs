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
    public BoardPosition position;
    public bool CanAttack(BoardPosition target)
    {
        if (target.x - target.y == position.x - position.y || target.x + target.y == position.x + position.y)
        {
            return true;
        }
        return false;
    }


    public BoardPosition[] GenerateMoves(Board board)
    {
        List<BoardPosition> moves = new List<BoardPosition>();
        foreach (BoardPosition move in GetMovesInDirection("diagUpLeft", board))
        {
            moves.Add(move);
        }
        foreach (BoardPosition move in GetMovesInDirection("diagUpRight", board))
        {
            moves.Add(move);
        }
        foreach (BoardPosition move in GetMovesInDirection("diagDownLeft", board))
        {
            moves.Add(move);
        }
        foreach (BoardPosition move in GetMovesInDirection("diagDownRight", board))
        {
            moves.Add(move);
        }
        return moves.ToArray();
    }

    public bool IsWhite()
    {
        return isWhite;
    }

    public BoardPosition GetPosition()
    {
        return position;
    }

    public void SetColor(bool isWhite)
    {
        this.isWhite = isWhite;
    }

    public void SetPosition(BoardPosition position)
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

    private BoardPosition[] GetMovesInDirection(String direction, Board board)
    {
        Piece piece = null;
        Vector2 dir = directions[direction];
        List<BoardPosition> moves = new List<BoardPosition>();
        for (int i = 1; i < 8; i++)
        {
            dir = directions[direction] * i;
            if (position.x + dir.x < 0 || position.y + dir.y < 0 || position.x + dir.x >= 8 || position.y + dir.y >= 8)
            {
                break;
            }
            BoardPosition move = new BoardPosition((int)(position.x + dir.x), (int)(position.y + dir.y));
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
