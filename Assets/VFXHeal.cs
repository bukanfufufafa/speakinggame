using UnityEngine;

public class HealVFXController : MonoBehaviour
{
    [Tooltip("Length in seconds of your longest layer's animation")]
    public float totalDuration = 1f;

    public void Play()
    {
        gameObject.SetActive(false);
        gameObject.SetActive(true);

        CancelInvoke();
        Invoke(nameof(TurnOff), totalDuration);
    }

    public void Stop()
    {
        CancelInvoke();
        gameObject.SetActive(false);
    }

    private void TurnOff()
    {
        gameObject.SetActive(false);
    }
}