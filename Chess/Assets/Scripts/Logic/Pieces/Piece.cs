using System.Collections.Generic;
using UnityEngine;

public interface Piece
{
    public BoardPosition[] GenerateMoves(Board board);
    public bool CanAttack(BoardPosition target);
    public void SetPosition(BoardPosition position);
    public BoardPosition GetPosition();
    public void SetColor(bool isWhite);
    public bool IsWhite();
    public Piece DuplicatePiece();
}
