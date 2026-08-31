using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece
{
    public bool isWhite;
    public BoardPosition position;
    private int direction = 1; // 1 for white, -1 for black


    public BoardPosition[] GenerateMoves(Board board)
    {
        List<BoardPosition> coordinates = new List<BoardPosition>();
        BoardPosition frontMove = new BoardPosition(position.x + 1 * direction, position.y);
        BoardPosition frontDoubleMove = new BoardPosition(position.x + 2 * direction, position.y);
        BoardPosition frontLeftMove = new BoardPosition(position.x + 1 * direction, position.y - 1);
        BoardPosition frontRightMove = new BoardPosition(position.x + 1 * direction, position.y + 1);
        if (board.IsEmptyHouse(frontMove))
        {
            coordinates.Add(frontMove);
            if(IsFirstMove() && board.IsEmptyHouse(frontDoubleMove))
            {
                coordinates.Add(frontDoubleMove);
            }
        }
        Piece rightEnemy = board.GetPieceAt(frontRightMove);
        Piece leftEnemy = board.GetPieceAt(frontLeftMove);
        if (rightEnemy != null && rightEnemy.IsWhite() != isWhite)
        {
            coordinates.Add(frontRightMove);
        }
        if (leftEnemy != null && leftEnemy.IsWhite() != isWhite)
        {
            coordinates.Add(frontLeftMove);
        }
        return coordinates.ToArray();
    }

    public BoardPosition GetPosition()
    {
        return position;
    }

    public void SetColor(bool isWhite)
    {
        this.isWhite = isWhite;
        direction = isWhite ? 1 : -1;
    }

    public bool IsWhite()
    {
        return isWhite;
    }

    public bool CanAttack(BoardPosition target)
    {
        return (target.y == position.y - 1 || target.y == position.y + 1) && target.x == position.x + 1 * direction;
    }

    public void SetPosition(BoardPosition position)
    {
        this.position = position;
    }

    public bool IsFirstMove()
    {
        int firstX = isWhite ? 1 : 6;
        return firstX == position.x;
    }


    public Piece DuplicatePiece()
    {
        Piece piece = new Pawn();
        piece.SetColor(isWhite);
        piece.SetPosition(position);
        return piece;
    }
}
