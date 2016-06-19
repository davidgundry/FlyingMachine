using UnityEngine;
using System.Collections;

public class FollowCamera : MonoBehaviour
{
    public GameController gameController;
    public Transform background;

    public Transform target;
    public Vector3 offset;
    public float minY;

    void Start()
    {
    }

    void Update()
    {

        if (!gameController.PlayPaused)
        {
            transform.position = new Vector3(target.position.x + offset.x, Mathf.Max(target.position.y + offset.y, minY), target.position.z + offset.z);

            if (transform.position.x > background.position.x + 11)
                background.position = new Vector3(background.position.x + 11, background.position.y, background.position.z);
            if (transform.position.x < background.position.x - 11)
                background.position = new Vector3(background.position.x - 11, background.position.y, background.position.z);
        }
    }
}