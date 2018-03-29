# UnityAnimatorEvent
Animator extension that allow to add event on state enter/exit through script

Two prototype are on this repo

## Method 1 : Through automatically added StateMachineBehaviour 

This method automatically add a special StateMachineBehaviour called `CallbackSMB` to all the state
of your AnimatorController when it is saved on disk.

This is the most robust as it rely on the Unity internal implementation to get when state are entered/existed

To use it, add an event through :

`animator.AddEventSMB("StateName", AnimationEventType.STATE_ENTER/STATE_EXIT, CallbackFunc, layer)`

layer is optional, if not specified, layer 0 (Base Layer) is used.

### Drawback 

The drawback is that it will modify your asset, adding a StateMachineBehaviour on all state, as there is no
way to add behaviour at runtime.

That also mean that state machine created before adding the plugin need to be resaved.
(_TODO : auto resave all existing StateMachineBehaviour on plugin first import? with confirmation popup_)

## Method 2 : Through monitoring the animator state

The second method monitor if GetCurrentAnimatorStateInfo change since last frame and use those info to call the callback
This has the advantage of not modifying your asset, 

**STILL TODO, NOT ON THE REPO YET**

### Drawback

This is less precise, the callback won't be called exactly when the state change as we are working in Update/FixedUpdate function and not
being notified by Unity internal Animator messaging system.
 
Also depending on when the check occur it could occur before the StateMachine change state in the frame, leading to a frame lag before the callback call (_probably
can be alleviated by using DefaultExectionOrder attribute_) 