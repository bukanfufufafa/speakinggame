using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

/// <summary>
/// see here https://lightbuzz.com/speech-recognition-unity/
/// </summary>
public class menu : MonoBehaviour
{
    public string[] keywords = new string[] { "Play", "Settings", "Quit", "back"};
    public ConfidenceLevel confidence = ConfidenceLevel.Low;
    public GameObject settingspanel;

    

    protected PhraseRecognizer recognizer;
    protected string word;

    private void Start()
    {
        settingspanel.active = false;
        if (keywords != null)
        {
            recognizer = new KeywordRecognizer(keywords, confidence);
            recognizer.OnPhraseRecognized += Recognizer_OnPhraseRecognized;
            recognizer.Start();
            Debug.Log(recognizer.IsRunning);
        }

        foreach (var device in Microphone.devices)
        {
            Debug.Log("Name: " + device);
        }
    }

    private void Recognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        word = args.text;
        Debug.Log("Recognized word: " + word);
    }

    private void Update()
    {
        if (string.IsNullOrEmpty(word)) return;

        switch (word)
        {
            case "Play":
                play();
                break;
            case "Settings":
                setting();
                break;
            case "Quit":
                quit();
                break;
            case "back":
                back(); 
                break;
        }

        word = "";
    }


    private void OnApplicationQuit()
    {
        if (recognizer != null && recognizer.IsRunning)
        {
            recognizer.OnPhraseRecognized -= Recognizer_OnPhraseRecognized;
            recognizer.Stop();
        }
    }

    public void play()
    {
        SceneManager.LoadScene("gameplay");
    }

    public void quit()
    {
        Application.Quit();
    }

    public void setting()
    {
        settingspanel.active = true;
    }

    public void back()
    {
        settingspanel.active = false;
    }
}
