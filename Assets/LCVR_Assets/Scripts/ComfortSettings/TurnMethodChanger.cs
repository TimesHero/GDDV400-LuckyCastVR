using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Turning;
using TMPro;

public class TurnMethodChanger : MonoBehaviour
{
    public ContinuousTurnProvider continuousTurn;
    public SnapTurnProvider snapTurn;
    public TMP_Text statusText;

    void Awake()
    {
        UpdateUIText();
    }

    // Toggles between snap and continuous turning
    public void ToggleTurnMethod()
    {
        if (continuousTurn != null && snapTurn != null)
        {
            bool isContinuous = continuousTurn.enabled;
            
            continuousTurn.enabled = !isContinuous;
            snapTurn.enabled = isContinuous;

            UpdateUIText();
        }
    }

    // Syncs the UI text to the active turn provider
    private void UpdateUIText()
    {
        if (statusText != null && continuousTurn != null)
        {
            statusText.text = continuousTurn.enabled ? "Continuous" : "Snap";
        }
    }
}