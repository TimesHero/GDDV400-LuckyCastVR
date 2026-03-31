using UnityEngine;
using TMPro;

public class VignetteChanger : MonoBehaviour
{
    public Behaviour vignetteController; 
    public TMP_Text statusText;

    void Awake()
    {
        UpdateUIText();
    }

    // Toggles the Vignette on and off
    //Couldn't actually get it working for editor testing. Not clear why.
    public void ToggleVignette()
    {
        if (vignetteController != null)
        {
            vignetteController.enabled = !vignetteController.enabled;
            UpdateUIText();
        }
    }
    
    private void UpdateUIText()
    {
        if (statusText != null && vignetteController != null)
        {
            statusText.text = vignetteController.enabled ? "ON" : "OFF";
        }
    }
}