using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;

/// Not a useful distinction, just allows for distinction against query nodes
public class BehaviourTreeLoadVec3 : BehaviourTreeTask {

    [Export] public Vector3 value = Vector3.Zero;
    [Export] public string saveName = "value";

    public override TickResult Tick(BehaviourTreeRoot root, Dictionary<string, object> blackboard, float deltaTime){
        blackboard[saveName] = value;
        return TickResult.SUCCESS;
    }

}
