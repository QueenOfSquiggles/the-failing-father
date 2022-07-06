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
    [Export] public string saveName;
    [Export] public ActionTrigger actionTrigger = ActionTrigger.PRESSED;

    public override TickResult Tick(BehaviourTreeRoot root, Dictionary<string, object> blackboard, float deltaTime){
        blackboard.Remove(saveName);
        switch(actionTrigger){
            case ActionTrigger.PRESSED:
                blackboard[saveName] = Input.IsActionPressed(actionName);
                return TickResult.SUCCESS;
            case ActionTrigger.RELEASED:
                blackboard[saveName] = !Input.IsActionPressed(actionName);
                return TickResult.SUCCESS;
            case ActionTrigger.JUST_PRESSED:
                blackboard[saveName] = Input.IsActionJustPressed(actionName);
                return TickResult.SUCCESS;
            case ActionTrigger.JUST_RELEASED:
                blackboard[saveName] = Input.IsActionJustReleased(actionName);
                return TickResult.SUCCESS;                
            default:
                return TickResult.FAILURE;
        }
    }

}