# UnityAnimatorEvent

Animator extension that allow to add event on state enter/exit and transition start/end through scripts

You can copy the content of the AnimatorEvent folder inside your project to use it.

This repo contains 2 prototype of how to do this (see Methods further down)

## Usage

To add an event, just call on your animator

```csharp
animator.AddEvent("stateName", AnimationEventType.STATE_ENTER, callback, layer);
``` 

**layer** is optional, if you ommit it, layer 0 (Base Layer) is used.

Examples :

```csharp
animator.AddEvent("Walk", AnimationEventType.STATE_ENTER, WalkEntered, animator.GetLayerIndex("LowerBody"));
animator.AddEvent("Idle", AnimationEventType.STATE_EXIT_TRANSITION_START, anim => { Debug.Log("State idle exit transition started at" + Time.time); });
``` 

### Switching methods

To switch the method the system use, change the boolean `isUsingSMBMethod` in `AnimatorAnimationEventExtension` to :
- **false** to use **Method 1**
- **true** to use **Method 2**

note that switching to method 2 will require to resave your animator for now (just move a state and save the project). 

## Method 1 : Through monitoring the animator state

This method monitor if the state and transition infos changed change since last frame and and call the appropriates callbacks
This has the advantage of not modifying your assets, and just add a simple script on the monitored object at runtime.

### Drawback

This is less precise, the callback won't be called exactly when the state change as we are working in FixedUpdate function and not
being notified by Unity internal Animator messaging system.

It probably is slightly more computanional heavy, as we have to poll the animator lookup into dictionaries for callback for each states etc. 
 
Also depending on when the check occur it could occur before the StateMachine change state in the frame, leading to a frame lag before the callback call (_probably
can be alleviated by using DefaultExectionOrder attribute_)


## Method 2 : Through automatically added StateMachineBehaviour 

This method automatically add a special StateMachineBehaviour called `CallbackSMB` to all the state
of your AnimatorController when it is saved on disk.

**/!\\ THIS MEAN IT MODIFY YOUR ASSETS FOR YOU /!\\** 

This is the most robust as it rely on the Unity internal implementation to get when state are entered/existed

### Drawback 

The drawback is that it will modify your asset, adding a StateMachineBehaviour on all state, as there is no
way to add behaviour at runtime.

That also mean that state machine created before adding the plugin need to be resaved.
(_TODO : auto resave all existing StateMachineBehaviour on plugin first import? with confirmation popup_)

