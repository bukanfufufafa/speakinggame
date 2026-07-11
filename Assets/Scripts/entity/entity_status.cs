using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class entity_status : MonoBehaviour
{
    // Start is called before the first frame update
    public float maxHealth = 300;
    public float health;
    public float mana;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void takeDamage(float damage)
    {
        health = health - damage;
    }

    
}
