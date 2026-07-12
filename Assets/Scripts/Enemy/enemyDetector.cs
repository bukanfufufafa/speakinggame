using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyDetector : MonoBehaviour
{
    [SerializeField] EnemyRoutine enemyRoutine;
    [SerializeField] private bool chasingPlayer = false;
    private bool flee = false;
    // Start is called before the first frame update
    void Start()
    {
        enemyRoutine = GetComponent<EnemyRoutine>();
    }

    // Update is called once per frame
    void Update()
    {
        if (chasingPlayer)
        {
            enemyRoutine.Chasing();
        }
        else
        {
            enemyRoutine.ReturnToStart();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            chasingPlayer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            chasingPlayer= false;
        }
    }

    
}
