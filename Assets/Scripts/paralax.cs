using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    [Header("References")]
    public Transform cam;

    [Header("Parallax")]
    [Range(0f, 1f)]
    public float parallaxX = 0.5f;

    [Range(0f, 1f)]
    public float parallaxY = 0.5f;

    [Header("Infinite Scroll")]
    public bool infiniteScroll = true;

    private float startPosX;
    private float startPosY;
    private float spriteLength;

    void Start()
    {
        startPosX = transform.position.x;
        startPosY = transform.position.y;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        if (sr != null)
            spriteLength = sr.bounds.size.x;
        else
            Debug.LogError("BackgroundController membutuhkan SpriteRenderer!");
    }

    void LateUpdate()
    {
        // Efek parallax
        float posX = startPosX + cam.position.x * parallaxX;
        float posY = startPosY + cam.position.y * parallaxY;

        transform.position = new Vector3(posX, posY, transform.position.z);

        if (infiniteScroll)
        {
            float camRelative = cam.position.x * (1 - parallaxX);

            if (camRelative > startPosX + spriteLength)
            {
                startPosX += spriteLength;
            }
            else if (camRelative < startPosX - spriteLength)
            {
                startPosX -= spriteLength;
            }
        }
    }
}