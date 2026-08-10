using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece
{
    public bool isWhite;
    public Coordinates position;
    private int direction = 1; // 1 for white, -1 for black
    private bool isFirstMove = true;

    public Coordinates[] GenerateMoves(Board board)
    {
        List<Coordinates> coordinates = new List<Coordinates>();
        Coordinates frontMove = new Coordinates(position.x + 1 * direction, position.y);
        Coordinates frontDoubleMove = new Coordinates(position.x + 2 * direction, position.y);
        Coordinates frontLeftMove = new Coordinates(position.x + 1 * direction, position.y - 1);
        Coordinates frontRightMove = new Coordinates(position.x + 1 * direction, position.y + 1);
        if (board.IsEmptyHouse(frontMove))
        {
            coordinates.Add(frontMove);
            if(isFirstMove && board.IsEmptyHouse(frontDoubleMove))
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

    public Coordinates GetPosition()
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

    public bool CanAttack(Coordinates target)
    {
        return (target.y == position.y - 1 || target.y == position.y + 1) && target.x == position.x + 1 * direction;
    }

    public void SetPosition(Coordinates position)
    {
        this.position = position;
    }

    public void SetIsFirstMove(bool value)
    {
        isFirstMove = value;
    }

    public bool GetIsFirstMove()
    {
        return isFirstMove;
    }

    public Piece DuplicatePiece()
    {
        throw new System.NotImplementedException();
    }
}
