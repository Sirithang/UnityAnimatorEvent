﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Animations;
#endif

public static class AnimatorAnimationEventExtension
{
    public static void AddEventSMB(this Animator animator, string stateName, AnimationEventType eventType, System.Action<Animator> callback, int layer = 0)
    {
        var layerName = animator.GetLayerName(layer);
        var callbackSMB = animator.GetBehaviours(Animator.StringToHash(layerName+"."+stateName), 0);

        bool found = false;
        foreach (var smb in callbackSMB)
        {
            CallbackSMB clbk = smb as CallbackSMB;

            if (clbk.GetType() != typeof(CallbackSMB))
                continue;

            found = true;

            switch (eventType)
            {
                case AnimationEventType.STATE_ENTER:
                    clbk.enterAction += callback;
                    break;
                case AnimationEventType.STATE_END:
                    clbk.exitAction += callback;
                    break;
                default: break;
            }
        }

        if(!found)
        {
            Debug.LogError($"This animator does not have a state called {stateName}", animator);
        }
    }
}

public enum AnimationEventType
{
    STATE_ENTER,
    STATE_END
}

#if UNITY_EDITOR
public class AddingEventSNBToAnimator : UnityEditor.AssetModificationProcessor
{
    static string[] OnWillSaveAssets(string[] paths)
    {
        for (int i = 0; i < paths.Length; ++i)
        {
            var asset = AssetDatabase.LoadAssetAtPath<AnimatorController>(paths[i]);

            if(asset != null)
            {
                foreach(var layer in asset.layers)
                {
                    CheckStateMachine(layer.stateMachine);
                }
            }
        }

        return paths;
    }

    static void CheckStateMachine(AnimatorStateMachine stateMachine)
    {
        foreach (var childStateMachine in stateMachine.stateMachines)
            CheckStateMachine(childStateMachine.stateMachine);

       CheckState(stateMachine.states);
    }

    static void CheckState(ChildAnimatorState[] states)
    {
        foreach(var state in states)
        {
            bool needBehaviour = true;
            foreach (var smb in state.state.behaviours)
            {
                if(smb.GetType() == typeof(CallbackSMB))
                {
                    needBehaviour = false;
                    break;
                }
            }

            if (needBehaviour)
                state.state.AddStateMachineBehaviour<CallbackSMB>();
        }
    }
}
#endif