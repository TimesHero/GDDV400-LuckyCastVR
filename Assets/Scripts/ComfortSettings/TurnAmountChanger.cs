using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Turning;
using TMPro;

public class TurnAmountChanger : MonoBehaviour
{
    public SnapTurnProvider snapTurnProvider;
    public TMP_Text statusText; 

    private bool is45Degrees = true;

    void Awake()
    {
        UpdateUIText();
    }

    //should hopefully toggle between settings and update the menu's text
    public void ToggleTurnAmount()
    {
        if (snapTurnProvider != null)
        {
            is45Degrees = !is45Degrees;

            if (is45Degrees)
            {
                snapTurnProvider.turnAmount = 45f;
            }
            else
            {
                snapTurnProvider.turnAmount = 90f;
            }
            
            UpdateUIText();
        }
    }

    private void UpdateUIText()
    {
        if (statusText != null && snapTurnProvider != null)
        {
            statusText.text = snapTurnProvider.turnAmount + "°";
        }
    }
}