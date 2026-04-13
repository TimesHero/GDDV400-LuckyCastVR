using UnityEngine;
using System.Collections;

public class DuckRandomizer : MonoBehaviour
{
    float randomYRotation = 0f;


    void Start()
    {
        ChangeDuckMatColour();
        RotateDuck();
    }

    void Update()
    {
        
    }

    //Rotate Duck
    private void RotateDuck()
    {
        //yield return new WaitForSeconds(1);
        randomYRotation = Random.Range(0f,360f);
        //Debug.Log($"Duck Rotation: {randomYRotation} degrees.");
        transform.localRotation = Quaternion.Euler(0f, randomYRotation, 0f);
    }

    private void ChangeDuckMatColour()
    {
        return;
    }
}
