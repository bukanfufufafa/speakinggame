//using UnityEngine;
//using UnityEngine.Windows.Speech;
//using System.Collections.Generic;

//public class SpeechManager : MonoBehaviour
//{
//    public ConfidenceLevel confidence = ConfidenceLevel.Medium;

//    private KeywordRecognizer recognizer;
//    private string[] allKeywords;

//    private skill playerSkill;

//    void Start()
//    {
//        // Ambil skill player
//        playerSkill = FindObjectOfType<skill>();

//        List<string> keywordsList = new List<string>();

//        // Ambil semua keyword dari skill
//        foreach (string word in playerSkill.spells)
//        {
//            if (!keywordsList.Contains(word))
//                keywordsList.Add(word);
//        }

//        allKeywords = keywordsList.ToArray();

//        recognizer = new KeywordRecognizer(allKeywords, confidence);
//        recognizer.OnPhraseRecognized += OnRecognized;
//        recognizer.Start();
//    }

//    void OnRecognized(PhraseRecognizedEventArgs args)
//    {
//        Debug.Log("Kata terdeteksi: " + args.text);

//        // Kirim ke skill
//        playerSkill.spellingCast(args.text);
//    }

//    void OnDestroy()
//    {
//        if (recognizer != null && recognizer.IsRunning)
//        {
//            recognizer.OnPhraseRecognized -= OnRecognized;
//            recognizer.Stop();
//        }
//    }
//}