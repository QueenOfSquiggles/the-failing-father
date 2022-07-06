using Godot;
using Godot.Collections;
using Newtonsoft.Json;
using System;

[Tool]
public class BTPlugin : EditorPlugin {

    /// <summary> A defintion for a custom types including a custom node name and an icon </summary>
    private struct Entry {
        public string name;
        public string script;
        public string texture;
        public Entry(string name, string script, string texture, string parent = "Node") {
            this.name = name;
            this.script = script;
            this.texture = texture;
        }
    }

    private readonly Entry[] moduleScriptEntries = new Entry[] {
        // Root
        new Entry("BT_Root", "res://addons/behaviour-tree/scripts/BehaviourTreeRoot.cs", "bt-root.svg"),
        // Compositors
        new Entry("BT_Fallback", "res://addons/behaviour-tree/scripts/composite/BehaviourTreeFallback.cs", "bt-fallback.svg"),
        new Entry("BT_RandomSequence", "res://addons/behaviour-tree/scripts/composite/BehaviourTreeRandomSequence.cs", "bt-composite.svg"),
        new Entry("BT_Sequence", "res://addons/behaviour-tree/scripts/composite/BehaviourTreeSequence.cs", "bt-sequence.svg"),
        // Decorators
        new Entry("BT_Inverter", "res://addons/behaviour-tree/scripts/decorator/BehaviourTreeInverter.cs", "bt-decorator.svg"),
        new Entry("BT_Limiter", "res://addons/behaviour-tree/scripts/decorator/BehaviourTreeLimiter.cs", "bt-decorator.svg"),
        new Entry("BT_Once", "res://addons/behaviour-tree/scripts/decorator/BehaviourTreeOnce.cs", "bt-decorator.svg"),
        new Entry("BT_Succeeder", "res://addons/behaviour-tree/scripts/decorator/BehaviourTreeSucceeder.cs", "bt-decorator.svg"),
        new Entry("BT_TimerLimiter", "res://addons/behaviour-tree/scripts/decorator/BehaviourTreeTimerLimiter.cs", "bt-decorator.svg"),
        new Entry("BT_Checkpoint", "res://addons/behaviour-tree/scripts/decorator/BehaviourTreeCheckpoint.cs", "bt-checkpoint.svg"),
        // Tasks 
        new Entry("BT_Task", "res://addons/behaviour-tree/scripts/task/BehaviourTreeTask.cs", "bt-task.svg"),
        new Entry("BT_Action", "res://addons/behaviour-tree/scripts/task/action/BehaviourTreeAction.cs", "bt-action.svg"),
        new Entry("BT_Query", "res://addons/behaviour-tree/scripts/task/query/BehaviourTreeQuery.cs", "bt-query.svg"),
        // Specific Actions
        new Entry("BT_LoadInputAction", "res://addons/behaviour-tree/scripts/task/action/BehaviourTreeLoadAction.cs", "bt-action.svg"),
        new Entry("BT_DebugPrint", "res://addons/behaviour-tree/scripts/task/action/BehaviourTreePrintDebugState.cs", "bt-action.svg"),
        // -    Load Actions
        new Entry("BT_LoadNode", "res://addons/behaviour-tree/scripts/task/action/BehaviourTreeLoadNode.cs", "bt-action.svg"),
        new Entry("BT_LoadFloat", "res://addons/behaviour-tree/scripts/task/action/BehaviourTreeLoadFloat.cs", "bt-action.svg"),
        new Entry("BT_LoadVec3", "res://addons/behaviour-tree/scripts/task/action/BehaviourTreeLoadVec3.cs", "bt-action.svg"),
        new Entry("BT_LoadVec2", "res://addons/behaviour-tree/scripts/task/action/BehaviourTreeLoadVec2.cs", "bt-action.svg"),
        new Entry("BT_LoadBool", "res://addons/behaviour-tree/scripts/task/action/BehaviourTreeLoadBool.cs", "bt-action.svg"),
        new Entry("BT_LoadInt", "res://addons/behaviour-tree/scripts/task/action/BehaviourTreeLoadInt.cs", "bt-action.svg"),
        //  -    Animation Actions
        new Entry("BT_PlayAnimation", "res://addons/behaviour-tree/scripts/task/action/BehaviourTreePlayAnimation.cs", "bt-action.svg"),
        new Entry("BT_AnimTree_SetParamFloat", "res://addons/behaviour-tree/scripts/task/action/BehaviourTreeAnimTreeParamFloat.cs", "bt-action.svg"),
        new Entry("BT_AnimTree_StateMachineTravel", "res://addons/behaviour-tree/scripts/task/action/BehaviourTreeAnimTreeStateMachineTravel.cs", "bt-action.svg"),
        // Specific Queries
        new Entry("BT_QueryInputAction", "res://addons/behaviour-tree/scripts/task/query/BehaviourTreeQueryInputAction.cs", "bt-query.svg"),
        // Logic Gate Compositors
        // res://addons/behaviour-tree/scripts/composite/logic/BehaviourTreeLogic_AND.cs
        new Entry("BT_Logic_AND", "res://addons/behaviour-tree/scripts/composite/logic/BehaviourTreeLogic_AND.cs", "bt-logic-and.svg"),
        new Entry("BT_Logic_OR", "res://addons/behaviour-tree/scripts/composite/logic/BehaviourTreeLogic_OR.cs", "bt-logic-or.svg"),
        new Entry("BT_Logic_NAND", "res://addons/behaviour-tree/scripts/composite/logic/BehaviourTreeLogic_NAND.cs", "bt-logic-nand.svg"),
        new Entry("BT_Logic_NOR", "res://addons/behaviour-tree/scripts/composite/logic/BehaviourTreeLogic_NOR.cs", "bt-logic-nor.svg"),
        new Entry("BT_Logic_XOR", "res://addons/behaviour-tree/scripts/composite/logic/BehaviourTreeLogic_XOR.cs", "bt-logic-xor.svg"),
        new Entry("BT_Logic_XNOR", "res://addons/behaviour-tree/scripts/composite/logic/BehaviourTreeLogic_XNOR.cs", "bt-logic-xnor.svg"),
    };

    public override void _EnterTree() {
        foreach (Entry e in moduleScriptEntries){
            var texture = GD.Load<Texture>("res://addons/behaviour-tree/icons/" + e.texture);
            var script = GD.Load<Script>(e.script);
            this.AddCustomType(e.name, "Node", script, texture);
        }
    }

    public override void _ExitTree() {
        foreach(Entry e in moduleScriptEntries){
            RemoveCustomType(e.name);
        }
    }
}
