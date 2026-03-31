using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DuckWallBounce : MonoBehaviour
{
    [SerializeField] private float wallPushStrength = 0.15f;
    [SerializeField] private float wallSlowdown = 1.5f;
    [SerializeField] private bool onlyWhileInWater = true;

    private Rigidbody rb;
    private int waterCount = 0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionStay(Collision collision)
    {
        if (onlyWhileInWater && waterCount <= 0)
            return;

        if (!collision.collider.CompareTag("PoolWall"))
            return;

        for (int i = 0; i < collision.contactCount; i++)
        {
            ContactPoint contact = collision.GetContact(i);

            Vector3 wallNormal = contact.normal;
            wallNormal.y = 0f;

            if (wallNormal.sqrMagnitude < 0.0001f)
                continue;

            wallNormal.Normalize();

            float speedIntoWall = Vector3.Dot(rb.linearVelocity, -wallNormal);

            Vector3 pushAway = wallNormal * wallPushStrength;
            Vector3 slowIntoWall = Vector3.zero;

            if (speedIntoWall > 0f)
            {
                slowIntoWall = wallNormal * (speedIntoWall * wallSlowdown);
            }

            rb.AddForce(pushAway + slowIntoWall, ForceMode.Acceleration);
            break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Fusion.Fluid.Fluid>() != null)
            waterCount++;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Fusion.Fluid.Fluid>() != null)
            waterCount = Mathf.Max(0, waterCount - 1);
    }
}