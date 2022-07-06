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
        // Specific Tasks
        new Entry("BT_LoadInputAction", "res://addons/behaviour-tree/scripts/task/action/BehaviourTreeLoadAction.cs", "bt-action.svg"),
        new Entry("BT_QueryInputAction", "res://addons/behaviour-tree/scripts/task/query/BehaviourTreeQueryInputAction.cs", "bt-query.svg"),
        new Entry("BT_DebugPrint", "res://addons/behaviour-tree/scripts/task/action/BehaviourTreePrintDebugState.cs", "bt-action.svg"),
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
