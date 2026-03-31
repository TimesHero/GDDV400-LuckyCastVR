using UnityEditor.Callbacks;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DuckCenterOfMass : MonoBehaviour
{
    [SerializeField] private Transform centerOfMassMarker;
    private Rigidbody rb;
    private void Awake()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        ApplyCenterOfMass();
    }

    public void ApplyCenterOfMass()
    {
        if (centerOfMassMarker == null)
        {
            return;
        }

        rb.centerOfMass = centerOfMassMarker.localPosition;
    }
}
