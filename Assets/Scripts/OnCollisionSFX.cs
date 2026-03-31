using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class OnCollisionSFX : MonoBehaviour
{
    public AudioClip audioClip;
    public AudioSource audioSource;

    private void Awake()
    {
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
    }
    private void OllisionEnter(Collision collision)
    {
        audioSource.PlayOneShot(audioClip);
    }
}
