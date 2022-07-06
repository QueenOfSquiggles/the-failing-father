using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;

/// Not a useful distinction, just allows for distinction against query nodes
public class BehaviourTreeLoadFloat : BehaviourTreeTask {

    [Export] public float value = 0.0f;
    [Export] public string saveName = "value";

    public override TickResult Tick(BehaviourTreeRoot root, Dictionary<string, object> blackboard, float deltaTime){
        blackboard[saveName] = value;
        return TickResult.SUCCESS;
    }

}
