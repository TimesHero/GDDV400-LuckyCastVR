using UnityEngine;
using Fusion.Fluid;

[RequireComponent(typeof(Rigidbody))]
public class DuckPointsUp : MonoBehaviour
{
    [SerializeField] private float pointUpStrength = 0.35f;
    [SerializeField] private float turnSlowdown = 0.08f;
    [SerializeField] private bool onlyWhileInWater = true;

    private Rigidbody rb;
    private int waterCount = 0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (onlyWhileInWater && waterCount <= 0)
            return;

        Vector3 duckUp = transform.up;
        Vector3 worldUp = Vector3.up;

        // This finds the axis needed to tilt the duck back upright.
        // It does not try to control yaw/spinning around the vertical axis.
        Vector3 tiltAxis = Vector3.Cross(duckUp, worldUp);
        float tiltAmount = tiltAxis.magnitude;

        if (tiltAmount < 0.0001f)
            return;

        // Gentle restoring push back toward upright.
        Vector3 pointUpForce = tiltAxis.normalized * (tiltAmount * pointUpStrength);

        // Only slow down tipping motion, not spinning around vertical.
        Vector3 tiltSpin = Vector3.ProjectOnPlane(rb.angularVelocity, worldUp);
        Vector3 slowTiltForce = -tiltSpin * turnSlowdown;

        rb.AddTorque(pointUpForce + slowTiltForce, ForceMode.Acceleration);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Fluid>() != null)
            waterCount++;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Fluid>() != null)
            waterCount = Mathf.Max(0, waterCount - 1);
    }
}