

using System.Collections.Generic;
using UnityEngine;

public class Board
{
    private Piece[,] table = new Piece[8, 8];
    private Coordinates blackKingPosition;
    private Coordinates whiteKingPosition;

    public Piece GetPieceAt(Coordinates coordinates)
    {
        if (!IsValidCoordinate(coordinates)) return null;
        Piece _piece = table[coordinates.x, coordinates.y];
        if (_piece != null)
        {
            return _piece;
        }

        return null;
    }

    public Piece[] GetPiecesAt(Coordinates[] coordinates)
    {
        List<Piece> pieces = new List<Piece>();
        foreach (Coordinates position in coordinates)
        {
            Piece piece = GetPieceAt(position);
            if (piece != null)
            {
                pieces.Add(piece);
            }
        }
        return pieces.ToArray();
    }

    public void MovePiece(Coordinates from, Coordinates to)
    {
        Piece pieceToMove = GetPieceAt(from);
        if (pieceToMove != null)
        {
            table[to.x, to.y] = pieceToMove;
            table[from.x, from.y] = null;
        }
        pieceToMove.SetPosition(to);
    }

    public void PlacePiece(Piece piece, Coordinates coordinates)
    {
        if (!IsValidCoordinate(coordinates)) return;
        table[coordinates.x, coordinates.y] = piece;
        piece.SetPosition(coordinates);
    }

    public void RemovePieceAt(Coordinates from)
    {
        Piece removedPiece = GetPieceAt(from);
        removedPiece = null;
    }

    public void PrintBoard()
    {
        for (int y = 7; y >= 0; y--)
        {
            string row = "";
            for (int x = 0; x < 8; x++)
            {
                Piece piece = table[x, y];
                if (piece != null)
                {
                    row += piece.GetType().Name[0] + " "; 
                }
                else
                {
                    row += ". "; 
                }
            }
            Debug.Log(row);
        }
    }

    public Coordinates GetKingPosition(bool isWhite)
    {
        return isWhite ? whiteKingPosition : blackKingPosition;
    }

    private bool IsValidCoordinate(Coordinates position)
    {
        return position.x >= 0 && position.x < 8 && position.y >= 0 && position.y < 8;
    }

    public Board DuplicateBoard()
    {
        Board board = new Board();
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                Piece newPiece = table[i, j].DuplicatePiece();
                board.PlacePiece(newPiece, new Coordinates(i, j));
            }
        }
        board.blackKingPosition = new Coordinates(blackKingPosition.x, blackKingPosition.y);
        board.whiteKingPosition = new Coordinates(whiteKingPosition.x, whiteKingPosition.y);

        return board;
    }

    public bool IsEmptyHouse(Coordinates position)
    {
        if (!(IsValidCoordinate(position)))
        {
            return false;
        }
        return GetPieceAt(position) == null;
    }
}
