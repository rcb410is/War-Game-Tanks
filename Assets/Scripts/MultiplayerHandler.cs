using UnityEngine;
using UnityEngine.UI;

public class MultiplayerHandler : MonoBehaviour
{

    [SerializeField] Text playerTurnText;
    [SerializeField] Text player1ScoreText;
    [SerializeField] Text player2ScoreText;
    [SerializeField] Text actionsRemainingText;
    [SerializeField] Text playerWinsText;
    static string currentPlayerTag;
    public static int actionsRemaining = 3;
    int turnCount;
    static int player1Score;
    static int player2Score;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentPlayerTag = "Player1";
        playerTurnText.text = "Player 1's Turn";
    }

    public string GetCurrentPlayerTag() { return currentPlayerTag; }

    public static int GetActionsRemaining() { return actionsRemaining; }

    public static void UsedAction(string typeOfAction) 
    { 
        if (typeOfAction == "Movement")
        {
            actionsRemaining -= 1;
        }
        else if (typeOfAction == "Shoot")
        {
            actionsRemaining -= 2;
        }
        else if (typeOfAction == "Deploy")
        {
            actionsRemaining -= 3;
        }

    }

    public static void AddPlayerScore(string player)
    {
        if (player == "Player1")
        {
            player2Score++;
        }

        if (player == "Player2")
        {
            player1Score++;
        }
        
        if (player1Score >= 5)
        {
            Debug.Log("Player 1 Wins!");
        }

        if (player2Score >= 5)
        {
            Debug.Log("Player 2 Wins!");
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (currentPlayerTag == "Player1")
        {
            if (actionsRemaining <= 0)
            {
                playerTurnText.text = "Player 2's Turn";
                currentPlayerTag = "Player2";
                actionsRemaining = 3;
            }
        }
        else if (currentPlayerTag == "Player2")
        {
            if (actionsRemaining <= 0)
            {
                playerTurnText.text = "Player 1's Turn";
                currentPlayerTag = "Player1";
                actionsRemaining = 3;
            }
        }
        player1ScoreText.text = $"{player1Score}";
        player2ScoreText.text = $"{player2Score}";
        actionsRemainingText.text = $"Actions Remaining: {actionsRemaining}";

        if (player1Score >= 3)
        {
            playerWinsText.text = "Player 1 Wins!";
        }

        if (player2Score >= 3)
        {
            playerWinsText.text = "Player 2 Wins!";
        }
    }

}
