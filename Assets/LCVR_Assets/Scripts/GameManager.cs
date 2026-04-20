using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text[] statusText;
    [SerializeField] private TMP_Text[] scoreText;

    [Header("Duck Assignment")]
    [SerializeField] private bool assignDuckValuesOnStart = true;

    [Header("Display Audio")]
    [SerializeField] private AudioSource[] displayAudioSources;
    [SerializeField] private AudioClip bustedClip;
    [SerializeField] private AudioClip winnerClip;
    [SerializeField] private AudioClip scoreTickClip;
    [SerializeField] private float scoreTickDelay = 0.1f;
    [SerializeField] private float scoreTickVolume = .5f;
    private int nextDisplayAudioIndex = 0;



    private bool roundLocked = false;
    public bool IsRoundLocked => roundLocked;

    private int score = 0;

    private readonly List<int> availableDuckValues = new List<int>();
    private readonly List<int> scoredDuckValues = new List<int>();
    private readonly HashSet<int> uniqueScoredValues = new HashSet<int>();

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
        scoredDuckValues.Add(duckValue);
        PlayScoreTickSequence(duckValue);

        DuckAudioEvents duckAudio = duck.GetComponentInParent<DuckAudioEvents>();
        if (duckAudio != null)
        {
            Debug.Log($"Duck Audio Found");
            duckAudio.PlayScoredInBucketSound();
        }

        if (uniqueScoredValues.Contains(duckValue))
        {
            roundLocked = true;
            UpdateScoreUI();
            ShowMessage($"BUSTED \n Please Reset");
            PlayBustedSound();
            Debug.Log("BUSTED");
            return;
        }

        uniqueScoredValues.Add(duckValue);
        score += duckValue;

        if (uniqueScoredValues.Count >= 7)
        {
            roundLocked = true;
            UpdateScoreUI();
            ShowMessage($"YOU WIN!\n Can you get a higher score?");
            PlayWinnerSound();
            Debug.Log("YOU WIN!");
            return;
        }

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
        scoredDuckValues.Clear();
        uniqueScoredValues.Clear();

        UpdateScoreUI();
        ShowMessage("Please use the fishing rod. Reset the round to try again.");
    }

    public void ShowMessage(string message)
    {
        if (statusText != null)
            for (int i = 0; i < statusText.Length; i++)
            {
                if (statusText[i] != null)
                    statusText[i].text = message;
            }

        Debug.Log(message);
    }

        private void UpdateScoreUI()
    {
        string pulledValuesText = scoredDuckValues.Count > 0
            ? string.Join(", ", scoredDuckValues)
            : "None";

        string scoreDisplay =
            $"Duck Score:\n {score}\n" +
            $"Ducks Pulled:\n {pulledValuesText}.";

        if (scoreText != null)
        {
            for (int i = 0; i < scoreText.Length; i++)
            {
                if (scoreText[i] != null)
                    scoreText[i].text = scoreDisplay;
            }
        }
    }

    private void ShuffleList(List<int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(i, list.Count);
            (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
        }
    }

    private void PlayBustedSound()
    {
        if (displayAudioSources == null || displayAudioSources.Length == 0)
            return;

        if (bustedClip == null)
            return;

        for (int i = 0; i < displayAudioSources.Length; i++)
        {
            if (displayAudioSources[i] != null)
                displayAudioSources[i].PlayOneShot(bustedClip);
        }
    }

    private void PlayWinnerSound()
    {
        if (displayAudioSources == null || displayAudioSources.Length == 0)
            return;

        if (winnerClip == null)
            return;

        for (int i = 0; i < displayAudioSources.Length; i++)
        {
            if (displayAudioSources[i] != null)
                displayAudioSources[i].PlayOneShot(winnerClip);
        }
    }

    private void PlayScoreTickSequence(int valueScored)
    {
        if (valueScored <= 0)
            return;

        StartCoroutine(PlayScoreTickSound(scoreTickClip, valueScored, scoreTickDelay));
    }

    private IEnumerator PlayScoreTickSound(AudioClip clip, int times, float delay)
    {
        if (displayAudioSources == null || displayAudioSources.Length == 0)
            yield break;

        if (clip == null || times <= 0)
            yield break;

        for (int i = 0; i < times; i++)
        {
            AudioSource currentSource = displayAudioSources[nextDisplayAudioIndex];

            if (currentSource != null)
                currentSource.PlayOneShot(clip, scoreTickVolume);

            nextDisplayAudioIndex++;
            if (nextDisplayAudioIndex >= displayAudioSources.Length)
                nextDisplayAudioIndex = 0;

            yield return new WaitForSeconds(delay);
        }
    }
}