using UnityEngine;
using System.Collections;
using System;

public class AnalogueClockController : MonoBehaviour
{
    private static WaitForSeconds waitForOneSecond = new WaitForSeconds(1f);

    public Transform hourPivot;
    public Transform minutePivot;
    public Transform secondPivot;

    private const float HOUR_STEP = 30f;
    private const float MINUTE_STEP = 6f;
    private const float SECOND_STEP = 6f;

    private Quaternion desiredMinuteRotation;
    private Quaternion desiredHourRotation;

    private void OnEnable()
    {
        CalculateHandPositionsNow();
        minutePivot.localRotation = desiredMinuteRotation;
        hourPivot.localRotation = desiredHourRotation;

        StartCoroutine(UpdateClockHands());
    }

    private void CalculateHandPositionsNow()
    {
        DateTime now = DateTime.Now;

        secondPivot.localEulerAngles = new(0 + now.Second * SECOND_STEP, 0, 0f);

        float minuteAngle = 90f + now.Minute * MINUTE_STEP + (MINUTE_STEP *(now.Second / 60f));
        desiredMinuteRotation = Quaternion.Euler(minuteAngle, 0, 0f);

        float hourAngle = 90f + now.Hour * HOUR_STEP + (HOUR_STEP * (now.Minute / 60f));
        desiredHourRotation = Quaternion.Euler(hourAngle, 0, 0f);
    }

    private IEnumerator UpdateClockHands()
    {
        yield return waitForOneSecond;

        CalculateHandPositionsNow();

        StartCoroutine(UpdateClockHands());
    }

    private void Update()
    {
        minutePivot.localRotation = Quaternion.Slerp(minutePivot.localRotation, desiredMinuteRotation, Time.deltaTime);
        hourPivot.localRotation = Quaternion.Slerp(hourPivot.localRotation, desiredHourRotation, Time.deltaTime);
    }

    private void OnDisable()
    {
        StopCoroutine(UpdateClockHands());
    }
}
