using UnityEngine;

public class LightToggle : MonoBehaviour
{
    public Light targetLight;

    public void ToggleTheLight()
    {
        if (targetLight != null)
        {
            targetLight.enabled = !targetLight.enabled;
        }
    }
}