using System;
using UnityEngine;

public class Events : MonoBehaviour
{
    public static Events current;

    private void Awake()
    {
        if (current == null)
        {
            current = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public event Action<Ball> OnBallClicked;

    public void BallClicked(Ball ball)
    {
        OnBallClicked?.Invoke(ball);
    }
}