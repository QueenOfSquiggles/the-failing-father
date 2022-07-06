using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;

public class BehaviourTreeLoadAction : BehaviourTreeAction {
    // load in action input, fairly versatile because there's a couple different kinds of inputs to load

    public enum LoadType{
        BOOL, FLOAT, VECTOR
    }

    public enum ActionTrigger {
        VALUE,PRESSED, RELEASED, JUST_PRESSED, JUST_RELEASED
    }

    [Export] public LoadType loadDataType = LoadType.BOOL;
    [Export] public ActionTrigger activationActionTrigger = ActionTrigger.PRESSED;
    [Export] public string actionName = "";
    [Export] public string saveName = "";

    public override TickResult Tick(BehaviourTreeRoot root, Dictionary<string, object> blackboard, float deltaTime){
        switch(activationActionTrigger){
            case ActionTrigger.VALUE:
                switch(loadDataType){
                    case LoadType.BOOL:
                        return TickResult.FAILURE; // INVALID COMBO
                    case LoadType.FLOAT:
                        var strength = Input.GetActionStrength(actionName);
                        blackboard[saveName] = strength;
                        return TickResult.SUCCESS;
                    case LoadType.VECTOR:
                        var axesActions = actionName.Split(';');
                        if(axesActions.Length != 4) return TickResult.FAILURE; // needs 4 axes
                        var direction = Input.GetVector(axesActions[0],axesActions[1],axesActions[2],axesActions[3]);
                        blackboard.Remove(saveName); // clear out in case of data type change
                        blackboard[saveName] = direction;
                        return TickResult.SUCCESS;
                }
                return TickResult.FAILURE; // This should not be reached
            case ActionTrigger.PRESSED:
                var isPressed = Input.IsActionPressed(actionName);
                blackboard.Remove(saveName);
                blackboard[saveName] = isPressed;
                return TickResult.SUCCESS;
            case ActionTrigger.RELEASED:
                var isReleased = Input.IsActionPressed(actionName);
                blackboard.Remove(saveName);
                blackboard[saveName] = isReleased;
                return TickResult.SUCCESS;
            case ActionTrigger.JUST_PRESSED:
                var isJustPressed = Input.IsActionPressed(actionName);
                blackboard.Remove(saveName);
                blackboard[saveName] = isJustPressed;
                return TickResult.SUCCESS;
            case ActionTrigger.JUST_RELEASED:
                var isJustReleased = Input.IsActionPressed(actionName);
                blackboard.Remove(saveName);
                blackboard[saveName] = isJustReleased;
                return TickResult.SUCCESS;
        }

        return TickResult.SUCCESS;
    }


    

}