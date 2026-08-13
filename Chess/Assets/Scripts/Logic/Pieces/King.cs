using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class King : Piece
{
    private bool isWhite;
    public Coordinates position;


    public bool CanAttack(Coordinates target)
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

        foreach (Coordinates move in moves)
        {
            if (move.x == target.x && move.y == target.y)
            {
                return true;
            }
        }
        return false;
    }


    public Coordinates[] GenerateMoves(Board board)
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

        List<Coordinates> validMoves = new List<Coordinates>();

        foreach (Coordinates move in moves)
        {
            if (board.IsEmptyHouse(move))
            {
                validMoves.Add(move);
            }
            else if (board.GetPieceAt(move) != null && board.GetPieceAt(move).IsWhite() != isWhite)
            {
                validMoves.Add(move);
            }
        }

        return validMoves.ToArray();
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
        Piece piece = new King();
        piece.SetPosition(position);
        piece.SetColor(isWhite);
        return piece;
    }
}
