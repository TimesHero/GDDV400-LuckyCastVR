using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DuckAudioEvents : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private AudioSource duckAudioSource;
    [SerializeField] private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable duckGrab;

    [Header("Sound Clips")]
    [SerializeField] private AudioClip grabClip;
    [SerializeField] private AudioClip hookedClip;
    [SerializeField] private AudioClip[] scoredInBucketClips;

    [Header("Volume Settings")]
    [SerializeField] private float scoredInBucketVolume = 1.0f;

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

    public void PlayScoredInBucketSound()
{
    if (duckAudioSource == null)
        return;

    if (scoredInBucketClips == null || scoredInBucketClips.Length == 0)
        return;

    int randomIndex = Random.Range(0, scoredInBucketClips.Length);
    AudioClip chosenClip = scoredInBucketClips[randomIndex];

    if (chosenClip != null)
        duckAudioSource.PlayOneShot(chosenClip, scoredInBucketVolume);
}
}