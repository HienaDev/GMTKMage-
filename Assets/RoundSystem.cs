using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundSystem : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private Sprite emptyRound;
    [SerializeField] private Sprite roundWon;
    [SerializeField] private Image[] images;

    [SerializeField] private GameObject gameOverPlayerWins;
    [SerializeField] private GameObject gameOverArenaWins;

    [SerializeField] private string winner;

    private int roundsWon = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < roundsWon; i++)
        {
            images[i].sprite = roundWon;
        }
        for (int i = roundsWon; i < images.Length; i++)
        {
            images[i].sprite = emptyRound;
        }

        if (roundsWon == 2)
        { 
            if (winner == "Knight")
            {
                gameOverPlayerWins.SetActive(true);
            }

            if (winner == "Arena")
            {
                gameOverArenaWins.SetActive(true);
            }
        }
    }

    public void WinRound()
    {
        roundsWon += 1;
    }
}
