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


    List<List<Piece>> board = new List<List<Piece>>(8);


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public struct Move
{
    public Piece piece;
    public int previousIndex;
    public int newIndex;
}

public struct State
{
    bool granHoqueWhite;
    bool grandHoqueBlack;
    bool shortHoqueWhite;
    bool shortHoqueBlack;

    private Move lastMovement;

    List<List<Piece>> board;

    public State(bool granHoqueWhite, bool grandHoqueBlack, bool shortHoqueWhite, bool shortHoqueBlack, Move lastMovement, List<List<Piece>> board)
    {
        this.granHoqueWhite = granHoqueWhite;
        this.grandHoqueBlack = grandHoqueBlack;
        this.shortHoqueWhite = shortHoqueWhite;
        this.shortHoqueBlack = shortHoqueBlack;
        this.lastMovement = lastMovement;
        this.board = board;
    }
}
