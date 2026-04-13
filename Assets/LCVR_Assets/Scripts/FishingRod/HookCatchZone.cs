using UnityEngine;

public class HookCatchZone : MonoBehaviour
{
    [SerializeField] private Transform duckHangPose;

    private void Reset()
    {
        //makes sure collider is trigger
        Collider col = GetComponent<Collider>();
        if (col != null)
            col.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Check for duck
        DuckHookPoint hookPoint = other.GetComponent<DuckHookPoint>();
        if (hookPoint == null)
            return;

        // Tell duck to hang its self
        hookPoint.TryHook(duckHangPose);
    }
}