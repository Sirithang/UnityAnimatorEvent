using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallbackSMB : StateMachineBehaviour
{
    public System.Action<Animator> enterAction;
    public System.Action<Animator> exitAction;
    public System.Action<Animator> startTransition;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enterAction?.Invoke(animator);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        exitAction?.Invoke(animator);
    }
}
