using UnityEngine;
using System.Collections;

public class Roulette_Controller : MonoBehaviour
{
    [SerializeField] float spinDurationSeconds = 70f;
    [SerializeField] float spinForceMin = 5000f;
    [SerializeField] float spinForceMax = 15000f;

    [SerializeField] GameObject roulettePart1;
    [SerializeField] GameObject roulettePart2;
    //Rigidbody rb1;
    //Rigidbody rb2;

    Coroutine spinRoutine;
    Coroutine subscribeRoutine;
    bool isSubscribedToPlayEvent;

    private void OnEnable()
    {
        if (!TrySubscribeToPlayEvent())
        {
            subscribeRoutine = StartCoroutine(WaitForEventsAndSubscribe());
        }
        //rb1 = roulettePart1.GetComponent<Rigidbody>();
        //rb2 = roulettePart2.GetComponent<Rigidbody>();
    }

    private void OnDisable()
    {
        if (subscribeRoutine != null)
        {
            StopCoroutine(subscribeRoutine);
            subscribeRoutine = null;
        }

        TryUnsubscribeFromPlayEvent();
    }

    private IEnumerator WaitForEventsAndSubscribe()
    {
        while (!isSubscribedToPlayEvent)
        {
            if (TrySubscribeToPlayEvent())
            {
                subscribeRoutine = null;
                yield break;
            }

            yield return null;
        }
    }

    private bool TrySubscribeToPlayEvent()
    {
        if (isSubscribedToPlayEvent || Events.current == null)
            return false;

        Events.current.OnPlayTriggered += OnPlayTriggered;
        isSubscribedToPlayEvent = true;
        return true;
    }

    private void TryUnsubscribeFromPlayEvent()
    {
        if (!isSubscribedToPlayEvent)
            return;

        if (Events.current != null)
        {
            Events.current.OnPlayTriggered -= OnPlayTriggered;
        }

        isSubscribedToPlayEvent = false;
    }

    private void OnPlayTriggered()
    {
        if (roulettePart1 == null || roulettePart2 == null)
        {
            Debug.LogWarning("Roulette parts are not assigned on Roulette_Controller.");
            return;
        }
        if (spinRoutine != null)
        {
            StopCoroutine(spinRoutine);
        }
        Debug.Log("Roulette spin triggered with force range: " + spinForceMin + " to " + spinForceMax);

        float minForce = Mathf.Min(spinForceMin, spinForceMax);
        float maxForce = Mathf.Max(spinForceMin, spinForceMax);
        float spinForce = Random.Range(minForce, maxForce);
        spinRoutine = StartCoroutine(SpinRoutine(spinForce));
    }

    private IEnumerator SpinRoutine(float spinForce)
    {
        float duration = Mathf.Max(4f, spinDurationSeconds);

        float startingSpeed = Mathf.Abs(spinForce);

        float elapsed = 0f;
        float accumulatedDegrees = 0f;
        Quaternion startRotation1 = roulettePart1.transform.localRotation;
        Quaternion startRotation2 = roulettePart2.transform.localRotation;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float currentSpeed = Mathf.Lerp(startingSpeed, 0f, t);
            accumulatedDegrees += currentSpeed * Time.deltaTime;
            roulettePart1.transform.localRotation = startRotation1 * Quaternion.Euler(0f, 0f, accumulatedDegrees);
            //roulettePart2.transform.localRotation = startRotation2 * Quaternion.Euler(0f, 0f, accumulatedDegrees);
            yield return null;
        }

        spinRoutine = null;
    }
}
