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

    void Start()
    {
        board = new Board();
        board.PrintBoard();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void MovePiece(Coordinates from, Coordinates to)
    {
        board.MovePiece(from, to);
    }

}

public struct Move
{
    public Piece piece;
    public Coordinates lastCoordinate;
    public Coordinates newCoordinate;
}

public struct GameState
{
    public bool granHoqueWhite;
    public bool grandHoqueBlack;
    public bool shortHoqueWhite;
    public bool shortHoqueBlack;
    public bool isWhiteTurn;

    public Move lastMovement;

    public Board board;

    public GameState(bool granHoqueWhite, bool grandHoqueBlack, bool shortHoqueWhite, bool shortHoqueBlack, Move lastMovement, Board board, bool isWhiteTurn)
    {
        this.granHoqueWhite = granHoqueWhite;
        this.grandHoqueBlack = grandHoqueBlack;
        this.shortHoqueWhite = shortHoqueWhite;
        this.shortHoqueBlack = shortHoqueBlack;
        this.lastMovement = lastMovement;
        this.board = board;
        this.isWhiteTurn = isWhiteTurn;
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