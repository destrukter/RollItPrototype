using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class Tile : MonoBehaviour
{
    public enum TileColor
    {
        Red,
        Black,
        Green
    }

    [SerializeField] int value;
    [SerializeField] float angle;
    [SerializeField] TileColor tileColor;
    [SerializeField] TMP_Text valueText;

    static int redPoints;
    static int bluePoints;
    static int greenPoints;

    readonly HashSet<Ball> ballsInSlot = new HashSet<Ball>();
    bool subscribedToGameEnd;

    public int Value => value;
    public float Angle => angle;
    public TileColor Color => tileColor;

    private void Awake()
    {
        if (valueText == null)
        {
            valueText = GetComponentInChildren<TMP_Text>(true);
        }
    }

    private void OnEnable()
    {
        TrySubscribe();
    }

    private void Start()
    {
        TrySubscribe();
    }

    private void OnDisable()
    {
        if (!subscribedToGameEnd || Events.current == null)
            return;

        Events.current.OnGameEndTriggered -= OnGameEndTriggered;
        subscribedToGameEnd = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        Ball ball = other.GetComponent<Ball>();
        if (ball != null)
        {
            ballsInSlot.Add(ball);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Ball ball = other.GetComponent<Ball>();
        if (ball != null)
        {
            ballsInSlot.Remove(ball);
        }
    }

    public void Configure(int assignedValue, float assignedAngle, TileColor assignedColor)
    {
        value = assignedValue;
        angle = assignedAngle;
        tileColor = assignedColor;

        if (valueText == null)
        {
            valueText = GetComponentInChildren<TMP_Text>(true);
        }

        if (valueText != null)
        {
            valueText.text = value.ToString();
        }
    }

    private void TrySubscribe()
    {
        if (subscribedToGameEnd || Events.current == null)
            return;

        Events.current.OnGameEndTriggered += OnGameEndTriggered;
        subscribedToGameEnd = true;
    }

    private void OnGameEndTriggered()
    {
        int ballsHere = ballsInSlot.Count;
        if (ballsHere <= 0)
            return;

        switch (tileColor)
        {
            case TileColor.Red:
                redPoints += ballsHere;
                break;
            case TileColor.Green:
                greenPoints += ballsHere;
                break;
            default:
                bluePoints += ballsHere; // black slots are counted as blue points
                break;
        }

        Debug.Log($"Slot points => Red: {redPoints}, Blue: {bluePoints}, Green: {greenPoints}");
    }
}
