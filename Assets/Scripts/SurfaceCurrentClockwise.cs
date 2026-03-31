using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class SurfaceCurrentClockwise : MonoBehaviour
{
    [SerializeField] private float currentStrength = 0.2f;
    [SerializeField] private float wallBuffer = 0.08f;
    [SerializeField] private float wallPushStrength = 0.4f;
    [SerializeField] private float outwardSlowdown = 1.0f;
    [SerializeField] private Transform currentCenter;

    private readonly List<Rigidbody> bodiesInWater = new();
    private BoxCollider waterTrigger;

    private void Awake()
    {
        waterTrigger = GetComponent<BoxCollider>();

        if (!waterTrigger.isTrigger)
        {
            Debug.LogWarning($"{name}: BoxCollider should be a trigger.", this);
        }

        if (currentCenter == null)
        {
            currentCenter = transform;
        }
    }

    private void FixedUpdate()
    {
        if (bodiesInWater.Count == 0)
            return;

        Vector3 up = transform.up;
        Vector3 center = currentCenter.position;
        Bounds bounds = waterTrigger.bounds;

        for (int i = bodiesInWater.Count - 1; i >= 0; i--)
        {
            Rigidbody rb = bodiesInWater[i];

            if (rb == null)
            {
                bodiesInWater.RemoveAt(i);
                continue;
            }

            Vector3 objectPos = rb.transform.position;
            Vector3 toObject = objectPos - center;
            Vector3 flatToObject = Vector3.ProjectOnPlane(toObject, up);

            if (flatToObject.sqrMagnitude < 0.0001f)
                continue;

            Vector3 outward = flatToObject.normalized;
            Vector3 clockwise = Vector3.Cross(up, outward).normalized;

            Vector3 totalForce = clockwise * currentStrength;

            Vector3 flatVelocity = Vector3.ProjectOnPlane(rb.linearVelocity, up);

            // Distances to each inside wall of the trigger bounds.
            float distToMinX = objectPos.x - bounds.min.x;
            float distToMaxX = bounds.max.x - objectPos.x;
            float distToMinZ = objectPos.z - bounds.min.z;
            float distToMaxZ = bounds.max.z - objectPos.z;

            // Push away from left wall
            if (distToMinX < wallBuffer)
            {
                float amount = 1f - Mathf.Clamp01(distToMinX / wallBuffer);
                totalForce += Vector3.right * (wallPushStrength * amount);

                float outwardSpeed = Vector3.Dot(flatVelocity, Vector3.left);
                if (outwardSpeed > 0f)
                    totalForce += Vector3.right * (outwardSpeed * outwardSlowdown * amount);
            }

            // Push away from right wall
            if (distToMaxX < wallBuffer)
            {
                float amount = 1f - Mathf.Clamp01(distToMaxX / wallBuffer);
                totalForce += Vector3.left * (wallPushStrength * amount);

                float outwardSpeed = Vector3.Dot(flatVelocity, Vector3.right);
                if (outwardSpeed > 0f)
                    totalForce += Vector3.left * (outwardSpeed * outwardSlowdown * amount);
            }

            // Push away from bottom/back wall
            if (distToMinZ < wallBuffer)
            {
                float amount = 1f - Mathf.Clamp01(distToMinZ / wallBuffer);
                totalForce += Vector3.forward * (wallPushStrength * amount);

                float outwardSpeed = Vector3.Dot(flatVelocity, Vector3.back);
                if (outwardSpeed > 0f)
                    totalForce += Vector3.forward * (outwardSpeed * outwardSlowdown * amount);
            }

            // Push away from top/front wall
            if (distToMaxZ < wallBuffer)
            {
                float amount = 1f - Mathf.Clamp01(distToMaxZ / wallBuffer);
                totalForce += Vector3.back * (wallPushStrength * amount);

                float outwardSpeed = Vector3.Dot(flatVelocity, Vector3.forward);
                if (outwardSpeed > 0f)
                    totalForce += Vector3.back * (outwardSpeed * outwardSlowdown * amount);
            }

            rb.AddForce(totalForce, ForceMode.Acceleration);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        AddBody(other);
    }

    private void OnTriggerStay(Collider other)
    {
        AddBody(other);
    }

    private void OnTriggerExit(Collider other)
    {
        Rigidbody rb = other.attachedRigidbody;

        if (rb == null)
            return;

        bodiesInWater.Remove(rb);
    }

    private void AddBody(Collider other)
    {
        Rigidbody rb = other.attachedRigidbody;

        if (rb == null)
            return;

        if (!bodiesInWater.Contains(rb))
            bodiesInWater.Add(rb);
    }
}