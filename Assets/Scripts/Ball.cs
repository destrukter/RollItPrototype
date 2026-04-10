using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private BallData data;

    private bool inHand = false;

    public BallData Data => data;
    public bool InHand => inHand;

    public void Initialize()
    {
        data.name = "Standard";
        data.size = 1;
        data.weight = 1;
        data.points = 1;
    }

    private void OnMouseDown()
    {
        if (Events.current != null)
        {
            Events.current.BallClicked(this);
        }
    }

    public void ToggleHand()
    {
        inHand = !inHand;
    }

    public int GetPoints()
    {
        return data.points;
    }

    public float GetWeight()
    {
        return data.weight;
    }

    public float GetSize()
    {
        return data.size;
    }
}
