using UnityEngine;

public class GameStateManager
{

    private GameState lastState;

    public void SaveState(bool granHoqueWhite, bool grandHoqueBlack, bool shortHoqueWhite, bool shortHoqueBlack, Move lastMovement, Board board, bool isWhiteTurn)
    {
        lastState = new GameState(granHoqueWhite, grandHoqueBlack, shortHoqueWhite, shortHoqueBlack, lastMovement, board, isWhiteTurn);
    }

    public GameState LoadLastState()
    {
        return lastState;
    }
    
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