using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteFlipbook : MonoBehaviour
{
    [Header("Frames (drag sliced sprites in order)")]
    public Sprite[] frames;

    [Header("Settings")]
    public float frameRate = 24f;
    public bool disableOnFinish = true;

    private SpriteRenderer sr;
    private Coroutine playing;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void Play()
    {
        gameObject.SetActive(true);
        if (playing != null) StopCoroutine(playing);
        playing = StartCoroutine(PlayRoutine());
    }

    public void Stop()
    {
        if (playing != null) StopCoroutine(playing);
        gameObject.SetActive(false);
    }

    private IEnumerator PlayRoutine()
    {
        float delay = 1f / frameRate;

        for (int i = 0; i < frames.Length; i++)
        {
            sr.sprite = frames[i];
            yield return new WaitForSeconds(delay);
        }

        if (disableOnFinish)
            gameObject.SetActive(false);
    }
}