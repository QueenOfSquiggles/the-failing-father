using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

public class BehaviourTreeRandomSequence : BehaviourTreeSequence {

    public override TickResult Tick(BehaviourTreeRoot root, Dictionary<string, object> blackboard, float deltaTime) {
        var children = GetChildren();
        var shuffled = GetChildrenAsArray().OrderBy(_ => root.random.Next()).ToArray<Node>();
        return base.ProcessChildren(root, blackboard, deltaTime, shuffled);
    }
}
