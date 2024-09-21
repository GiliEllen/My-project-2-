using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    public Button[] buttons;  
    public TMP_Text resultText; 
    public Button restartButton;  
    public DifficultyController difficultyController;
    
    public Sprite xSprite; 
    public Sprite oSprite; 
    
    private string player = "X";
    private string computer = "O";
    private string[,] board = new string[3, 3]; 
    private bool gameActive = false;
    private int turnCounter = 0;
    private bool firstMoveMade  = false;

    private void Start()
    {
        Debug.Log("start");
        restartButton.onClick.AddListener(RestartGame);
        InitializeBoard();
        EnableDifficultyButtons();
    }

public void PlayerMove(int index)
{
    Debug.Log("player move");
    if (!gameActive)
    {
        Debug.Log("game is not active");
        gameActive = true;
        firstMoveMade = true; 
        DisableDifficultyButtons(); 
    }

    Debug.Log("continue");

    int row = index / 3;
    int col = index % 3;

    if (board[row, col] == "")
    {
        Debug.Log("entered if");
        board[row, col] = player;
        buttons[index].GetComponent<Image>().sprite = xSprite;
        buttons[index].interactable = false;

        if (CheckForWinner())
        {
            EndGame(player + " Wins!");
        }
        else
        {
            DisableButtons(); 
            StartCoroutine(ComputerMove());
        }
    }
}

private IEnumerator ComputerMove()
    {
        Debug.Log("comp move");
        yield return new WaitForSeconds(1); 

        if (!gameActive)
        {
            yield break; 
        }

        DifficultyController.DifficultyLevel difficulty = difficultyController.selectedDifficulty;

        if (difficulty == DifficultyController.DifficultyLevel.Easy)
        {
            ExecuteRandomMove();
        }
        else if (difficulty == DifficultyController.DifficultyLevel.Medium)
        {
            if (turnCounter % 2 == 0)
                ExecuteMinimaxMove();
            else
                ExecuteRandomMove();

            turnCounter++; 
        }
        else if (difficulty == DifficultyController.DifficultyLevel.Hard)
        {
            ExecuteMinimaxMove(); 
        }

        if (CheckForWinner())
        {
            EndGame(computer + " Wins!");
        }
        EnableButtons();
    }

    private void ExecuteRandomMove()
    {
        Debug.Log("excute random move");
        bool moveMade = false;
        while (!moveMade)
        {
            int randomIndex = Random.Range(0, 9);
            int row = randomIndex / 3;
            int col = randomIndex % 3;

            if (board[row, col] == "")
            {
                board[row, col] = computer;
                buttons[randomIndex].GetComponent<Image>().sprite = oSprite;
                buttons[randomIndex].interactable = false;
                moveMade = true;
            }
        }
    }

    private void ExecuteMinimaxMove()
    {
        Debug.Log("excute minimax move");
        int bestMove = GetBestMove();
        int row = bestMove / 3;
        int col = bestMove % 3;

        board[row, col] = computer;
        buttons[bestMove].GetComponent<Image>().sprite = oSprite;
        buttons[bestMove].interactable = false;
    }

    private int GetBestMove()
    {
        Debug.Log("get best move");
        int bestScore = int.MinValue;
        int bestMove = -1;

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (board[i, j] == "")
                {
                    board[i, j] = computer;
                    int score = Minimax(board, false);
                    board[i, j] = "";

                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestMove = i * 3 + j;
                    }
                }
            }
        }

        return bestMove;
    }

    private int Minimax(string[,] board, bool isMaximizing)
    {
        Debug.Log("minimaxing");
        if (CheckForWinner()) return isMaximizing ? -1 : 1;
        if (IsBoardFull()) return 0;

        if (isMaximizing)
        {
            int bestScore = int.MinValue;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board[i, j] == "")
                    {
                        board[i, j] = computer;
                        int score = Minimax(board, false);
                        board[i, j] = "";
                        bestScore = Mathf.Max(score, bestScore);
                    }
                }
            }
            return bestScore;
        }
        else
        {
            int bestScore = int.MaxValue;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board[i, j] == "")
                    {
                        board[i, j] = player;
                        int score = Minimax(board, true);
                        board[i, j] = "";
                        bestScore = Mathf.Min(score, bestScore);
                    }
                }
            }
            return bestScore;
        }
    }

    private void DisableButtons()
    {
        Debug.Log("disable buttons");
        foreach (var button in buttons)
        {
            button.interactable = false;
        }
    }

    private void EnableButtons()
    {
        Debug.Log("enable buttons");
        foreach (var button in buttons)
        {
            if (button.GetComponent<Image>().sprite == null)
            {
                button.interactable = true;
            }
        }
    }

    private bool IsBoardFull()
    {
        Debug.Log("is board full");
        foreach (var cell in board)
        {
            if (cell == "") return false;
        }
        return true;
    }
private bool CheckForWinner()
{
    for (int i = 0; i < 3; i++)
    {
        if (board[i, 0] == board[i, 1] && board[i, 1] == board[i, 2] && board[i, 0] != "")
            return true;
        if (board[0, i] == board[1, i] && board[1, i] == board[2, i] && board[0, i] != "")
            return true;
    }

    if (board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2] && board[0, 0] != "")
        return true;

    if (board[0, 2] == board[1, 1] && board[1, 1] == board[2, 0] && board[0, 2] != "")
        return true;

    if (IsBoardFull())
    {
        EndGame("It's a Tie!");
        return false; 
    }

    return false; 
}

 private void EndGame(string message)
    {
        Debug.Log("game ended");
        resultText.text = message;
        gameActive = false;
        foreach (var button in buttons)
        {
            button.interactable = false; 
        }
        restartButton.gameObject.SetActive(true);
        EnableDifficultyButtons(); 
        firstMoveMade = false; 
    }

private void RestartGame()
{
    Debug.Log("game restarted");
    InitializeBoard();
    gameActive = false; 
    resultText.text = "";
    restartButton.gameObject.SetActive(false);
    EnableDifficultyButtons(); 
}

private void InitializeBoard()
{
    Debug.Log("Initialize board");
    board = new string[3, 3]; 

    for (int i = 0; i < 3; i++)
    {
        for (int j = 0; j < 3; j++)
        {
            board[i, j] = ""; 
        }
    }

    foreach (var button in buttons)
    {
        button.GetComponent<Image>().sprite = null;
        button.interactable = true;
    }

    gameActive = false; 
}
    private void EnableDifficultyButtons()
    {
        Debug.Log("difficulty Buttons Enabled");
        difficultyController.easyButton.interactable = true;
        difficultyController.mediumButton.interactable = true;
        difficultyController.hardButton.interactable = true;
    }

    private void DisableDifficultyButtons()
    {
        Debug.Log("difficulty Buttons Disabled");
        difficultyController.easyButton.interactable = false;
        difficultyController.mediumButton.interactable = false;
        difficultyController.hardButton.interactable = false;
    }
}
