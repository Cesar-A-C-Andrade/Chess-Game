using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LogicController : MonoBehaviour
{

    [SerializeField]
    InputField inputField;
    [SerializeField]
    InputField inputField2;

    private bool granHoqueWhite = true;
    private bool grandHoqueBlack = true;
    private bool shortHoqueWhite = true;
    private bool shortHoqueBlack = true;

    private Move lastMovement;
    private bool isWhiteTurn = true;


    Board board;
    MoveValidator moveValidator = new MoveValidator();
    GameStateManager stateManager = new GameStateManager();
    XequeManager xequeManager = new XequeManager();
    SpecialMoveManager specialMoveManager = new SpecialMoveManager();


    void Start()
    {
        board = new Board();
        board.StartBoard();
        board.PrintBoard();
        lastMovement = new Move(null, null, new Coordinates(-1,-1), new Coordinates(-1, -1));
        stateManager.SaveState(granHoqueWhite, grandHoqueBlack, shortHoqueWhite, shortHoqueBlack, lastMovement, board.DuplicateBoard(), isWhiteTurn);
    }

    void MovePiece(Coordinates from, Coordinates to)
    {
        if(specialMoveManager.IsSpecialMove(board.GetPieceAt(from), lastMovement, to))
        {
            lastMovement = specialMoveManager.MakeEnPassant(board.GetPieceAt(from) as Pawn, lastMovement, board);
            stateManager.SaveState(granHoqueWhite, grandHoqueBlack, shortHoqueWhite, shortHoqueBlack, lastMovement, board.DuplicateBoard(), isWhiteTurn);
            return;
        }
        lastMovement = board.MovePiece(from, to);
        isWhiteTurn = !isWhiteTurn;
        stateManager.SaveState(granHoqueWhite, grandHoqueBlack, shortHoqueWhite, shortHoqueBlack, lastMovement, board.DuplicateBoard(), isWhiteTurn);
        board.PrintBoard();
    }

    public void OnPieceSelected(Coordinates pieceLocation)
    {
        Piece piece = board.GetPieceAt(pieceLocation);

        List<Coordinates> moves = new List<Coordinates>();
        foreach (Coordinates move in moveValidator.ValidateMoves(piece, piece.GenerateMoves(board), stateManager.LoadLastState()))
        {
            moves.Add(move);
        }
        if(piece is Pawn)
        {
            Coordinates[] enPassant = new Coordinates[] { specialMoveManager.GetEnPassantCoordinate(lastMovement, piece as Pawn) };
            foreach (Coordinates move in moveValidator.ValidateMoves(piece, enPassant, stateManager.LoadLastState()))
            {
                moves.Add(move);
            }
        }
        Debug.Log("This is your moves");
        foreach (Coordinates move in moves)
        {
            move.PrintCoordinates();
        }



        //Enviar as coordenadas para o board UI marcar as posições que o usuário pode clicar no board.

    }

    public void OnPieceMoved(Coordinates lastPieceLocation, Coordinates newPieceLocation)
    {
        MovePiece(lastPieceLocation, newPieceLocation);
        lastMovement.PrintMovement();
        stateManager.PrintState();
    }

    public Move GetLastMove()
    {
        return lastMovement;
    }

    public void TestButtonPressed()
    {
        Coordinates coordinates = new Coordinates(int.Parse(inputField.text[0].ToString()), int.Parse(inputField.text[1].ToString()));
        OnPieceSelected(coordinates);
    }

    public void TestButtonPressed2()
    {
        Coordinates coordinates = new Coordinates(int.Parse(inputField.text[0].ToString()), int.Parse(inputField.text[1].ToString()));
        Coordinates coordinates2 = new Coordinates(int.Parse(inputField2.text[0].ToString()), int.Parse(inputField2.text[1].ToString()));
        OnPieceMoved(coordinates, coordinates2);
    }

}

public struct Move
{
    public Piece pieceMoved;
    public Piece pieceDestroyed;
    public Coordinates lastCoordinate;
    public Coordinates newCoordinate;

    public Move(Piece piece, Piece destroyed, Coordinates lastCoordinates, Coordinates newCoordinates)
    {
        this.pieceMoved = piece;
        this.lastCoordinate = lastCoordinates; 
        this.newCoordinate = newCoordinates;
        this.pieceDestroyed = destroyed;
    }

    public void PrintMovement()
    {
        Debug.Log("Piece moved: " + pieceMoved.GetType().Name);
        if(pieceDestroyed != null)
        {
            Debug.Log($"Piece destroyed {pieceDestroyed.GetType().Name}");
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
}