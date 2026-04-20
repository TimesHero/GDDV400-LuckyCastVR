using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DuckHookPoint : MonoBehaviour
{
    [SerializeField] private Transform duckRoot;
    [SerializeField] private DuckAudioEvents duckAudioEvents;
    [SerializeField] private Rigidbody duckBody;
    [SerializeField] private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable duckGrab;
    [SerializeField] private Behaviour basicFluidInteractor;
    [SerializeField] private HookCatchZone hookCatchZone;

    private Transform hangPose;
    private bool previousUseGravity;
    public bool IsHooked => hangPose != null;

    [SerializeField] private GameManager gameManager;
    [SerializeField] public bool wasHooked;
    [SerializeField] public bool hasScored;
    public int duckValue;
    public Vector3 startPosition;
    private Quaternion startRotation;

    private void Reset()
    {
        duckRoot = transform.root;
        duckBody = GetComponentInParent<Rigidbody>();
        duckGrab = GetComponentInParent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        duckAudioEvents = GetComponentInParent<DuckAudioEvents>();  
        basicFluidInteractor = GetComponentInParent<Behaviour>();
          
    }

    private void OnEnable()
    {
        if (duckGrab != null)
            duckGrab.selectEntered.AddListener(OnDuckGrabbed);
            duckGrab.selectExited.AddListener(OnDuckUnGrabbed);
    }

    private void OnDisable()
    {
        if (duckGrab != null)
            duckGrab.selectEntered.RemoveListener(OnDuckGrabbed);
            duckGrab.selectExited.RemoveListener(OnDuckUnGrabbed);
    }

    private void LateUpdate()
    {
        if (hangPose == null || duckRoot == null)
            return;

        //Force duck to hook position
        duckRoot.SetPositionAndRotation(hangPose.position, hangPose.rotation);
    }

        public void SetHookCatchZone(HookCatchZone zone)
    {
        hookCatchZone = zone;
    }

    public bool TryHook(Transform targetHangPose)
    {
        if (IsHooked || targetHangPose == null || duckBody == null || duckRoot == null)
            return false;

        wasHooked = true;

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
        //Debug.Log($"Unhook entered for {name}. hangPose={(hangPose == null ? "null" : hangPose.name)}");
        if (!IsHooked || duckBody == null)
        {
            //Debug.Log("Unhook() failed: not hooked or no rigidbody");
            return;
        }

        //Debug.Log("Unhook() called");

        hangPose = null;

        if (hookCatchZone != null)
            hookCatchZone.ClearCurrentDuck(this); 

        duckBody.isKinematic = false;
        duckBody.useGravity = true;
        duckBody.linearVelocity = Vector3.zero;
        duckBody.angularVelocity = Vector3.zero;

        //Debug.Log($"After unhook: isKinematic={duckBody.isKinematic}, useGravity={duckBody.useGravity}");
    }

    private void OnDuckGrabbed(SelectEnterEventArgs args)
    {
        //Debug.Log($"OnDuckGrabbed fired for {name}. IsHooked={IsHooked}");
        if (IsHooked == true)
            Unhook();

        if (!wasHooked)
        {
            if (gameManager != null)
                gameManager.HandleIllegalGrab();
        }
    }

        private void OnDuckUnGrabbed(SelectExitEventArgs args)
    {
        if (duckBody == null)
            return;

        duckBody.isKinematic = false;
        duckBody.useGravity = true;

        //Debug.Log($"OnDuckUngrabbed forced state: isKinematic={duckBody.isKinematic}, useGravity={duckBody.useGravity}");
    }
}