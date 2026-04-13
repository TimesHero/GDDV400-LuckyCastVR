using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// This script handles duck-specific sounds.
// Put it on the duck ROOT object.
public class DuckAudioEvents : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private AudioSource duckAudioSource;
    [SerializeField] private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable duckGrab;

    [Header("Sound Clips")]
    [SerializeField] private AudioClip grabClip;
    [SerializeField] private AudioClip hookedClip;
    [SerializeField] private AudioClip impactClip;

    [Header("Impact Settings")]
    [SerializeField] private float minimumImpactSpeed = 1.0f;
    [SerializeField] private float impactVolumeMultiplier = 0.15f;

    private void Reset()
    {
        duckAudioSource = GetComponent<AudioSource>();
        duckGrab = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
    }

    private void OnEnable()
    {
        if (duckGrab != null)
            duckGrab.selectEntered.AddListener(OnDuckGrabbed);
    }

    private void OnDisable()
    {
        if (duckGrab != null)
            duckGrab.selectEntered.RemoveListener(OnDuckGrabbed);
    }

    private void OnDuckGrabbed(SelectEnterEventArgs args)
    {
        PlayGrabSound();
    }

    public void PlayGrabSound()
    {
        if (duckAudioSource != null && grabClip != null)
            duckAudioSource.PlayOneShot(grabClip);
    }

    public void PlayHookedSound()
    {
        if (duckAudioSource != null && hookedClip != null)
            duckAudioSource.PlayOneShot(hookedClip);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (duckAudioSource == null || impactClip == null)
            return;

        // Measure how hard the duck hit something
        float impactSpeed = collision.relativeVelocity.magnitude;

        // Ignore tiny bumps so the sound does not spam
        if (impactSpeed < minimumImpactSpeed)
            return;

        // Scale the volume a bit based on impact strength.
        float volume = Mathf.Clamp01(impactSpeed * impactVolumeMultiplier);

        duckAudioSource.PlayOneShot(impactClip, volume);
    }
}