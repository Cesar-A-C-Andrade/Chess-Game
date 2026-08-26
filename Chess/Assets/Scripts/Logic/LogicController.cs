using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LogicController : MonoBehaviour
{

    [SerializeField]
    InputField inputField;

    private Move lastMovement;
    private bool isWhiteTurn = true;
    private Piece pieceSelected = null;
    private Coordinates[] validMovesForPieceSelected = null;


    Board board;
    MoveValidator moveValidator = new MoveValidator();
    GameStateManager stateManager = new GameStateManager();
    XequeManager xequeManager = new XequeManager();
    SpecialMoveManager specialMoveManager = new SpecialMoveManager();


    void Start()
    {
        EventBus.instance.Subscribe<OnHouseSelectedEvent>(HandleHouseSelectedEvent);
        board = new Board();
        board.StartBoard();
        //board.TestScenario();
        board.PrintBoard();
        lastMovement = new Move(null, null, new Coordinates(-1,-1), new Coordinates(-1, -1), false, false);
        stateManager.SaveState(lastMovement, board.DuplicateBoard(), isWhiteTurn);
    }

    void MovePiece(Coordinates to)
    {
        if (specialMoveManager.IsSpecialMove(pieceSelected, stateManager.LoadLastState(), to))
        {
            lastMovement = specialMoveManager.MakeSpecialMove(pieceSelected, lastMovement, board, to);
            ChangeTurn();
            return;
        }
        lastMovement = board.MovePiece(pieceSelected.GetPosition(), to);
        ChangeTurn();
    }

    void ChangeTurn()
    {
        isWhiteTurn = !isWhiteTurn;
        pieceSelected = null;
        validMovesForPieceSelected = null;
        stateManager.SaveState(lastMovement, board.DuplicateBoard(), isWhiteTurn);
        board.PrintBoard();
    }

    public void OnPieceSelected(Coordinates pieceLocation)
    {
        Piece piece = board.GetPieceAt(pieceLocation);

        if (piece.IsWhite() != isWhiteTurn)
        {
            Debug.Log("Not your turn");
            return;
        }

        List<Coordinates> moves = new List<Coordinates>();
        foreach (Coordinates move in moveValidator.ValidateMoves(piece, piece.GenerateMoves(board), stateManager.LoadLastState()))
        {
            moves.Add(move);
        }
        Coordinates[] specialMoves = specialMoveManager.GetSpecialMoveCoordinates(stateManager.LoadLastState(), piece);
        foreach (Coordinates move in moveValidator.ValidateMoves(piece, specialMoves, stateManager.LoadLastState()))
        {
            moves.Add(move);
        }
        EventBus.instance.Invoke<OnPieceSelectedEvent>(new OnPieceSelectedEvent(moves.ToArray()));
        pieceSelected = piece;
        validMovesForPieceSelected = moves.ToArray();

        //Enviar as coordenadas para o board UI marcar as posições que o usuário pode clicar no board.

    }

    public bool IsValidMoveForSelectedPiece(Coordinates move)
    {
        foreach (Coordinates validMove in validMovesForPieceSelected)
        {
            if (validMove.Equals(move)) return true;
        }
        return false;
    }

    public void OnPieceMoved(Coordinates newPieceLocation)
    {
        if (!(IsValidMoveForSelectedPiece(newPieceLocation))) { Debug.Log("Movimento invalido, tente outro por favor"); return; }
        MovePiece(newPieceLocation);
        EventBus.instance.Invoke<OnTableChangedEvent>(new OnTableChangedEvent(board.ConvertBoardIntoStringData(), board.ConvertBoardIntoColorsData()));
        stateManager.PrintState();
    }

    public Move GetLastMove()
    {
        return lastMovement;
    }

    public void TestButtonPressed()
    {
        Coordinates coordinates = new Coordinates(int.Parse(inputField.text[0].ToString()), int.Parse(inputField.text[1].ToString()));
        OnHousePressed(coordinates);
    }
     
    public void OnHousePressed(Coordinates house)
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

}

public struct Move
{
    public Piece pieceMoved;
    public Piece secondPieceMoved;
    public Coordinates lastCoordinate;
    public Coordinates newCoordinate;
    public bool isShortHoque;
    public bool isGrandHoque;

    public Move(Piece piece, Piece destroyed, Coordinates lastCoordinates, Coordinates newCoordinates, bool isShortHoque, bool isGrandHoque)
    {
        this.pieceMoved = piece;
        this.lastCoordinate = lastCoordinates; 
        this.newCoordinate = newCoordinates;
        this.secondPieceMoved = destroyed;
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


public struct Coordinates
{
    public int x;
    public int y;

    public Coordinates(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public bool Equals(Coordinates coordinate)
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