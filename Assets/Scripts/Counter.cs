using System;
using System.Collections.Generic;
using UnityEngine;

public class Counter : MonoBehaviour
{
    [SerializeField]
    private Digit[] digits;

    [SerializeField]
    private AudioClip money;

    [SerializeField]
    private AudioClip oneHundred;

    [SerializeField]
    [Range(0f, 1f)]
    private float moneyVolume;

    private int score = 0;
    private int lastHundred = 0;

    private AudioSource audioSource;

    // Start is called before the first frame update
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        SetAllToZero();
    }

    private void SetAllToZero()
    {
        foreach (Digit d in digits)
        {
            d.SetDigit(0);
        }
    }

    public void SetPoints(int points)
    {
        score = points;

        int maxScore = (int)Math.Pow(10, digits.Length) - 1;

        if (score < 0)
        {
            score = 0;
        }

        if (score > maxScore)
        {
            score = maxScore;
        }
        SetAllToZero();
        int[] scoreArray = GetIntArray(score);

        for (int i = 0; i < scoreArray.Length; i++)
        {
            digits[i].SetDigit(scoreArray[i]);
        }
    }

    public void AddToScore(int points)
    {
        score += points;

        int maxScore = (int)Math.Pow(10, digits.Length) - 1;

        if (score < 0)
        {
            score = 0;
        }

        if (score > maxScore)
        {
            score = maxScore;
        }

        int currentHundred = score / 100;
        Debug.Log(currentHundred);
        if (currentHundred != lastHundred)
        {
            lastHundred = currentHundred;
            if (oneHundred)
            {
                audioSource.PlayOneShot(oneHundred, moneyVolume);
            }
        }
        else if (money)
        {
            audioSource.PlayOneShot(money, moneyVolume);
        }
        SetAllToZero();
        int[] scoreArray = GetIntArray(score);

        for (int i = 0; i < scoreArray.Length; i++)
        {
            digits[i].SetDigit(scoreArray[i]);
        }
    }

    public void SetMaterial(Material m)
    {
        foreach (Digit digit in digits)
        {
            digit.SetMaterial(m);
        }
    }

    private int[] GetIntArray(int num)
    {
        List<int> listOfInts = new List<int>();
        while (num > 0)
        {
            listOfInts.Add(num % 10);
            num = num / 10;
        }

        return listOfInts.ToArray();
    }
}