using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(9999)]
public class AnimatorPolling : MonoBehaviour
{
    Animator _animator;

    bool[] _wasInTransition;
    AnimatorStateInfo[] _currentState;

    Dictionary<int, System.Action<Animator>> _onStateEnterClbk = new Dictionary<int, System.Action<Animator>>();
    Dictionary<int, System.Action<Animator>> _onStateExitClbk = new Dictionary<int, System.Action<Animator>>();
    Dictionary<int, System.Action<Animator>> _onStateEnterTransitionEndClbk = new Dictionary<int, System.Action<Animator>>();
    Dictionary<int, System.Action<Animator>> _onStateExitTransitionStartClbk = new Dictionary<int, System.Action<Animator>>();

    void OnEnable()
    {
        _animator = GetComponent<Animator>();

        _wasInTransition = new bool[_animator.layerCount];
        for (int i = 0; i < _wasInTransition.Length; ++i)
            _wasInTransition[i] = _animator.IsInTransition(i);

        _currentState = new AnimatorStateInfo[_animator.layerCount];
        for (int i = 0; i < _currentState.Length; ++i)
            _currentState[i] = _animator.GetCurrentAnimatorStateInfo(i);
    }

    private void FixedUpdate()
    {
        for(int i = 0; i < _animator.layerCount; ++i)
        {
            var stateInfo = _animator.GetCurrentAnimatorStateInfo(i);
            bool inTransition = _animator.IsInTransition(i);

            if(stateInfo.fullPathHash != _currentState[i].fullPathHash)
            {//we changed state, call the on stateExit on the previous state
                if (_onStateExitClbk.ContainsKey(_currentState[i].fullPathHash))
                    _onStateExitClbk[_currentState[i].fullPathHash].Invoke(_animator);

                if(!_wasInTransition[i])
                {//no need to call on enter on new state if we were in transition, this will have been called already
                    if (_onStateEnterClbk.ContainsKey(stateInfo.fullPathHash))
                        _onStateEnterClbk[stateInfo.fullPathHash].Invoke(_animator);
                }
                else
                {//we need to notify the new state that its entry transition is finished
                    if (_onStateEnterTransitionEndClbk.ContainsKey(stateInfo.fullPathHash))
                        _onStateEnterTransitionEndClbk[stateInfo.fullPathHash].Invoke(_animator);
                }
            }
            else
            {//the same state is still going, now check if the transition state Changed
                if(!_wasInTransition[i] && inTransition)
                {//we just entered a transition, notify the current state that its exit transition started
                    if (_onStateExitTransitionStartClbk.ContainsKey(_currentState[i].fullPathHash))
                        _onStateExitTransitionStartClbk[_currentState[i].fullPathHash].Invoke(_animator);

                    //we also notify the enter event of the next state
                    var nextState = _animator.GetNextAnimatorStateInfo(i);

                    if (_onStateEnterClbk.ContainsKey(nextState.fullPathHash))
                        _onStateEnterClbk[nextState.fullPathHash].Invoke(_animator);
                }
            } 


            _currentState[i] = stateInfo;
            _wasInTransition[i] = inTransition;
        }
    }


    public void AddEvent(string stateName, AnimationEventType eventType, System.Action<Animator> callback, int layer = 0)
    {
        string layerName = _animator.GetLayerName(layer);
        int fullnameHash = Animator.StringToHash(layerName + "." + stateName);

        switch (eventType)
        {
            case AnimationEventType.STATE_ENTER:
                if (!_onStateEnterClbk.ContainsKey(fullnameHash)) _onStateEnterClbk.Add(fullnameHash, new System.Action<Animator>(callback));
                else _onStateEnterClbk[fullnameHash] += callback;
                break;
            case AnimationEventType.STATE_END:
                if (!_onStateExitClbk.ContainsKey(fullnameHash)) _onStateExitClbk.Add(fullnameHash, new System.Action<Animator>(callback));
                else _onStateExitClbk[fullnameHash] += callback;
                break;
            case AnimationEventType.STATE_ENTER_TRANSITION_END:
                if (!_onStateEnterTransitionEndClbk.ContainsKey(fullnameHash)) _onStateEnterTransitionEndClbk.Add(fullnameHash, new System.Action<Animator>(callback));
                else _onStateEnterTransitionEndClbk[fullnameHash] += callback;
                break;
            case AnimationEventType.STATE_EXIT_TRANSITION_START:
                if (!_onStateExitTransitionStartClbk.ContainsKey(fullnameHash)) _onStateExitTransitionStartClbk.Add(fullnameHash, new System.Action<Animator>(callback));
                else _onStateExitTransitionStartClbk[fullnameHash] += callback;
                break;
            default:
                break;
        }
    }
}
