using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallbackSMB : StateMachineBehaviour
{
    public System.Action<Animator> enterAction;
    public System.Action<Animator> exitAction;
    public System.Action<Animator> startTransition;
    public System.Action<Animator> exitTransition;

    protected bool _previousTransitionState;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(enterAction != null) enterAction.Invoke(animator);
        _previousTransitionState = animator.IsInTransition(layerIndex);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bool inTransition = animator.IsInTransition(layerIndex);

        if (inTransition)
        {
            if (!_previousTransitionState)
            {
                //we've just entered a transition, mean we are exiting
                if(exitTransition != null) exitTransition.Invoke(animator);
            }
        }
        else
        {
            if(_previousTransitionState)
            {//we just exited a transition, so the enter transition is finished
                if(startTransition != null) exitTransition.Invoke(animator);
            }
        }

        _previousTransitionState = inTransition;
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(exitAction != null) exitAction.Invoke(animator);
    }
}
