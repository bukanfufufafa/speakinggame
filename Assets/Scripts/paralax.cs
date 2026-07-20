using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    private float startPosX;
    private float startPosY;
    private float length;

    public GameObject cam;

    [Header("Parallax")]
    [Range(0f, 1f)] public float parallaxX = 0.5f;
    [Range(0f, 1f)] public float parallaxY = 0.5f;

    [Header("Infinite Scroll")]
    public bool infiniteScroll = true;

    private float movement;

    void Start()
    {
        startPosX = transform.position.x;
        startPosY = transform.position.y;

        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void FixedUpdate()
    {
        // Hitung posisi parallax
        float distanceX = cam.transform.position.x * parallaxX;
        float distanceY = cam.transform.position.y * parallaxY;

        // Digunakan untuk infinite scroll horizontal
        movement = cam.transform.position.x * (1 - parallaxX);

        // Update posisi background
        transform.position = new Vector3(
            startPosX + distanceX,
            startPosY + distanceY,
            transform.position.z
        );

        if (infiniteScroll)
        {
            Infinite();
        }
    }

    void Infinite()
    {
        if (movement > startPosX + length)
        {
            startPosX += length;
        }
        else if (movement < startPosX - length)
        {
            startPosX -= length;
        }
    }
}