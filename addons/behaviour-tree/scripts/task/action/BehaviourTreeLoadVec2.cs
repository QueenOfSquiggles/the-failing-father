using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;

/// Not a useful distinction, just allows for distinction against query nodes
public class BehaviourTreeLoadVec2 : BehaviourTreeTask {

    [Export] public Vector2 value = Vector2.Zero;
    [Export] public string saveName = "value";

    public override TickResult Tick(BehaviourTreeRoot root, Dictionary<string, object> blackboard, float deltaTime){
        blackboard[saveName] = value;
        return TickResult.SUCCESS;
    }

}
