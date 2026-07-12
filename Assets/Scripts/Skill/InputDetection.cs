using System;
using UnityEngine;

public class InputDetection : MonoBehaviour
{
    [Header("Konfigurasi")]
    public KeyCode tombol = KeyCode.E;
    public float ambangHold = 0.35f;

    //event
    public event Action OnTap;
    public event Action OnHoldStart;
    public event Action OnHoldEnd;

    private float waktuMulaiTekan;
    private bool sedangHold = false;

    void Update()
    {
        if (Input.GetKeyDown(tombol))
        {
            waktuMulaiTekan = Time.time;
            sedangHold = false;
        }
        if (Input.GetKey(tombol))
        {
            float durasiTekan = Time.time - waktuMulaiTekan;
            if(!sedangHold && durasiTekan >= ambangHold)
            {
                sedangHold = true;
                OnHoldStart?.Invoke();
            }
        }
        if (Input.GetKeyUp(tombol))
        {
            if (sedangHold)
            {
                OnHoldEnd?.Invoke();
                sedangHold= false;
            }
            else
            {
                OnTap?.Invoke();
            }
        }
    }
}
