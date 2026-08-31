using NUnit.Framework;
using NUnit.Framework.Constraints;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class SpecialMoveManager
{
    private AttackDetector detector = new AttackDetector();
    private BoardPosition[] grandHoqueWhiteCoordinates = new BoardPosition[3];
    private BoardPosition[] grandHoqueBlackCoordinates = new BoardPosition[3];
    private BoardPosition[] shortHoqueWhiteCoordinates = new BoardPosition[2];
    private BoardPosition[] shortHoqueBlackCoordinates = new BoardPosition[2];

    private bool grandHoqueWhite = true;
    private bool grandHoqueBlack = true;
    private bool shortHoqueWhite = true;
    private bool shortHoqueBlack = true;

    Dictionary<bool, bool> grandHoque = new Dictionary<bool, bool>();
    Dictionary<bool, bool> shortHoque = new Dictionary<bool, bool>();
    Dictionary<bool, BoardPosition[]> grandHoqueCoordinates = new Dictionary<bool, BoardPosition[]>();
    Dictionary<bool, BoardPosition[]> shortHoqueCoordinates = new Dictionary<bool, BoardPosition[]>();

    public SpecialMoveManager()
    {
        shortHoqueWhiteCoordinates[0] = new BoardPosition(0, 1);
        shortHoqueWhiteCoordinates[1] = new BoardPosition(0, 2);
        grandHoqueWhiteCoordinates[0] = new BoardPosition(0, 4);
        grandHoqueWhiteCoordinates[1] = new BoardPosition(0, 5);
        grandHoqueWhiteCoordinates[2] = new BoardPosition(0, 6);
        shortHoqueBlackCoordinates[0] = new BoardPosition(7, 1);
        shortHoqueBlackCoordinates[1] = new BoardPosition(7, 2);
        grandHoqueBlackCoordinates[0] = new BoardPosition(7, 4);
        grandHoqueBlackCoordinates[1] = new BoardPosition(7, 5);
        grandHoqueBlackCoordinates[2] = new BoardPosition(7, 6);
        grandHoque.Add(true, grandHoqueWhite);
        grandHoque.Add(false, grandHoqueBlack);
        shortHoque.Add(true, shortHoqueWhite);
        shortHoque.Add(false, shortHoqueBlack);
        grandHoqueCoordinates.Add(true, grandHoqueWhiteCoordinates);
        grandHoqueCoordinates.Add(false, grandHoqueBlackCoordinates);
        shortHoqueCoordinates.Add(true, shortHoqueWhiteCoordinates);
        shortHoqueCoordinates.Add(false, shortHoqueBlackCoordinates);
    }

    public BoardPosition GetEnPassantCoordinate(Move lastMovement, Pawn pawn)
    {
        BoardPosition lastCoordinate = lastMovement.lastCoordinate;
        BoardPosition newCoordinate = lastMovement.newCoordinate;
        BoardPosition enPassantCoordinate = new BoardPosition(-1, -1);
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
            return new BoardPosition(-1, -1);
        }
        return enPassantCoordinate;
    }

    public bool IsSpecialMove(Piece piece, GameState gameState, BoardPosition targetPosition)
    {
        if (piece == null) return false;
        if (piece is Pawn)
        {
            if (targetPosition.Equals(GetEnPassantCoordinate(gameState.lastMovement, piece as Pawn))) return true;
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
        return false;
    }

    public Move MakeSpecialMove(Piece piece, Move lastMovement, Board board, BoardPosition targetPosition)
    {
        Move move;
        if (piece is Pawn) 
        {
            if (targetPosition.Equals(GetEnPassantCoordinate(lastMovement, piece as Pawn)))
            {
                move = MakeEnPassant(piece as Pawn, lastMovement, board);
                Debug.Log("AAAAAAAAAAAA");
                return move;
            }
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
        BoardPosition enPassant = GetEnPassantCoordinate(lastMovement, pawn);
        BoardPosition lastPawnPosition = lastMovement.pieceMoved.GetPosition();
        Move enPassantMove = new Move(pawn, lastMovement.pieceMoved, pawn.GetPosition(), enPassant, false, false);
        board.MovePiece(pawn.GetPosition(), enPassant);
        board.RemovePieceAt(lastPawnPosition);
        return enPassantMove;
    }
    
    public Move MakeGrandHoque(Board board , bool isWhite)
    {
        BoardPosition kingPosition = board.GetKingPosition(isWhite);
        BoardPosition rookPosition = isWhite ? new BoardPosition(0, 7) : new BoardPosition(7 , 7);
        BoardPosition hoquePosition = grandHoqueCoordinates[isWhite][1];
        BoardPosition newRookPosition = grandHoqueCoordinates[isWhite][0];
        board.MovePiece(kingPosition, hoquePosition);
        board.MovePiece(rookPosition, newRookPosition);
        Move granHoqueMove = new Move(null, null, kingPosition, rookPosition , false, true);
        grandHoque[isWhite] = false;
        return granHoqueMove;
    }
    
    public Move MakeShortHoque(Board board, bool isWhite)
    {
        BoardPosition kingPosition = board.GetKingPosition(isWhite);
        BoardPosition rookPosition = isWhite ? new BoardPosition(0, 0) : new BoardPosition(7, 0);
        BoardPosition hoquePosition = shortHoqueCoordinates[isWhite][0];
        BoardPosition newRookPosition = shortHoqueCoordinates[isWhite][1];
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
        foreach (BoardPosition house in shortHoqueCoordinates[isWhite])
        {
            if (detector.IsHouseUnderAttack(board, house, isWhite)) { return false; }
        }
        foreach (BoardPosition house in shortHoqueCoordinates[isWhite])
        {
            if (board.GetPieceAt(house) != null ) { return false; }
        }
        return true;
    }

    public bool CanMakeGrandHoque(bool isWhite, Board board)
    {
        if (!grandHoque[isWhite]) { return false; }
        if (detector.IsHouseUnderAttack(board, board.GetKingPosition(isWhite), isWhite)) { return false; }
        foreach (BoardPosition house in grandHoqueCoordinates[isWhite])
        {
            if (detector.IsHouseUnderAttack(board, house, isWhite)) { return false;}
        }
        foreach (BoardPosition house in grandHoqueCoordinates[isWhite])
        {
            if (board.GetPieceAt(house) != null) { return false; }
        }
        return true;
    }

    public BoardPosition GetGrandHoqueCoordinates(bool isWhiteKing, Board board)
    {
        BoardPosition coordinates = new BoardPosition(-1, -1);

        if(CanMakeGrandHoque(isWhiteKing, board))
        {
            coordinates = grandHoqueCoordinates[isWhiteKing][1];
        } 
        return coordinates;
    }

    public BoardPosition GetShortHoqueCoordinates(bool isWhiteKing, Board board)
    {
        BoardPosition coordinates = new BoardPosition(-1, -1);

        if (CanMakeShortHoque(isWhiteKing, board))
        {
            coordinates = shortHoqueCoordinates[isWhiteKing][0];
        }
        return coordinates;
    }

    public BoardPosition[] GetSpecialMoveCoordinates(GameState gameState, Piece piece)
    {
        List<BoardPosition> coordinates = new List<BoardPosition>();
        if(piece is Pawn)
        {
            BoardPosition enPassant = GetEnPassantCoordinate(gameState.lastMovement, piece as Pawn);
            if (enPassant.IsEmpty())
            {
                return coordinates.ToArray();
            }
            coordinates.Add(enPassant);
        }
        if(piece is King)
        {
            BoardPosition grandHoque = GetGrandHoqueCoordinates(piece.IsWhite(), gameState.board);
            BoardPosition shortHoque = GetShortHoqueCoordinates(piece.IsWhite(), gameState.board);
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

    public bool PawnReachesPromotion(Piece pawn)
    {
        if(pawn is not Pawn) {  return false; }
        return pawn.IsWhite() ? pawn.GetPosition().x == 7 : pawn.GetPosition().x == 0;
    }
}
