﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Sentence
{
    public string sentence;
    public List<string> words;
    public static int wordCount;

    [Header("Leave empty if you want randomised")]
    public string desiredRandom;

    public List<string> ActualString()
    {
        words = new List<string>(Regex.Matches(sentence, "\\w+").OfType<Match>().Select(m => m.Value).ToArray());
        wordCount = words.Count;
        return words;
    }

    public string GetString()
    {
        if (!string.IsNullOrEmpty(desiredRandom))
        {
            return desiredRandom;
        }

        string result = sentence;

        while (result == sentence)
        {
            result = "";
            words = ActualString();
            //UnityEngine.Debug.Log(words.Count);

            while (words.Count > 0)
            {
                int indexWord = UnityEngine.Random.Range(0, words.Count);
                result += words[indexWord];
                if (words.Count > 1)
                {
                    result += " ";
                }
                words.RemoveAt(indexWord);
            }
        }
        //UnityEngine.Debug.Log("Scrambled sentence");
        //UnityEngine.Debug.Log(result);
        return result;
    }
}

public class SentenceJumble : MonoBehaviour
{
    public Sentence[] sentences;
    public GameObject retryPanel;
    public GameObject scenePanel;
    public GameObject introPanel;
    public static List<string> originalWords;

    [Header("UI REFERENCE")]
    public WordObject prefab;
    public Transform container;
    public float space;
    public float lerpSpeed = 5;

    List<WordObject> wordObjects = new List<WordObject>();
    WordObject firstSelected;

    public int currentSentence;
    public static SentenceJumble main;
    private float timer = 20f;
    private Text timerSeconds;

    public void ChangeScene(string scene)
    {
        CheckSentence();
    }

    void Awake()
    {
        main = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        ShowScramble();
        //ShowScramble(currentSentence);
        timerSeconds = GameObject.Find("Timer").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        RepositionObject();
        timer -= Time.deltaTime;
        timerSeconds.text = timer.ToString("0");
        if (timer <= 1)
        {
            CheckSentence();
        }
    }

    void RepositionObject()
    {
        float center = 0;
        if (wordObjects.Count == 0)
        {
            return;
        }

        if (wordObjects.Count % 2 == 0)
        {
            center = ((wordObjects.Count - 1) / 2) + 0.5f;
        }
        else
        {
            center = (wordObjects.Count - 1) / 2;
        }

        for (int i = 0; i < wordObjects.Count; i++)
        {
            wordObjects[i].rectTransform.anchoredPosition
                = Vector2.Lerp(wordObjects[i].rectTransform.anchoredPosition,
                new Vector2((i - center) * space, 0), lerpSpeed * Time.deltaTime);
            wordObjects[i].index = i;
        }
    }

    /// <summary>
    /// Show a random sentence on the screen
    /// </summary>
    public void ShowScramble()
    {
        string difficulty = LevelDifficulty.difficulty;
        int level = LevelDifficulty.level;
        UnityEngine.Debug.Log("Difficulty");
        UnityEngine.Debug.Log(difficulty);

        //ShowScramble(UnityEngine.Random.Range(0, sentences.Length));

        if (difficulty == "Easy")
        {
            if (level == 1)
            {
                ShowScramble(0);
            }
            else if (level == 2)
            {
                ShowScramble(1);
            }
            else if (level == 3)
            {
                ShowScramble(2);
            }
            else if (level == 4)
            {
                ShowScramble(3);
            }
            else if (level == 5)
            {
                ShowScramble(4);
            }
        }
        else if (difficulty == "Medium")
        {
            if (level == 1)
            {
                ShowScramble(5);
            }
            else if (level == 2)
            {
                ShowScramble(6);
            }
            else if (level == 3)
            {
                ShowScramble(7);
            }
            else if (level == 4)
            {
                ShowScramble(8);
            }
            else if (level == 5)
            {
                ShowScramble(9);
            }
        }
        else if (difficulty == "Hard")
        {
            if (level == 1)
            {
                ShowScramble(10);
            }
            else if (level == 2)
            {
                ShowScramble(11);
            }
            else if (level == 3)
            {
                ShowScramble(12);
            }
            else if (level == 4)
            {
                ShowScramble(13);
            }
            else if (level == 5)
            {
                ShowScramble(14);
            }
        }
        else if (difficulty == "Extreme")
        {
            if (level == 1)
            {
                ShowScramble(15);
            }
            else if (level == 2)
            {
                ShowScramble(16);
            }
            else if (level == 3)
            {
                ShowScramble(17);
            }
            else if (level == 4)
            {
                ShowScramble(18);
            }
            else if (level == 5)
            {
                ShowScramble(19);
            }
        }
    }

    /// <summary>
    /// Show sentence from collection with desired index
    /// </summary>
    /// <param name="index"index of the element></param>
    public void ShowScramble(int index)
    {
        wordObjects.Clear();
        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }

        //Sentences finished
        //if (index > sentences.Length - 1)
        //{
        //    UnityEngine.Debug.LogError("Index out of range, please enter range between 0-" + (sentences.Length - 1).ToString());
        //    return;
        //}

        string wordString = sentences[index].GetString();
        string[] wordsInSentence = Regex.Matches(wordString, "\\w+").OfType<Match>().Select(m => m.Value).ToArray();

        originalWords = sentences[index].ActualString();
        //UnityEngine.Debug.Log("Sentences are");
        //UnityEngine.Debug.Log(originalWords[0]);

        foreach (string w in wordsInSentence)
        {
            WordObject clone = Instantiate(prefab.gameObject).GetComponent<WordObject>();
            clone.transform.SetParent(container);
            wordObjects.Add(clone.Init(w));
        }

        currentSentence = index;
    }

    public void Swap(int indexA, int indexB)
    {
        WordObject tmpA = wordObjects[indexA];
        wordObjects[indexA] = wordObjects[indexB];
        wordObjects[indexB] = tmpA;

        wordObjects[indexA].transform.SetAsLastSibling();
        wordObjects[indexB].transform.SetAsLastSibling();

        //CheckSentence();
    }

    public void Select(WordObject wordObject)
    {
        if (firstSelected)
        {
            Swap(firstSelected.index, wordObject.index);

            firstSelected.Select();
            wordObject.Select();
        }
        else
        {
            firstSelected = wordObject;
        }
    }

    public void UnSelect()
    {
        firstSelected = null;
    }

    public void CheckSentence()
    {
        StartCoroutine(CoCheckSentence());
    }

    IEnumerator CoCheckSentence()
    {
        yield return new WaitForSeconds(0.5f);
        string sentence = "";
        int numOfWords = wordObjects.Count;

        foreach (WordObject wordObject in wordObjects)
        {
            sentence += wordObject.word;
            if (numOfWords > 1)
            {
                sentence += " ";
                numOfWords--;
            }
            //UnityEngine.Debug.Log("sentence");
            //UnityEngine.Debug.Log(sentence);
        }

        //UnityEngine.Debug.Log("sentences[currentSentence].sentence");
        //UnityEngine.Debug.Log(sentences[currentSentence].sentence);
        if (sentence == sentences[currentSentence].sentence)
        {
            //currentSentence++;
            //ShowScramble(currentSentence);
            SceneManager.LoadScene("ArenaZone");
        }
        else
        {
            retryPanel.SetActive(true);
            scenePanel.SetActive(false);
        }
    }
}