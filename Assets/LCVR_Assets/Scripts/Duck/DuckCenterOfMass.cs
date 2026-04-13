using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DuckCenterOfMass : MonoBehaviour
{
    [SerializeField] private Transform centerOfMassMarker;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        ApplyCenterOfMass();
    }

    public void ApplyCenterOfMass()
    {
        if (centerOfMassMarker == null)
        {
            Debug.LogWarning("No center of mass marker assigned.", this);
            return;
        }

        rb.centerOfMass = centerOfMassMarker.localPosition;
    }
}