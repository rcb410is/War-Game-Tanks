using UnityEngine;
using UnityEngine.UI;

public class MultiplayerHandler : MonoBehaviour
{

    [SerializeField] Text playerTurnText;
    [SerializeField] Text player1ScoreText;
    [SerializeField] Text player2ScoreText;
    static string currentPlayerTag;
    public static int actionsRemaining = 1;
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

    public static void UsedAction() { actionsRemaining--; Debug.Log("Used an action"); }

    public static void AddPlayerScore(string player)
    {
        Debug.Log(player);
        if (player == "Player1")
        {
            player2Score++;
            Debug.Log(player2Score);
        }

        if (player == "Player2")
        {
            player1Score++;
            Debug.Log(player1Score);
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
                actionsRemaining = 1;
            }
        }
        else if (currentPlayerTag == "Player2")
        {
            if (actionsRemaining <= 0)
            {
                playerTurnText.text = "Player 1's Turn";
                currentPlayerTag = "Player1";
                actionsRemaining = 1;
            }
        }
        player1ScoreText.text = $"{player1Score}";
        player2ScoreText.text = $"{player2Score}";

    }

}
