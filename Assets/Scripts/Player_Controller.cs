using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class Player_Controller : MonoBehaviour
{
    [SerializeField] GameObject ballPrefab;

    [SerializeField] Transform drawPileParent;
    [SerializeField] Transform handPileParent;
    [SerializeField] Transform playPileParent;
    [SerializeField] Transform discardPileParent;

    //List<Joker> jokers = new List<Joker>();

    List<Ball> drawPile = new List<Ball>();
    List<Ball> handPile = new List<Ball>();
    List<Ball> playPile = new List<Ball>();
    List<Ball> discardPile = new List<Ball>();

    Roulette_Controller roulette;

    int num_draw = 3;
    int hand_size = 5;
    int play_size = 3;

    enum PlayState { drawBalls, selectBalls, playBalls, postRound }
    PlayState play_state = PlayState.drawBalls;

    private void Start()
    {
        if (Events.current != null)
        {
            Events.current.OnBallClicked += OnBallClicked;
        }
        GenerateStartingDeck(10);
    }

    private void OnDestroy()
    {
        if (Events.current != null)
        {
            Events.current.OnBallClicked -= OnBallClicked;
        }
    }

    void DrawBalls()
    {
        if (play_state == PlayState.drawBalls)
        {
            System.Random rng = new System.Random();

            List<Ball> drawnBalls = drawPile
                .OrderBy(x => rng.Next())
                .Take(num_draw)
                .ToList();

            foreach (var ball in drawnBalls)
            {
                handPile.Add(ball);
                drawPile.Remove(ball);
            }
        }
    }

    private void OnBallClicked(Ball ball)
    {
        if (play_state != PlayState.selectBalls)
            return;

        if (handPile.Contains(ball))
        {
            if (playPile.Count >= play_size)
                return;

            handPile.Remove(ball);
            playPile.Add(ball);
            ball.ToggleHand();
        }
        else if (playPile.Contains(ball))
        {
            playPile.Remove(ball);
            handPile.Add(ball);
            ball.ToggleHand();
        }
    }

    void GenerateStartingDeck(int amount)
    {
        drawPile.Clear();
        handPile.Clear();
        playPile.Clear();
        discardPile.Clear();

        for (int i = 0; i < amount; i++)
        {
            GameObject obj = Instantiate(ballPrefab, drawPileParent);
            Ball ball = obj.GetComponent<Ball>();

            ball.Initialize();

            drawPile.Add(ball);
        }
    }

    void SelectBalls()
    {
        if (play_state == PlayState.selectBalls)
        {

        }
    }

    void PlayBalls()
    {
        if (play_state == PlayState.playBalls)
        {
               
        }
    }

    void PostRound()
    {
        if (play_state == PlayState.postRound)
        {

        }
    }
}