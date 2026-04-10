using UnityEngine;
using System.Collections;

public class Roulette_Controller : MonoBehaviour
{
    [SerializeField] float spinDurationSeconds = 2.8f;
    [SerializeField] float spinForceMin = 260f;
    [SerializeField] float spinForceMax = 420f;

    Coroutine spinRoutine;
    bool isSubscribedToPlay;

    private void OnEnable()
    {
        TrySubscribeToEvents();
    }

    private void Start()
    {
        // Fallback subscription when Events.current is initialized after this component enables.
        TrySubscribeToEvents();
    }

    private void OnDisable()
    {
        UnsubscribeFromEvents();
    }

    private void OnDestroy()
    {
        UnsubscribeFromEvents();
    }

    private void TrySubscribeToEvents()
    {
        if (isSubscribedToPlay || Events.current == null)
            return;

        Events.current.OnPlayTriggered += OnPlayTriggered;
        isSubscribedToPlay = true;
    }

    private void UnsubscribeFromEvents()
    {
        if (!isSubscribedToPlay || Events.current == null)
            return;

        Events.current.OnPlayTriggered -= OnPlayTriggered;
        isSubscribedToPlay = false;
    }

    private void OnPlayTriggered()
    {
        if (spinRoutine != null)
        {
            StopCoroutine(spinRoutine);
        }

        float minForce = Mathf.Min(spinForceMin, spinForceMax);
        float maxForce = Mathf.Max(spinForceMin, spinForceMax);
        float spinForce = Random.Range(minForce, maxForce);
        spinRoutine = StartCoroutine(SpinRoutine(spinForce));
    }

    private IEnumerator SpinRoutine(float spinForce)
    {
        float duration = Mathf.Max(0.1f, spinDurationSeconds);
        float randomDirection = Random.value > 0.5f ? 1f : -1f;
        float startingSpeed = Mathf.Abs(spinForce) * randomDirection;

        float elapsed = 0f;
        float accumulatedDegrees = 0f;
        Quaternion startRotation = transform.localRotation;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float currentSpeed = Mathf.Lerp(startingSpeed, 0f, t);
            accumulatedDegrees += currentSpeed * Time.deltaTime;
            transform.localRotation = startRotation * Quaternion.Euler(0f, accumulatedDegrees, 0f);
            yield return null;
        }

        spinRoutine = null;
    }
}
