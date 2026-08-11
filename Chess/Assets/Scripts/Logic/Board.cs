

using System.Collections.Generic;
using UnityEngine;

public class Board
{
    private Piece[,] table = new Piece[8, 8];
    private Coordinates blackKingPosition;
    private Coordinates whiteKingPosition;

    public Board()
    {
        for (int i = 0; i < 8; i++)
        {
            Coordinates whitePawnPosition = new Coordinates(1, i);
            Coordinates blackPawnPosition = new Coordinates(6, i);
            Pawn whitePaw = new Pawn();
            Pawn blackPaw = new Pawn();
            whitePaw.SetColor(true);
            blackPaw.SetColor(false);
            PlacePiece(whitePaw, whitePawnPosition);
            PlacePiece(blackPaw, blackPawnPosition);
        }


        Coordinates whiteHorse1 = new Coordinates(0, 1);
        Coordinates whiteHorse2 = new Coordinates(0, 6);
        Coordinates blackHorse1 = new Coordinates(7, 1);
        Coordinates blackHorse2 = new Coordinates(7, 6);
        Horse whiteHorseLeft = new Horse();
        Horse blackHorseLeft = new Horse();
        Horse whiteHorseRight = new Horse();
        Horse blackHorseRight = new Horse();
        whiteHorseLeft.SetColor(true);
        whiteHorseRight.SetColor(true);
        blackHorseLeft.SetColor(false);
        blackHorseRight.SetColor(false);
        PlacePiece(whiteHorseLeft, whiteHorse1);
        PlacePiece(blackHorseLeft, blackHorse1);
        PlacePiece(whiteHorseRight, whiteHorse2 );
        PlacePiece(blackHorseRight, blackHorse2);

        Coordinates whiteRook1 = new Coordinates(0, 0);
        Coordinates whiteRook2 = new Coordinates(0, 7);
        Coordinates blackRook1 = new Coordinates(7, 0);
        Coordinates blackRook2 = new Coordinates(7, 7);
        Rook whiteRookLeft = new Rook();
        Rook blackRookLeft = new Rook();
        Rook whiteRookRight = new Rook();
        Rook blackRookRight = new Rook();
        whiteRookLeft.SetColor(true);
        whiteRookRight.SetColor(true);
        blackRookLeft.SetColor(false);
        blackRookRight.SetColor(false);
        PlacePiece(whiteRookLeft, whiteRook1);
        PlacePiece(blackRookLeft, blackRook1);
        PlacePiece(whiteRookRight, whiteRook2);
        PlacePiece(blackRookRight, blackRook2);

        Coordinates bishopPos1 = new Coordinates(0, 2);
        Coordinates bishopPos2 = new Coordinates(0, 5);
        Coordinates bishopPos3 = new Coordinates(7, 2);
        Coordinates bishopPos4 = new Coordinates(7, 5);
        Bishop bishop1 = new Bishop();
        Bishop bishop2 = new Bishop();
        Bishop bishop3 = new Bishop();
        Bishop bishop4 = new Bishop();
        bishop1.SetColor(true);
        bishop2.SetColor(true);
        bishop3.SetColor(false);
        bishop4.SetColor(false);
        PlacePiece(bishop1, bishopPos1);
        PlacePiece(bishop2, bishopPos2);
        PlacePiece(bishop3, bishopPos3);
        PlacePiece(bishop4, bishopPos4);

        Coordinates kingPos1 = new Coordinates(0, 4);
        Coordinates kingPos2 = new Coordinates(7, 4);
        Coordinates queenPos1 = new Coordinates(0, 3);
        Coordinates queenPos2 = new Coordinates(7, 3);
        King king1 = new King();
        King king2 = new King();
        Queen queen1 = new Queen();
        Queen queen2 = new Queen();
        king1.SetColor(true);
        king2.SetColor(false);
        queen1.SetColor(true);
        queen2.SetColor(false);
        PlacePiece(king1, kingPos1);
        PlacePiece(king2, kingPos2);
        PlacePiece(queen1, queenPos1);
        PlacePiece(queen2, queenPos2);

        blackKingPosition = kingPos2;
        whiteKingPosition = kingPos1;

    }

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
            pieceToMove.SetPosition(to);
        }
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
        for (int x = 0; x < 8; x++)
        {
            string row = "| ";
            for (int y = 0; y < 8; y++)
            {
                Piece piece = table[x, y];
                if (piece != null)
                {
                    row += " " + piece.GetType().Name[0] + " |";
                }
                else
                {
                    row += " X |";
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
