using NUnit.Framework;
using NUnit.Framework.Constraints;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class SpecialMoveManager
{
    private AttackDetector detector = new AttackDetector();
    private Coordinates[] grandHoqueWhiteCoordinates = new Coordinates[3];
    private Coordinates[] grandHoqueBlackCoordinates = new Coordinates[3];
    private Coordinates[] shortHoqueWhiteCoordinates = new Coordinates[2];
    private Coordinates[] shortHoqueBlackCoordinates = new Coordinates[2];

    private bool grandHoqueWhite = true;
    private bool grandHoqueBlack = true;
    private bool shortHoqueWhite = true;
    private bool shortHoqueBlack = true;

    Dictionary<bool, bool> grandHoque = new Dictionary<bool, bool>();
    Dictionary<bool, bool> shortHoque = new Dictionary<bool, bool>();
    Dictionary<bool, Coordinates[]> grandHoqueCoordinates = new Dictionary<bool, Coordinates[]>();
    Dictionary<bool, Coordinates[]> shortHoqueCoordinates = new Dictionary<bool, Coordinates[]>();

    public SpecialMoveManager()
    {
        shortHoqueWhiteCoordinates[0] = new Coordinates(0, 1);
        shortHoqueWhiteCoordinates[1] = new Coordinates(0, 2);
        grandHoqueWhiteCoordinates[0] = new Coordinates(0, 4);
        grandHoqueWhiteCoordinates[1] = new Coordinates(0, 5);
        grandHoqueWhiteCoordinates[2] = new Coordinates(0, 6);
        shortHoqueBlackCoordinates[0] = new Coordinates(7, 1);
        shortHoqueBlackCoordinates[1] = new Coordinates(7, 2);
        grandHoqueBlackCoordinates[0] = new Coordinates(7, 4);
        grandHoqueBlackCoordinates[1] = new Coordinates(7, 5);
        grandHoqueBlackCoordinates[2] = new Coordinates(7, 6);
        grandHoque.Add(true, grandHoqueWhite);
        grandHoque.Add(false, grandHoqueBlack);
        shortHoque.Add(true, shortHoqueWhite);
        shortHoque.Add(false, shortHoqueBlack);
        grandHoqueCoordinates.Add(true, grandHoqueWhiteCoordinates);
        grandHoqueCoordinates.Add(false, grandHoqueBlackCoordinates);
        shortHoqueCoordinates.Add(true, shortHoqueWhiteCoordinates);
        shortHoqueCoordinates.Add(false, shortHoqueBlackCoordinates);
    }

    public Coordinates GetEnPassantCoordinate(Move lastMovement, Pawn pawn)
    {
        Coordinates lastCoordinate = lastMovement.lastCoordinate;
        Coordinates newCoordinate = lastMovement.newCoordinate;
        Coordinates enPassantCoordinate = new Coordinates(-1, -1);
        if (!(lastMovement.pieceMoved is Pawn))
        {
            return enPassantCoordinate;
        }
        if (lastCoordinate.x < 0 || newCoordinate.x < 0)
        {
            return enPassantCoordinate;
        }
        int lastPawnDirection = lastMovement.pieceMoved.IsWhite() ? 1 : -1;

        if (Mathf.Abs(lastCoordinate.x - newCoordinate.x) != 2)
        {
            return enPassantCoordinate;
        }

        enPassantCoordinate.x = lastCoordinate.x + (1 * lastPawnDirection);
        enPassantCoordinate.y = lastCoordinate.y;

        if (!pawn.CanAttack(enPassantCoordinate))
        {
            return new Coordinates(-1, -1);
        }
        return enPassantCoordinate;
    }

    public bool IsSpecialMove(Piece piece, GameState gameState, Coordinates targetPosition)
    {
        if (piece == null) return false;
        if (piece is Pawn)
        {
            if (!(targetPosition.Equals(GetEnPassantCoordinate(gameState.lastMovement, piece as Pawn)))) return false;
        }
        if (piece is King)
        {
            if (targetPosition.Equals(GetGrandHoqueCoordinates(piece.IsWhite(), gameState.board)))
            {
                return true;
            }
            if (targetPosition.Equals(GetShortHoqueCoordinates(piece.IsWhite(), gameState.board)))
            {
                return true;
            }
        }
        return true;
    }

    public Move MakeSpecialMove(Piece piece, Move lastMovement, Board board, Coordinates targetPosition)
    {
        Move move;
        if (targetPosition.Equals(GetEnPassantCoordinate(lastMovement, piece as Pawn)))
        {
            move = MakeEnPassant(piece as Pawn, lastMovement, board);
            return move;
        }
        if (targetPosition.Equals(GetGrandHoqueCoordinates(piece.IsWhite(), board)))
        {
            move = MakeGrandHoque(board, piece.IsWhite());
            return move;
        }
        move = MakeShortHoque(board, piece.IsWhite());
        return move;
    }

    public Move MakeEnPassant(Pawn pawn, Move lastMovement, Board board)
    {
        Coordinates enPassant = GetEnPassantCoordinate(lastMovement, pawn);
        Coordinates lastPawnPosition = lastMovement.pieceMoved.GetPosition();
        Move enPassantMove = new Move(pawn, lastMovement.pieceMoved, pawn.GetPosition(), enPassant, false, false);
        board.MovePiece(pawn.GetPosition(), enPassant);
        board.RemovePieceAt(lastPawnPosition);
        return enPassantMove;
    }
    
    public Move MakeGrandHoque(Board board , bool isWhite)
    {
        Coordinates kingPosition = board.GetKingPosition(isWhite);
        Coordinates rookPosition = isWhite ? new Coordinates(0, 7) : new Coordinates(7 , 7);
        Coordinates hoquePosition = grandHoqueCoordinates[isWhite][1];
        Coordinates newRookPosition = grandHoqueCoordinates[isWhite][0];
        board.MovePiece(kingPosition, hoquePosition);
        board.MovePiece(rookPosition, newRookPosition);
        Move granHoqueMove = new Move(null, null, kingPosition, rookPosition , false, true);
        grandHoque[isWhite] = false;
        return granHoqueMove;
    }
    
    public Move MakeShortHoque(Board board, bool isWhite)
    {
        Coordinates kingPosition = board.GetKingPosition(isWhite);
        Coordinates rookPosition = isWhite ? new Coordinates(0, 0) : new Coordinates(7, 0);
        Coordinates hoquePosition = shortHoqueCoordinates[isWhite][0];
        Coordinates newRookPosition = shortHoqueCoordinates[isWhite][1];
        board.MovePiece(kingPosition, hoquePosition);
        board.MovePiece(rookPosition, newRookPosition);
        Move shortHoqueMove = new Move(null, null, kingPosition, rookPosition, true, false);
        shortHoque[isWhite] = false;
        return shortHoqueMove;
    }
    
    public bool CanMakeShortHoque(bool isWhite, Board board)
    {
        if (!shortHoque[isWhite]) { return false; }
        if (detector.IsHouseUnderAttack(board, board.GetKingPosition(isWhite), isWhite)) { return false; }
        foreach (Coordinates house in shortHoqueCoordinates[isWhite])
        {
            if (detector.IsHouseUnderAttack(board, house, isWhite)) { return false; }
        }
        return true;
    }

    public bool CanMakeGrandHoque(bool isWhite, Board board)
    {
        if (!grandHoque[isWhite]) { return false; }
        if (detector.IsHouseUnderAttack(board, board.GetKingPosition(isWhite), isWhite)) { return false; }
        foreach (Coordinates house in grandHoqueCoordinates[isWhite])
        {
            if (detector.IsHouseUnderAttack(board, house, isWhite)) { return false;}
        }
        return true;
    }

    public Coordinates GetGrandHoqueCoordinates(bool isWhiteKing, Board board)
    {
        Coordinates coordinates = new Coordinates(-1, -1);

        if(CanMakeGrandHoque(isWhiteKing, board))
        {
            coordinates = grandHoqueCoordinates[isWhiteKing][1];
        } 
        return coordinates;
    }

    public Coordinates GetShortHoqueCoordinates(bool isWhiteKing, Board board)
    {
        Coordinates coordinates = new Coordinates(-1, -1);

        if (CanMakeShortHoque(isWhiteKing, board))
        {
            coordinates = shortHoqueCoordinates[isWhiteKing][0];
        }
        return coordinates;
    }

    public Coordinates[] GetSpecialMoveCoordinates(GameState gameState, Piece piece)
    {
        List<Coordinates> coordinates = new List<Coordinates>();
        if(piece is Pawn)
        {
            Coordinates enPassant = GetEnPassantCoordinate(gameState.lastMovement, piece as Pawn);
            if (enPassant.IsEmpty())
            {
                return coordinates.ToArray();
            }
            coordinates.Add(enPassant);
        }
        if(piece is King)
        {
            Coordinates grandHoque = GetGrandHoqueCoordinates(piece.IsWhite(), gameState.board);
            Coordinates shortHoque = GetShortHoqueCoordinates(piece.IsWhite(), gameState.board);
            if (!(grandHoque.IsEmpty()))
            {
                coordinates.Add(grandHoque);
            }
            if (!(shortHoque.IsEmpty()))
            {
                coordinates.Add(shortHoque);
            }
        }
        return coordinates.ToArray();
    }
}
