using UnityEngine;
using UnityEngine.Windows.Speech;
using System.Collections.Generic;

public class SpeechManager : MonoBehaviour
{
    public ConfidenceLevel confidence = ConfidenceLevel.Medium;

    private KeywordRecognizer recognizer;
    private string[] allKeywords;

    void Start()
    {
        // Ambil semua enemy
        Enemy[] enemies = FindObjectsOfType<Enemy>();

        List<string> keywordsList = new List<string>();

        foreach (Enemy e in enemies)
        {
            foreach (string word in e.keywords)
            {
                if (!keywordsList.Contains(word))
                    keywordsList.Add(word);
            }
        }

        allKeywords = keywordsList.ToArray();

        recognizer = new KeywordRecognizer(allKeywords, confidence);
        recognizer.OnPhraseRecognized += OnRecognized;
        recognizer.Start();
    }

    void OnRecognized(PhraseRecognizedEventArgs args)
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();

        foreach (Enemy e in enemies)
        {
            e.CheckWord(args.text);
        }
    }

    void OnDestroy()
    {
        if (recognizer != null && recognizer.IsRunning)
        {
            recognizer.OnPhraseRecognized -= OnRecognized;
            recognizer.Stop();
        }
    }
}
