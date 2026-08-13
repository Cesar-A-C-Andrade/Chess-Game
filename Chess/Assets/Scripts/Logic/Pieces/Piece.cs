using System.Collections.Generic;
using UnityEngine;

public interface Piece
{
    public Coordinates[] GenerateMoves(Board board);
    public bool CanAttack(Coordinates target);
    public void SetPosition(Coordinates position);
    public Coordinates GetPosition();
    public void SetColor(bool isWhite);
    public bool IsWhite();
    public Piece DuplicatePiece();
}
