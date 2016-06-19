using UnityEngine;
using System.Collections;

public class BlueprintPickupBehaviour : PickupBehaviour {

    Vector3 Velocity { get; set;}
    float TimeToChange { get; set;}
    private PlayerBehaviour PlayerBehaviour { get; set; }
    private Rigidbody2D playerRB;
    private GameController GameController { get; set; }

	void Start ()
    {
        PlayerBehaviour = GameObject.FindObjectOfType<PlayerBehaviour>();
        playerRB = PlayerBehaviour.GetComponent<Rigidbody2D>();
        GameController = GameObject.FindObjectOfType<GameController>();
        ResetPosition();
	}
	

	void Update ()
    {
        if (!GameController.PlayPaused)
        {
            if (TimeToChange <= 0)
            {
                ChangeVelocity();
                TimeToChange = Random.value * 4;
            }
            TimeToChange -= Time.deltaTime;
            transform.position = transform.position + (Velocity * Time.deltaTime);

            if (transform.position.y < 2)
                transform.position = new Vector3(transform.position.x,2,transform.position.z);

            if (transform.position.y > PlayerBehaviour.transform.position.y + 4)
                ResetPosition();
            if (transform.position.x > PlayerBehaviour.transform.position.x + 4)
                ResetPosition();
            if (transform.position.x < PlayerBehaviour.transform.position.x - 4)
                ResetPosition();
        }

	}

    public override void PickedUp()
    {
    }

    private void ChangeVelocity()
    {
        Velocity = new Vector3(Random.value / 2f - Velocity.x, Random.value / 2f - Velocity.y, 0);
    }

    public void ResetPosition()
    {
        transform.position = new Vector3(playerRB.position.x + (Random.value - 0.5f) * 4, playerRB.position.y + (Random.value - 0.5f) * 4);
        if (Mathf.Abs((new Vector3(playerRB.position.x,playerRB.position.y,0) - transform.position).magnitude) < 1f)
            ResetPosition();
    }

}
