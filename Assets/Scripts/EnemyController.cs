using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour {
    //References
    public Sprite Walking, Chasing, Gun;
    private Transform _enemyTransform;
    private Rigidbody2D _rb2d;
    private CommentHandler _commentHandler;

    //Stats
    public float Speed = 10, RunSpeed = 15;
    public float HearRadius;
    public float ChaseRadius;
    public float SearchTime;
    public float StopTime = 3;

    //View Distance
    public float ViewDistance = 3;
    public float ViewAngle = 45;        //Degrees
    public LayerMask ViewLayerMask;

    //States
    public enum States { Patrol, Chase, Searching };
    public List<List<Vector3>> SearchPoint = new List<List<Vector3>>();

    //Misc
    private States _state;
    private bool _isRunning;
    private bool _isChasing;
    private bool _inRange;
    private bool _movingForward = true;
    private int _currentPath;
    private int _pathState;
    private float _timer;
    private Vector3 _target;
    private Vector3 _playerLastPos;


    private void Awake() {
        //Sets the References
        _commentHandler = GetComponent<CommentHandler>();
        _rb2d = GetComponent<Rigidbody2D>();
        _enemyTransform = transform;
        _state = States.Patrol;

        //Checks to make sure that the points exist first
        if (transform.Find("PatrolLocations") == null)
            return;

        GetSearchPoints(transform.Find("PatrolLocations"));
        _target = SearchPoint[0][0];
    }

    private void Update() {
        ChangeState();
    }

    //This is the finite _state-machine that the enemy will use
    private void ChangeState() {
        switch (_state) {
            case States.Patrol:
                Patrol();
                //Stop _timer
                if(_timer < Time.time)
                    Movement(_target);
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

        Vector2 newDirection = (GameManager.Manager.Player.transform.position - _enemyTransform.position).normalized;
        var hit = Physics2D.Raycast(_enemyTransform.position, newDirection, 100, ViewLayerMask);

        //Switches state
        if (Time.time > _timer) {
            _state = States.Searching;
            GameManager.Manager.Heard++;
            GameManager.Manager.Spotters--;
            transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Chasing;
            _timer = Time.time + SearchTime;
            _isRunning = false;
            _commentHandler.LosePlayerComments();
            return;
        }

        //Skips the Rest if the player isn't in range
        if (hit.transform.tag != "Player")
            return;

        _timer = Time.time + SearchTime;
        _enemyTransform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(newDirection.y, newDirection.x) * Mathf.Rad2Deg);

        //If he sees the player and is chasing him
        if ((GameManager.Manager.Player.transform.position - _enemyTransform.position).magnitude > ChaseRadius) {
            Movement(GameManager.Manager.Player.transform.position, true);
        }

    }

    //Looks around for the player, of he sees him then start following
    private void Searching() {
        //Looks at the Player's direction
        Vector2 newDirection = (_playerLastPos - _enemyTransform.position).normalized;
        var slowRotation = Mathf.LerpAngle(_enemyTransform.eulerAngles.z, Mathf.Atan2(newDirection.y, newDirection.x) * Mathf.Rad2Deg, 0.3f);
        _enemyTransform.rotation = Quaternion.Euler(0, 0, slowRotation);

        //Checks for the player
        if (DetectPlayer()) {
            transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Gun;
            _state = States.Chase;
            GameManager.Manager.Heard--;
            GameManager.Manager.Spotters++;
            _isRunning = true;
            _commentHandler.ChaseComments();
            return;
        }

        //If the player wasn't spotted in the amount of time
        if (_timer < Time.time) {
            transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Walking;
            GameManager.Manager.Heard--;
            _state = States.Patrol;
            _commentHandler.StopHearingComments();
        }
    }

    //Raycasts towards the player
    private bool DetectPlayer() {
        //Detects if the player is in front of the enemy
        RaycastHit2D hit = Physics2D.Raycast(_enemyTransform.position, _enemyTransform.right, ViewDistance, ViewLayerMask);
        if (hit.collider != null && hit.collider.tag == "Player")
            return true;

        //Detects if the player is far right
        hit = Physics2D.Raycast(_enemyTransform.position, (_enemyTransform.right + (_enemyTransform.up / ViewAngle)), ViewDistance, ViewLayerMask);
        if(hit.collider != null && hit.collider.tag == "Player")
            return true;

        //Detects if the player is far left
        hit = Physics2D.Raycast(_enemyTransform.position, (_enemyTransform.right - (_enemyTransform.up / ViewAngle)), ViewDistance, ViewLayerMask);
        if(hit.collider != null && hit.collider.tag == "Player")
            return true;

        //Detects if the player is mid-right
        hit = Physics2D.Raycast(_enemyTransform.position, (_enemyTransform.right + (_enemyTransform.up / ViewAngle / 2)), ViewDistance, ViewLayerMask);
        if(hit.collider != null && hit.collider.tag == "Player")
            return true;

        //Detects if the player is mid-left
        hit = Physics2D.Raycast(_enemyTransform.position, (_enemyTransform.right - (_enemyTransform.up / ViewAngle / 2)), ViewDistance, ViewLayerMask);
        if(hit.collider != null && hit.collider.tag == "Player")
            return true;

        //The player was not detected
        return false;
    }

    private void Patrol() {
        //Don't run if there is no positions to move to
        if (SearchPoint.Count == 0)
            return;

        //In range of Locator
        if(_inRange) {
            //Sets the location
            GetNextLocation();
            _commentHandler.IdleComments();
            _inRange = false;
        }

        //Breaks if there isn't a player or game manager in the scene so the game doesn't crash 
        if (GameManager.Manager == null || GameManager.Manager.Player == null)
            return;

        //Checks for the player
        var hit = Physics2D.Raycast(_enemyTransform.position, (_enemyTransform.position - GameManager.Manager.Player.transform.position).normalized, HearRadius);

        //If the enemy hears the player by getting to close. If the player is behind a wall, he will get alerted
        if ((_enemyTransform.position - GameManager.Manager.Player.transform.position).magnitude <= HearRadius) {
            _timer = Time.time + SearchTime;

            //Sight
            if(hit.collider != null && hit.collider.CompareTag("Player")) {
                //Stops the Enemy
                _rb2d.velocity = new Vector2(0, 0);
                _playerLastPos = GameManager.Manager.Player.transform.position;

                //Changes _state and sprite
                _state = States.Chase;
                GameManager.Manager.Spotters++;
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Gun;
                _commentHandler.ChaseComments();
                return;
            }

            if(GameManager.Manager.Player.GetComponent<Rigidbody2D>().velocity.magnitude > 0.1f) {
                //Stops the Enemy
                _rb2d.velocity = new Vector2(0, 0);
                _playerLastPos = GameManager.Manager.Player.transform.position;

                //Changes _state and sprite
                _state = States.Searching;
                GameManager.Manager.Heard++;
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Chasing;
                _commentHandler.HearComments();
            }
        }
    }

    private void Movement(Vector3 target, bool chasing = false) {
        //Look at target and moves towards it
        Vector2 newDirection = (target - _enemyTransform.position).normalized;

        //Moves the player and faces the direction
        _rb2d.velocity = newDirection * (_isRunning ? RunSpeed : Speed);
        var slowRotation = Mathf.LerpAngle(_enemyTransform.eulerAngles.z, Mathf.Atan2(newDirection.y, newDirection.x) * Mathf.Rad2Deg, 0.3f);
        _enemyTransform.rotation = Quaternion.Euler(0, 0, slowRotation);

        //Stops if it gets in range with its target
        if ((target - _enemyTransform.position).magnitude < 0.5f && !chasing) {
            _rb2d.velocity = Vector3.zero;
            _timer = Time.time + Random.Range(2f, StopTime);
            _inRange = true;
        }
    }

    //Makes sure the Values Wrap around and don't return the same number
    private void GetNextLocation() {
        //Goes forwards
        if ((_movingForward && Random.Range(0, 100) > 15) || _pathState == 0) {
            //If hasn't reached the end of the list
            if(SearchPoint[_currentPath].Count - 1 != _pathState)
                _pathState += 1;
            else {
                _movingForward = false;
                _pathState -= 1;
            }

            _target = SearchPoint[_currentPath][_pathState];
            return;
        }

        //Got back to the start of the room
        if(_pathState == 1) {
            _movingForward = true;
            _currentPath = Random.Range(0, SearchPoint.Count);
            _pathState = 0;

            _target = SearchPoint[_currentPath][_pathState];
            return;
        }


        //Goes backwards
        if(Random.Range(0, 100) > 20)
            _pathState -= 1;
        else
            _pathState += 1;

        _target = SearchPoint[_currentPath][_pathState];
    }

    //References all of the Children and adds them to the Vector3 List
    private void GetSearchPoints(Transform holder) {
        List<string> names = new List<string>();
        //Gets all of the positions and saves them
        for (int i = 0; i < holder.childCount; i++) {

            //If the path already exists then add on to it
            var index = holder.GetChild(i).name.Contains(" ") ? holder.GetChild(i).name.IndexOf(" ") : holder.GetChild(i).name.Length;
            string title = holder.GetChild(i).name.Substring(0, index);
            if (names.Contains(title)) {
                SearchPoint[names.IndexOf(title)].Add(holder.GetChild(i).position);
                continue;
            }

            //If that path doesn't exist then add a new one
            names.Add(title);
            SearchPoint.Add(new List<Vector3>{ holder.GetChild(i).position});

        }

        //Destroys the Object so it doesn't skew the information
        Destroy(holder.gameObject);
    }
}
