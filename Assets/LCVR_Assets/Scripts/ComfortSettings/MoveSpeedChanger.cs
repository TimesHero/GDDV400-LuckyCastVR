using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Movement;
using UnityEngine.UI;
using TMPro;

public class MoveSpeedChanger : MonoBehaviour
{
    public ContinuousMoveProvider continuousMoveProvider;
    public TMP_Text statusText; 
    public Slider speedSlider;

    void Awake()
    {
        if (continuousMoveProvider != null && speedSlider != null)
        {
            speedSlider.value = continuousMoveProvider.moveSpeed;
        }
        UpdateUIText();
    }

    // Updates moveSpeed based on the float passed by the slider
    public void SetMoveSpeed(float newSpeed)
    {
        if (continuousMoveProvider != null)
        {
            continuousMoveProvider.moveSpeed = newSpeed;
            UpdateUIText();
        }
    }

    // Syncs the UI text formatting to one decimal place
    private void UpdateUIText()
    {
        if (statusText != null && continuousMoveProvider != null)
        {
            statusText.text = continuousMoveProvider.moveSpeed.ToString("F1");
        }
    }
}