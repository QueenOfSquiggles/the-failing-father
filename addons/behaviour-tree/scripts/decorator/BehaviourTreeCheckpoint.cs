using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// A decorator node that processes the checkpoint system. Use this as a root for any sub-tree that would likely dominate control of the tree for an extended period of time
/// The use of checkpoints can model a state-machine pattern using the power of behaviour trees. Additionally, it allows the AI to skip over sections of the entire tree should it become very dense.
/// </summary>
public class BehaviourTreeCheckpoint : BehaviourTreeDecorator {
    public override TickResult Tick(BehaviourTreeRoot root, Dictionary<string, object> blackboard, float deltaTime) {
        var result = GetChild<BehaviourTreeNode>(0).Tick(root, blackboard, deltaTime);
        if (result == TickResult.SUCCESS){
            // set checkpoint
            root.SetCheckpoint(this);
        }else if (result == TickResult.FAILURE){
            // unset checkpoint
            root.SetCheckpoint(null);
        }
        return result;
    }
}