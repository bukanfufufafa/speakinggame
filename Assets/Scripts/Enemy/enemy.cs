using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

public class Enemy : MonoBehaviour
{
    public string[] keywords;
    [SerializeField] private string word;
    public TMP_Text text;
    public void Start()
    {
        text.text = keywords[0];
    }
    public void CheckWord(string word)
    {
        foreach (string key in keywords)
        {
            if (key == word)
            {
                Die();
                return;
            }
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
