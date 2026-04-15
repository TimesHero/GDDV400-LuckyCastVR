using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text statusText;
    [SerializeField] private TMP_Text scoreText;

    [Header("Duck Assignment")]
    [SerializeField] private bool assignDuckValuesOnStart = true;

    private bool roundLocked = false;
    public bool IsRoundLocked => roundLocked;

    private int score = 0;

    private List<int> availableDuckValues = new List<int>();
    private DuckHookPoint[] allDucks;

    private void Start()
    {
        if (assignDuckValuesOnStart)
        {
            BuildDuckValuePool();
            AssignDuckValuesToSceneDucks();
        }

        UpdateScoreUI();
    }

    private void BuildDuckValuePool()
    {
        availableDuckValues.Clear();

        for (int value = 12; value >= 1; value--)
        {
            for (int count = 0; count < value; count++)
            {
                availableDuckValues.Add(value);
            }
        }

        availableDuckValues.Add(0);

        ShuffleList(availableDuckValues);
    }

    private void AssignDuckValuesToSceneDucks()
    {
        allDucks = FindObjectsByType<DuckHookPoint>(FindObjectsSortMode.None);

        if (allDucks.Length > availableDuckValues.Count)
        {
            Debug.LogWarning(
                $"There are {allDucks.Length} ducks, but only {availableDuckValues.Count} values in the value pool.");
        }

        int ducksToAssign = Mathf.Min(allDucks.Length, availableDuckValues.Count);

        for (int i = 0; i < ducksToAssign; i++)
        {
            allDucks[i].duckValue = availableDuckValues[i];
            allDucks[i].wasHooked = false;
            allDucks[i].hasScored = false;
        }

        for (int i = ducksToAssign; i < allDucks.Length; i++)
        {
            allDucks[i].duckValue = -1;
            allDucks[i].wasHooked = false;
            allDucks[i].hasScored = false;
        }
    }

    public void ScoreDuck(int duckValue, DuckHookPoint duck)
    {
        if (roundLocked)
            return;

        if (duck == null)
            return;

        if (!duck.wasHooked)
            return;

        if (duck.hasScored)
            return;

        if (duckValue < 0)
            return;

        duck.hasScored = true;
        score += duckValue;

        UpdateScoreUI();
        ShowMessage($"Scored duck: {duckValue}");
        Debug.Log($"Score: {score}");
    }

    public void HandleIllegalGrab()
    {
        if (roundLocked)
            return;

        roundLocked = true;
        score = 0;
        UpdateScoreUI();
        ShowMessage("Please use the fishing rod. Reset the round to try again.");
    }

    public void ShowMessage(string message)
    {
        if (statusText != null)
            statusText.text = message;

        Debug.Log(message);
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = $"Score: {score}";
    }

    private void ShuffleList(List<int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(i, list.Count);
            (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
        }
    }
}