using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Audio.ProcessorInstance;


public class LogicController : MonoBehaviour
{

    [SerializeField]
    InputField inputField;

    private Move lastMovement;
    private bool isWhiteTurn = true;
    private Piece pieceSelected = null;
    private BoardPosition[] validMovesForPieceSelected = null;
    private bool resetGame = false;
    private bool isWaitingForPromotion = false;

    Board board;
    MoveValidator moveValidator = new MoveValidator();
    GameStateManager stateManager = new GameStateManager();
    XequeManager xequeManager = new XequeManager();
    SpecialMoveManager specialMoveManager = new SpecialMoveManager();

    Event<OnPawnReachedPromotion> onPawnReachedPromotionEvent = new Event<OnPawnReachedPromotion>();


    void Start()
    {
        EventBus.instance.Subscribe<OnHouseSelectedEvent>(HandleHouseSelectedEvent);
        EventBus.instance.AddBroadCaster(onPawnReachedPromotionEvent);
        EventBus.instance.Subscribe<OnPawnPromoted>(HandlePawnPromotion);
        StartGame();
    }

    void StartGame()
    {
        resetGame = false;
        board = new Board();
        board.StartBoard();
        isWhiteTurn = true;
        pieceSelected = null;
        validMovesForPieceSelected = null;
        lastMovement = new Move(null, null, new BoardPosition(-1, -1), new BoardPosition(-1, -1), false, false);
        stateManager.SaveState(lastMovement, board.DuplicateBoard(), isWhiteTurn);

    }

    void MovePiece(BoardPosition to)
    {
        if (specialMoveManager.IsSpecialMove(pieceSelected, stateManager.LoadLastState(), to))
        {
            lastMovement = specialMoveManager.MakeSpecialMove(pieceSelected, lastMovement, board, to);
            ChangeTurn();
            return;
        }
        lastMovement = board.MovePiece(pieceSelected.GetPosition(), to);
        if (specialMoveManager.PawnReachesPromotion(pieceSelected))
        {
            BoardPosition pawnPosition = pieceSelected.GetPosition();
            EventBus.instance.Invoke<OnPawnReachedPromotion>(new OnPawnReachedPromotion(pawnPosition));
            isWaitingForPromotion = true;
            return;
        }
        ChangeTurn();
    }

    void ChangeTurn()
    {
        isWhiteTurn = !isWhiteTurn;
        pieceSelected = null;
        validMovesForPieceSelected = null;
        stateManager.SaveState(lastMovement, board.DuplicateBoard(), isWhiteTurn);
        if (xequeManager.XequeChecker(board.DuplicateBoard(), isWhiteTurn))
        {
            if (xequeManager.IsXequeMate(board.DuplicateBoard(), isWhiteTurn, lastMovement))
            {
                Debug.Log("Perdeu otario");
                resetGame = true;
            }
            Debug.Log("Check");
        }
    }

    public void OnPieceSelected(BoardPosition pieceLocation)
    {
        Piece piece = board.GetPieceAt(pieceLocation);
        if (piece.IsWhite() != isWhiteTurn)
        {
            Debug.Log("Not your turn");
            return;
        }
        BoardPosition[] moves = GetPieceMoves(piece);
        EventBus.instance.Invoke<OnPieceSelectedEvent>(new OnPieceSelectedEvent(moves));
        pieceSelected = piece;
        validMovesForPieceSelected = moves;
    }

    BoardPosition[] GetPieceMoves(Piece piece)
    {
        List<BoardPosition> moves = new List<BoardPosition>();
        GameState currentState = stateManager.LoadLastState();

        moveValidator.ValidateMoves(piece, piece.GenerateMoves(currentState.board), currentState, ref moves);

        BoardPosition[] specialMoves = specialMoveManager.GetSpecialMoveCoordinates(currentState, piece);

        moveValidator.ValidateMoves(piece, specialMoves, currentState, ref moves);

        return moves.ToArray();
    }

    public bool IsValidMoveForSelectedPiece(BoardPosition move)
    {
        foreach (BoardPosition validMove in validMovesForPieceSelected)
        {
            if (validMove.Equals(move)) return true;
        }
        return false;
    }

    public void OnPieceMoved(BoardPosition newPieceLocation)
    {
        if (!(IsValidMoveForSelectedPiece(newPieceLocation))) { Debug.Log("Movimento invalido, tente outro por favor"); return; }
        MovePiece(newPieceLocation);
        if (resetGame) { StartGame(); return; }
        if (isWaitingForPromotion) { return; }
        EventBus.instance.Invoke<OnTableChangedEvent>(new OnTableChangedEvent(board.ConvertBoardIntoStringData(), board.ConvertBoardIntoColorsData()));
    }
     
    public void OnHousePressed(BoardPosition house)
    {
        if(pieceSelected != null)
        {
            if(board.GetPieceAt(house) != null && board.GetPieceAt(house).IsWhite() == pieceSelected.IsWhite())
            {
                OnPieceSelected(house);
                return;
            }
            OnPieceMoved(house);
            return;
        }
        if(board.GetPieceAt(house) != null)
        {
            OnPieceSelected(house);
            return;
        }
    }

    void HandleHouseSelectedEvent(OnHouseSelectedEvent _event)
    {
        OnHousePressed(_event.houseSelectedCoordinates);
    }

    void HandlePawnPromotion(OnPawnPromoted _event)
    {
        if (!isWaitingForPromotion) { return; }
        isWaitingForPromotion = false;
        BoardPosition pawnPosition = pieceSelected.GetPosition();
        board.CreateAndPlacePiece(_event.pieceType, pawnPosition.x, pawnPosition.y, pieceSelected.IsWhite());
        ChangeTurn();
        EventBus.instance.Invoke<OnTableChangedEvent>(new OnTableChangedEvent(board.ConvertBoardIntoStringData(), board.ConvertBoardIntoColorsData()));
    }

}

public struct Move
{
    public Piece pieceMoved;
    public Piece secondPieceMoved;
    public BoardPosition lastCoordinate;
    public BoardPosition newCoordinate;
    public bool isShortHoque;
    public bool isGrandHoque;

    public Move(Piece piece, Piece secondPiece, BoardPosition lastCoordinates, BoardPosition newCoordinates, bool isShortHoque, bool isGrandHoque)
    {
        this.pieceMoved = piece;
        this.lastCoordinate = lastCoordinates; 
        this.newCoordinate = newCoordinates;
        this.secondPieceMoved = secondPiece;
        this.isShortHoque = isShortHoque;
        this.isGrandHoque = isGrandHoque;
    }

    public void PrintMovement()
    {
        if (this.isShortHoque)
        {
            Debug.Log("O-O");
            return;
        }
        if (this.isGrandHoque)
        {
            Debug.Log("O-O-O"); 
            return;
        }
        Debug.Log("Piece moved: " + pieceMoved.GetType().Name);
        if(secondPieceMoved != null)
        {
            Debug.Log($"Second Piece Moved {secondPieceMoved.GetType().Name}");
        }
        this.lastCoordinate.PrintCoordinates();
        this.newCoordinate.PrintCoordinates();
    }
}


public struct BoardPosition
{
    public int x;
    public int y;

    public BoardPosition(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public bool Equals(BoardPosition coordinate)
    {
        if (coordinate.x != this.x) return false;
        if (coordinate.y != this.y) return false;
        return true;
    }

    public void PrintCoordinates()
    {
        Debug.Log($"X: {this.x} and Y: {this.y}");
    }

    public bool IsEmpty() 
    {
        return this.x == -1 || this.y == -1;
    }

}