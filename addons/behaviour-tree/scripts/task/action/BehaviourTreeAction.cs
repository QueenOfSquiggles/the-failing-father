using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/// Not a useful distinction, just allows for distinction against query nodes
public class BehaviourTreeAction : BehaviourTreeTask {

    public override TickResult Tick(BehaviourTreeRoot root, Dictionary<string, object> blackboard, float deltaTime){
        throw new NotImplementedException("This is an effectively abstract class! Do not use it!");
    }

}
