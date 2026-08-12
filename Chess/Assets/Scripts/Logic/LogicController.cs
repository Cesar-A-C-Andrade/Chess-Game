using System;
using System.Collections.Generic;
using UnityEngine;

public class LogicController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

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
        board.TestScenario();
        board.PrintBoard();
        lastMovement = new Move(null, new Coordinates(-1,-1), new Coordinates(-1, -1));
        stateManager.SaveState(granHoqueWhite, grandHoqueBlack, shortHoqueWhite, shortHoqueBlack, lastMovement, board.DuplicateBoard(), isWhiteTurn);
        Debug.Log(xequeManager.IsXequeMate(stateManager.LoadLastState()));
    }

    void MovePiece(Coordinates from, Coordinates to)
    {
        board.MovePiece(from, to);
        lastMovement = new Move(board.GetPieceAt(to), from, to);
        stateManager.SaveState(granHoqueWhite, grandHoqueBlack, shortHoqueWhite, shortHoqueBlack, lastMovement, board.DuplicateBoard(), isWhiteTurn);
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
            moveValidator.ValidateMoves(piece, new Coordinates[] { specialMoveManager.GetEnPassantCoordinate(lastMovement, piece as Pawn) }, stateManager.LoadLastState() );
        }


        //Enviar as coordenadas para o board UI marcar as posições que o usuário pode clicar no board.

    }

    public void OnPieceMoved(Coordinates lastPieceLocation, Coordinates newPieceLocation)
    {
        MovePiece(lastPieceLocation, newPieceLocation);
    }

}

public struct Move
{
    public Piece piece;
    public Coordinates lastCoordinate;
    public Coordinates newCoordinate;

    public Move(Piece piece, Coordinates lastCoordinates, Coordinates newCoordinates)
    {
        this.piece = piece;
        this.lastCoordinate = lastCoordinates; 
        this.newCoordinate = newCoordinates;
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
}