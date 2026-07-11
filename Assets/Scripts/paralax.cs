using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BackgroundController : MonoBehaviour
{
    private float startPos;
    private float length;

    public GameObject cam;
    public float parallaxEffect; // kecepatan background relatif terhadap kamera
    public bool infiniteScroll;


    private float distance;
    private float movement;
    void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void FixedUpdate()
    {
        // Hitung pergerakan berdasarkan posisi kamera
        distance = cam.transform.position.x * parallaxEffect;
        movement = cam.transform.position.x * (1 - parallaxEffect);

        // Update posisi background
        transform.position = new Vector3(
            startPos + distance,
            transform.position.y,
            transform.position.z
        );

        if ( infiniteScroll)
        {
            infinite();
        }
        // Infinite scrolling
        //if (movement > startPos + length)
        //{
        //    startPos += length;
        //}
        //else if (movement < startPos - length)
        //{
        //    startPos -= length;
        //}
    }

    void infinite()
    {
        if (movement > startPos + length)
        {
            startPos += length;
        }
        else if (movement < startPos - length)
        {
            startPos -= length;
        }
    }
}