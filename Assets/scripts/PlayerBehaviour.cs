using UnityEngine;
using System.Collections;

public class PlayerBehaviour : MonoBehaviour
{

    public bool GravityEnabled { get; set; }
    public bool SpeechEnabled { get; set; }

    // Bounds
    private const float maxYVelocity = 0.5f;
    private const float maxXVelocity = 1f;
    private const float maxY = 6.5f;

    private Rigidbody2D rb;

    private float forceTime = 0f;

    private float MoveSpeed { get; set; }

    public float Speed()
    {
        return rb.velocity.magnitude;
    }

    private Vector2 savedVelocity;
    private float savedAngularVelocity;

    private GameController GameController { get; set; }

    void Awake()
    {
        MoveSpeed = 200;
        rb = GetComponent<Rigidbody2D>();
        GameController = GameObject.FindObjectOfType<GameController>();
    }


    void Update()
    {
        if (!GameController.PlayPaused)
        {
            AddActiveForces();
            AddPassiveForces();
            ApplyPhysicalBounds();
        }
        if ((!rb.IsSleeping()) && (GameController.PlayPaused))
        {
            PauseRigidBody();
        }
        if ((rb.IsSleeping()) && (!GameController.PlayPaused))
        {
            UnpauseRigidBody();
        }
    }

    private void PauseRigidBody()
    {
        rb.Sleep();
    }

    private void UnpauseRigidBody()
    {
        rb.WakeUp();
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "Pickup")
        {
            Debug.Log("Collider");
            BlueprintPickupBehaviour behaviour = other.GetComponent<BlueprintPickupBehaviour>();
            behaviour.PickedUp();
            behaviour.ResetPosition();
            GameController.GotBlueprint();
        }
        if (other.transform.tag == "Floor")
        {
            GameController.CrashFloor();
        }
    }

    private void AddPassiveForces()
    {
        if (GravityEnabled)
            rb.AddForce(new Vector2(0, -25) * Time.deltaTime);
    }

    private void AddActiveForces()
    {
        //rb.AddForce(new Vector2(150, 0) * Input.acceleration.x * Time.deltaTime);

        if ((Input.GetKey("left")) || (Input.acceleration.x  < -0.1f))
            rb.AddForce(new Vector2(-1, 0) * MoveSpeed * Time.deltaTime);

        if ((Input.GetKey("right")) || (Input.acceleration.x  > 0.1f))
            rb.AddForce(new Vector2(1, 0) * MoveSpeed * Time.deltaTime);

        if (SpeechEnabled)
        {
            if (forceTime > 0)
            {
                if (forceTime > 0.1f)
                    rb.AddForce(new Vector2(0, 1f) * 70 * Time.deltaTime);
                else
                    rb.AddForce(new Vector2(0, 1f) * 50 * Time.deltaTime);
                forceTime -= Time.deltaTime;
            }

            //if (Input.GetKeyDown("up"))
            //    Thrust();
        }
    }

    private void ApplyPhysicalBounds()
    {
        transform.position = new Vector3(transform.position.x, Mathf.Min(transform.position.y, maxY), transform.position.z);
        rb.velocity = new Vector3( Mathf.Max(Mathf.Min(rb.velocity.x, maxXVelocity),-maxXVelocity), Mathf.Max(Mathf.Min(rb.velocity.y, maxYVelocity),-maxYVelocity), 0);
    }


    public void Thrust()
    {
        forceTime = 0.5f;
    }

}