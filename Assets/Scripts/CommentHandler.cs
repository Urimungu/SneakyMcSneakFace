using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine.UI;
using UnityEngine;

public class CommentHandler : MonoBehaviour
{
    //Variables
    public Text Text;
    public Canvas CanvasHolder;
    public float DisplayTime = 3;

    //Messages
    readonly List<string> _idleComments = new List<string> {
        "This job is boring", "Why won't something interesting happen?", "Is cheese just the corpse of milk?", "Potatoes get soft in hot water but eggs get hard",
        "I hate my life ngl", "Where is my tortilla?", "This program is broken.", "Wow, the same locations again!!", "Uggh!! These people have nothing valuable anyway",
        "Imagine being a thief, HA! What a loser.", "Elijah! Where did you go?"
    };
    readonly List<string> _hearComments = new List<string> {
        "What was that?!", "Huh!", "I heard something, hopefully it goes away.", "Footsteps!", "Is that the ghost?", "If this place is haunted, Im throwing hands",
        "If there is a thief, I am programmed to shoot on sight.", "That noise sounded ugly", "Ughh! Really?", "Hmmm"

    };
    readonly List<string> _chaseComments = new List<string> {
        "I will kill you!", "I'mma slide your grandma!", "Who are you?", "Why you gotta make me do my job?", "What are you doing here?", "Wait! I know you!",
        "I'mma clap your cheeks!", "Finally, I get to use my gun!", "Eww, he's ugly!", "As long as it's not Elijah!", "I know where you live!", "Your face is repelling!"
    };
    readonly List<string> _losePlayerComments = new List<string> {
        "Pfft.. whatever.", "Coward.", "I ain't finna chase his ass ngl.", "Get back here! I'm not programmed to chase you!", "Who cares lol", "Too ugly to chase",
        "Terrible pursuit, 0/10", "Thank god, he went away", "Not my problem anymore.", "I hope he didn't take anything", "Whatever!", "Who the fuck was that?"
    };

    readonly List<string> _stopHearingComments = new List<string> {
        "Must have been the wind", "Not my problem anymore.", "I don't really care what it was.", "Floor boards?", "I hope it's not that thief I saw earlier!",
        "Thank God, I didn't wanna check that out.", "Who cares? Not me. I don't care.", "I hope that wasn't a ghost. I hate ghosts."

    };

    //Functions
    private void Start() {
        Text = transform.Find("Canvas").GetChild(0).GetComponent<Text>();
        CanvasHolder = transform.Find("Canvas").GetComponent<Canvas>();
        Text.text = "";

    }

    private void Update() {
        CanvasHolder.transform.rotation = Quaternion.identity;
    }

    public void IdleComments() {
        Text.text = _idleComments[Random.Range(0, _idleComments.Count)];
        StopAllCoroutines();
        StartCoroutine(stopMessage());
    }
    public void HearComments() {
        Text.text = _hearComments[Random.Range(0, _hearComments.Count)];
        StopAllCoroutines();
        StartCoroutine(stopMessage());
    }
    public void ChaseComments() {
        Text.text = _chaseComments[Random.Range(0, _chaseComments.Count)];
        StopAllCoroutines();
        StartCoroutine(stopMessage());
    }
    public void LosePlayerComments() {
        Text.text = _losePlayerComments[Random.Range(0, _losePlayerComments.Count)];
        StopAllCoroutines();
        StartCoroutine(stopMessage());
    }

    public void StopHearingComments() {
        Text.text = _stopHearingComments[Random.Range(0, _stopHearingComments.Count)];
        StopAllCoroutines();
        StartCoroutine(stopMessage());
    }

    IEnumerator stopMessage() {
        yield return new WaitForSeconds(DisplayTime);
        Text.text = "";
    }
}
