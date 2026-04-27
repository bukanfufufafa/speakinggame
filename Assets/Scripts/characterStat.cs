using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterStat : MonoBehaviour
{
    public float health = 100f;
    public float mana = 50f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void getDamage (float damage)
    {
        health -= damage; 
    }
}
