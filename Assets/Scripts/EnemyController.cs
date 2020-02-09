using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public class EnemyController : MonoBehaviour
{
    //References
    public Sprite Walking, Chasing, Gun;
    private Transform EnemyTransform;
    private Rigidbody2D rb2d;

    //Stats
    public float Speed = 10, RunSpeed = 15;
    public float HearRadius;
    public float SearchTime;
    public float stopTime = 3;
    public float RotationSpeed = 10;

    //View Distance
    public float ViewDistance = 3;
    public float ViewAngle = 45;        //Degrees
    public LayerMask ViewLayerMask;

    //States
    public enum States { Patrol, Chase, Searching };
    public List<Vector3> SearchPoint = new List<Vector3>();

    //Misc
    private States state;
    private bool isRunning;
    private bool isChasing;
    private bool inRange;
    public Vector3 target;
    private float timer;

    private void Awake() {
        //Sets the References
        rb2d = GetComponent<Rigidbody2D>();
        EnemyTransform = transform;
        state = States.Patrol;

        //Checks to make sure that the points exist first
        if (transform.Find("PatrolLocations") == null)
            return;

        GetSearchPoints(transform.Find("PatrolLocations"));
        target = SearchPoint[0];
    }

    private void Update() {
        ChangeState();
    }

    //This is the finite state-machine that the enemy will use
    private void ChangeState() {
        switch (state) {
            case States.Patrol:
                Patrol();
                //Stop timer
                if(timer < Time.time)
                    Movement();
                break;
            case States.Searching:
                Searching();
                break;
            case States.Chase:
                Chase();
                break;
        }
    }

    private void Chase(){
        //Face at the players
        Vector2 newDirection = (GameManager.Manager.Player.transform.position - EnemyTransform.position).normalized;
        EnemyTransform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(newDirection.y, newDirection.x) * Mathf.Rad2Deg);

    }

    //Looks around for the player, of he sees him then start following
    private void Searching()
    {
        //Checks for the player
        if (DetectPlayer()) {
            transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Gun;
            state = States.Chase;
            return;
        }

        //If the player wasn't spotted in the amount of time
        if (timer < Time.time) {
            transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Walking;
            state = States.Patrol;
        }
    }

    //Raycasts towards the player
    private bool DetectPlayer() {
        //Detects if the player is in front of the enemy
        RaycastHit2D hit = Physics2D.Raycast(EnemyTransform.position, EnemyTransform.right, ViewDistance, ViewLayerMask);
        if (hit.collider.tag == "Player")
            return true;

        //Detects if the player is far right
        hit = Physics2D.Raycast(EnemyTransform.position, (EnemyTransform.right + (EnemyTransform.up / ViewAngle)), ViewDistance, ViewLayerMask);
        if(hit.collider != null && hit.collider.tag == "Player")
            return true;

        //Detects if the player is far left
        hit = Physics2D.Raycast(EnemyTransform.position, (EnemyTransform.right - (EnemyTransform.up / ViewAngle)), ViewDistance, ViewLayerMask);
        if(hit.collider != null && hit.collider.tag == "Player")
            return true;

        //Detects if the player is mid-right
        hit = Physics2D.Raycast(EnemyTransform.position, (EnemyTransform.right + (EnemyTransform.up / ViewAngle / 2)), ViewDistance, ViewLayerMask);
        if(hit.collider != null && hit.collider.tag == "Player")
            return true;

        //Detects if the player is mid-left
        hit = Physics2D.Raycast(EnemyTransform.position, (EnemyTransform.right - (EnemyTransform.up / ViewAngle / 2)), ViewDistance, ViewLayerMask);
        if(hit.collider != null && hit.collider.tag == "Player")
            return true;

        print(hit.collider.name);

        //The player was not detected
        return false;
    }

    private void Patrol() {
        //Don't run if there is no positions to move to
        if (SearchPoint.Count == 0)
            return;

        //In range of Locator
        if(inRange) {
            //Sets the location
            target = SearchPoint[DontOverLap(SearchPoint.Count)];
            inRange = false;
        }

        //Breaks if there isn't a player or game manager in the scene so the game doesn't crash
        if (GameManager.Manager == null || GameManager.Manager.Player == null)
            return;

        //If the player gets too close
        if ((EnemyTransform.position - GameManager.Manager.Player.transform.position).magnitude <= HearRadius) {
            timer = Time.time + SearchTime;

            //Stops the Enemy
            rb2d.velocity = new Vector2(0, 0);
            Vector2 newDirection = (GameManager.Manager.Player.transform.position - EnemyTransform.position).normalized;
            EnemyTransform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(newDirection.y, newDirection.x) * Mathf.Rad2Deg);


            //Changes state and sprite
            state = States.Searching;
            transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Chasing;
        }
    }

    private void Movement() {
        //Look at target and moves towards it
        Vector2 newDirection = (target - EnemyTransform.position).normalized;

        //Moves the player and faces the direction
        rb2d.velocity = newDirection * (isRunning ? RunSpeed : Speed);
        EnemyTransform.rotation = Quaternion.Euler(0, 0 , Mathf.Atan2(newDirection.y, newDirection.x) * Mathf.Rad2Deg);

        //Stops if it gets in range with its target
        if ((target - EnemyTransform.position).magnitude < 0.5f) {
            rb2d.velocity = Vector3.zero;
            timer = Time.time + Random.Range(2f, stopTime);
            inRange = true;
        }
    }

    //Makes sure the Values Wrap around and don't return the same number
    private int DontOverLap(int max) {
        int randomLocation = Random.Range(0, max);
        if(randomLocation == 0)
            return 1;
        if(randomLocation == SearchPoint.Count - 1)
            return 0;

        return ++randomLocation;
    }

    //References all of the Children and adds them to the Vector3 List
    private void GetSearchPoints(Transform holder) {
        //Gets all of the positions and saves them
        for (int i = 0; i < holder.childCount; i++)
            SearchPoint.Add(new Vector3(holder.GetChild(i).position.x, 0, holder.GetChild(i).position.z));

        //Destroys the Object so it doesn't skew the information
        Destroy(holder.gameObject);
    }
}
