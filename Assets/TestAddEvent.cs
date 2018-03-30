#define EVENT_SMB

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAddEvent : MonoBehaviour
{
    public Animator otherAnimator;

    protected Animator _animator;

	// Use this for initialization
	void Start ()
    {
        _animator = GetComponent<Animator>();

        _animator.AddEvent("PutGlass", AnimationEventType.STATE_ENTER, PutGlassEntered);

        _animator.AddEvent("Idle", AnimationEventType.STATE_EXIT_TRANSITION_START, anim => { Debug.Log("State idle exit transition started at" + Time.time); });
        _animator.AddEvent("Idle", AnimationEventType.STATE_END, IdleEnd);

        //otherAnimator.AddEventSMB("PutGlass", AnimationEventType.STATE_END, OtherPutGlassEntered);

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
            _animator.SetTrigger("TestTrigger");

        if(Input.GetKeyDown(KeyCode.DownArrow))
            otherAnimator.SetTrigger("TestTrigger");
    }

    void IdleEnd(Animator anim)
    {
        Debug.Log("Idle exited on" + gameObject.name + " at " +Time.time );
    }

    void PutGlassEntered(Animator anim)
    {
        Debug.Log("Put Glass entered on " + gameObject.name + " at " + Time.time);
    }

    void OtherPutGlassEntered(Animator anim)
    {
        Debug.Log("Other PutGlass exited on " + gameObject.name + " at " + Time.time);
    }
}
