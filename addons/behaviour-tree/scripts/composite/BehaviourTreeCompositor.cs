using System;
using System.Collections.Generic;
using Godot;

public class BehaviourTreeCompositor : BehaviourTreeNode
{
    public BehaviourTreeCompositor() : base(-1, 1) {}

        protected virtual TickResult ProcessChildren(BehaviourTreeRoot root, Dictionary<string, object> blackboard, float deltaTime, Node[] children){
            throw new NotImplementedException("This class should not be instanced!");
        }

}