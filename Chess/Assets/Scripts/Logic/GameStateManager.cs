using UnityEngine;

public class GameStateManager
{

    private GameState lastState;

    public void SaveState(Move lastMovement, Board board, bool isWhiteTurn)
    {
        lastState = new GameState(lastMovement, board, isWhiteTurn);
    }

    public GameState LoadLastState()
    {
        return lastState;
    }

    public void PrintState()
    {
        Debug.Log($"Turn {lastState.isWhiteTurn}");
        lastState.lastMovement.PrintMovement();
    }
    
}

public struct GameState
{
    public bool isWhiteTurn;

    public Move lastMovement;

    public Board board;

    public GameState(Move lastMovement, Board board, bool isWhiteTurn)
    {
        this.lastMovement = lastMovement;
        this.board = board;
        this.isWhiteTurn = isWhiteTurn;
    }
}