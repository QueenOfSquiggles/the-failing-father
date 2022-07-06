using System;
using System.Collections.Generic;
using Godot;

public class BehaviourTreeQueryInputAction : BehaviourTreeQuery {
    // only queries the input without saving the result as a data value.
    // basically this should be used instead of the LoadAction when all you want to know is whether or not something is pressed
    public enum ActionTrigger {
        PRESSED, RELEASED, JUST_PRESSED, JUST_RELEASED
    }

    [Export] public string actionName;
    [Export] public ActionTrigger actionTrigger = ActionTrigger.PRESSED;

    public override TickResult Tick(BehaviourTreeRoot root, Dictionary<string, object> blackboard, float deltaTime){
        switch(actionTrigger){
            case ActionTrigger.PRESSED:
                return (Input.IsActionPressed(actionName)) ? TickResult.SUCCESS : TickResult.FAILURE;

            case ActionTrigger.RELEASED:
                return (!Input.IsActionPressed(actionName)) ? TickResult.SUCCESS : TickResult.FAILURE;

            case ActionTrigger.JUST_PRESSED:
                return (Input.IsActionJustPressed(actionName)) ? TickResult.SUCCESS : TickResult.FAILURE;

            case ActionTrigger.JUST_RELEASED:
                return (Input.IsActionJustReleased(actionName)) ? TickResult.SUCCESS : TickResult.FAILURE;

            default:
                return TickResult.FAILURE;
        }
    }

}