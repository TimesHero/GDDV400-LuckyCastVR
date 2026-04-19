using UnityEngine;

public class HookCatchZone : MonoBehaviour
{
    [SerializeField] private Transform duckHangPose;
    private DuckHookPoint currentDuck;

    private void Reset()
    {
        //makes sure collider is trigger
        Collider col = GetComponent<Collider>();
        if (col != null)
            col.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {

        if (currentDuck != null)
            return;

        //Check for duck
        DuckHookPoint hookPoint = other.GetComponent<DuckHookPoint>();
        if (hookPoint == null)
            return;
        
        bool hookedSuccessfully = hookPoint.TryHook(duckHangPose);

        if (hookedSuccessfully)
        {
            currentDuck = hookPoint;
            hookPoint.SetHookCatchZone(this);
        }
    }

    public void ClearCurrentDuck(DuckHookPoint duck)
    {
        if (currentDuck == duck)
            currentDuck = null;
    }
}