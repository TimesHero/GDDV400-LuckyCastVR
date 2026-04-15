using UnityEngine;

public class BucketScoreZone : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Bucket triggered by: {other.name}");

        DuckHookPoint duck = other.GetComponentInParent<DuckHookPoint>();

        if (duck == null)
        {
            Debug.Log("No DuckHookPoint found on this object or its parents.");
            return;
        }

        Debug.Log($"Duck found. Value = {duck.duckValue}, wasHooked = {duck.wasHooked}, hasScored = {duck.hasScored}");

        if (gameManager == null)
        {
            Debug.Log("GameManager is missing from bucket.");
            return;
        }

        gameManager.ScoreDuck(duck.duckValue, duck);
    }
}