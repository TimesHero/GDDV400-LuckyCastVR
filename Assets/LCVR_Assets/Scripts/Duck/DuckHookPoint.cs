using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DuckHookPoint : MonoBehaviour
{
    [SerializeField] private Transform duckRoot;
    [SerializeField] private DuckAudioEvents duckAudioEvents;
    [SerializeField] private Rigidbody duckBody;
    [SerializeField] private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable duckGrab;

    //stores the hook position the duck should follow
    private Transform hangPose;

    //remembers if gravity was on before the duck got hooked
    private bool previousUseGravity;

    // True = the duck is currently
    public bool IsHooked => hangPose != null;

    private void Reset()
    {
        duckRoot = transform.root;
        duckBody = GetComponentInParent<Rigidbody>();
        duckGrab = GetComponentInParent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        duckAudioEvents = GetComponentInParent<DuckAudioEvents>();    
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

    private void LateUpdate()
    {
        if (hangPose == null || duckRoot == null)
            return;

        //Force duck to hook position
        duckRoot.SetPositionAndRotation(hangPose.position, hangPose.rotation);
    }

    public bool TryHook(Transform targetHangPose)
    {
        if (IsHooked || targetHangPose == null || duckBody == null || duckRoot == null)
            return false;

        //Save place the duck should hang from
        hangPose = targetHangPose;

        previousUseGravity = duckBody.useGravity;

        //stop other movement to attach to hook
        duckBody.linearVelocity = Vector3.zero;
        duckBody.angularVelocity = Vector3.zero;

        //ignores its own physics
        duckBody.useGravity = false;
        duckBody.isKinematic = true;

        //make duck hang its self
        duckRoot.SetPositionAndRotation(hangPose.position, hangPose.rotation);

        //Play audio when hooked
        if (duckAudioEvents != null)
        {
            duckAudioEvents.PlayHookedSound();
        }
        return true;
    }

    public void Unhook()
    {
        if (!IsHooked || duckBody == null)
            return;

        hangPose = null;
        duckBody.isKinematic = false;
        duckBody.useGravity = previousUseGravity;
        duckAudioEvents.PlayGrabSound();
    }

    private void OnDuckGrabbed(SelectEnterEventArgs args)
    {
        if (IsHooked)
            Unhook();
    }
}