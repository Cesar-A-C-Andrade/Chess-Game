using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class King : Piece
{
    private bool isWhite;
    public BoardPosition position;


    public bool CanAttack(BoardPosition target)
    {

        BoardPosition[] moves = new BoardPosition[8];
        moves[0] = new BoardPosition(position.x + 1, position.y);
        moves[1] = new BoardPosition(position.x - 1, position.y);
        moves[2] = new BoardPosition(position.x, position.y + 1);
        moves[3] = new BoardPosition(position.x, position.y - 1);
        moves[4] = new BoardPosition(position.x + 1, position.y + 1);
        moves[5] = new BoardPosition(position.x + 1, position.y - 1);
        moves[6] = new BoardPosition(position.x - 1, position.y + 1);
        moves[7] = new BoardPosition(position.x - 1, position.y - 1);

        foreach (BoardPosition move in moves)
        {
            if (move.x == target.x && move.y == target.y)
            {
                return true;
            }
        }
        return false;
    }


    public BoardPosition[] GenerateMoves(Board board)
    {
        BoardPosition[] moves = new BoardPosition[8];
        moves[0] = new BoardPosition(position.x + 1, position.y);
        moves[1] = new BoardPosition(position.x - 1, position.y);
        moves[2] = new BoardPosition(position.x, position.y + 1);
        moves[3] = new BoardPosition(position.x, position.y - 1);
        moves[4] = new BoardPosition(position.x + 1, position.y + 1);
        moves[5] = new BoardPosition(position.x + 1, position.y - 1);
        moves[6] = new BoardPosition(position.x - 1, position.y + 1);
        moves[7] = new BoardPosition(position.x - 1, position.y - 1);

        List<BoardPosition> validMoves = new List<BoardPosition>();

        foreach (BoardPosition move in moves)
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

    public BoardPosition GetPosition()
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

    public void SetPosition(BoardPosition position)
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
