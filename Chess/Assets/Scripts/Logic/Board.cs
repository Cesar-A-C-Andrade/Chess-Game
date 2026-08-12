

using System.Collections.Generic;
using UnityEngine;

public class Board
{
    private Piece[,] table = new Piece[8, 8];
    private Coordinates blackKingPosition;
    private Coordinates whiteKingPosition;



    public void StartBoard()
    {
        for (int i = 0; i < 8; i++)
        {
            CreateAndPlacePiece("pawn", 1, i, true);
            CreateAndPlacePiece("pawn", 6, i, false);
        }

        CreateAndPlacePiece("horse", 0, 1, true);
        CreateAndPlacePiece("horse", 0, 6, true);
        CreateAndPlacePiece("horse", 7, 1, false);
        CreateAndPlacePiece("horse", 7, 6, false);

        CreateAndPlacePiece("rook", 0, 0, true);
        CreateAndPlacePiece("rook", 0, 7, true);
        CreateAndPlacePiece("rook", 7, 0, false);
        CreateAndPlacePiece("rook", 7, 7, false);


        CreateAndPlacePiece("bishop", 0, 2, true);
        CreateAndPlacePiece("bishop", 0, 5, true);
        CreateAndPlacePiece("bishop", 7, 2, false);
        CreateAndPlacePiece("bishop", 7, 5, false);

        CreateAndPlacePiece("king", 0, 4, true);
        CreateAndPlacePiece("king", 7, 4, false);
        CreateAndPlacePiece("queen", 0, 3, true);
        CreateAndPlacePiece("queen", 7, 3, false);

        whiteKingPosition = new Coordinates(0, 4);
        blackKingPosition = new Coordinates(7, 4);

    }

    public void TestScenario()
    {
        CreateAndPlacePiece("rook", 7, 0, false);
        CreateAndPlacePiece("rook", 7, 1, false);
        CreateAndPlacePiece("rook", 1, 7, true);
        CreateAndPlacePiece("king", 0,0, true);
        CreateAndPlacePiece("king", 7, 4, false);


        blackKingPosition = new Coordinates(7, 4);
        whiteKingPosition = new Coordinates(0, 0);
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

    public Move MovePiece(Coordinates from, Coordinates to)
    {
        Piece pieceToMove = GetPieceAt(from);
        Piece pieceInLocation = GetPieceAt(to);
        if (pieceToMove != null)
        {
            table[to.x, to.y] = pieceToMove;
            table[from.x, from.y] = null;
            pieceToMove.SetPosition(to);
            if (pieceToMove is King)
            {
                SetKingPosition(pieceToMove.IsWhite(), to);
            }
            return new Move(pieceToMove, pieceInLocation, from, to);
        }
        return new Move(null, null, from, to);
    }

    public void PlacePiece(Piece piece, Coordinates coordinates)
    {
        if (!IsValidCoordinate(coordinates)) return;
        table[coordinates.x, coordinates.y] = piece;
        piece.SetPosition(coordinates);
    }

    public void CreateAndPlacePiece(string pieceType, int xCoordinate, int yCoordinate, bool isPieceWhite)
    {
        Coordinates pieceCoordinate = new Coordinates(xCoordinate, yCoordinate);
        PlacePiece(PieceFactory(pieceType, isPieceWhite), pieceCoordinate);
    }

    public void RemovePieceAt(Coordinates from)
    {
        Piece removedPiece = GetPieceAt(from);
        removedPiece = null;
        table[from.x, from.y] = null;
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

    public void SetKingPosition(bool isWhite, Coordinates newPos)
    {
        if (isWhite)
        {
            whiteKingPosition = newPos;
            return;
        }
        blackKingPosition = newPos;
        return;
    }

    public bool IsValidCoordinate(Coordinates position)
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
                if (table[i, j] != null)
                {
                    Piece newPiece = table[i, j].DuplicatePiece();
                    board.PlacePiece(newPiece, new Coordinates(i, j));
                }
                
                    
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

    public Piece[] GetPiecesByColor(string color)
    {
        List<Piece> pieces = new List<Piece>();
        switch (color.ToLower())
        {
            case "white":
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if(table[i, j] == null) { continue; }
                        if(table[i, j].IsWhite())
                        {
                            pieces.Add(table[i, j]);
                        }
                    }
                }
                break;
            case "black":
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (!(table[i, j].IsWhite()))
                        {
                            pieces.Add(table[i, j]);
                        }
                    }
                }
                break;
        }
        return pieces.ToArray();
    }

    public Piece PieceFactory(string pieceType, bool pieceIsWhite)
    {
        Piece piece;
        switch (pieceType)
        {
            case "pawn":
                piece = new Pawn();
                break;
            case "rook":
                piece = new Rook();
                break;
            case "queen":
                piece = new Queen();
                break;
            case "king":
                piece = new King();
                break;
            case "bishop":
                piece = new Bishop();
                break;
            case "horse":
                piece = new Horse();
                break;
            default:
                piece = new Pawn();
                break;
        }
        piece.SetColor(pieceIsWhite);
        return piece;
    }

    public void UndoMove(Move lastMove)
    {
        Piece pieceDestroyed = lastMove.pieceDestroyed;
        Piece pieceMoved = lastMove.pieceMoved;
        MovePiece(lastMove.newCoordinate, lastMove.lastCoordinate);
        if (pieceDestroyed == null)
        {
            table[lastMove.newCoordinate.x, lastMove.newCoordinate.y] = null;
            return;
        }
        table[pieceDestroyed.GetPosition().x, pieceDestroyed.GetPosition().y] = pieceDestroyed;
    }
}
