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

        _animator.AddEventSMB("PutGlass", AnimationEventType.STATE_ENTER, PutGlassEntered);
        otherAnimator.AddEventSMB("PutGlass", AnimationEventType.STATE_END, OtherPutGlassEntered);

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
            _animator.SetTrigger("TestTrigger");

        if(Input.GetKeyDown(KeyCode.DownArrow))
            otherAnimator.SetTrigger("TestTrigger");
    }

    void PutGlassEntered(Animator anim)
    {
        Debug.Log($"Put Glass entered on {gameObject.name}");
    }

    void OtherPutGlassEntered(Animator anim)
    {
        Debug.Log($"Other PutGlass entered on {gameObject.name}");
    }
}
