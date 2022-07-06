using System;
using System.Collections.Generic;
using Godot;

public class BehaviourTreeRoot : BehaviourTreeNode
{

    public enum TickMode {
        PROCESS, PHYSICS, MANUAL
    }

    [Export] public TickMode tickMode = TickMode.PROCESS;
    [Export] public int maxTicksBeforeReevaluation = -1;
    [Export] public float maxTimeBeforeReevaluation = -1f;
    [Export] public int randomSeed = 0;

    public Random random = new Random();

    private Dictionary<string, object> blackboard = new Dictionary<string, object>();
    private BehaviourTreeNode checkpoint = null;
    private int tickCount = 0;
    private float timer = 0f;

    public BehaviourTreeRoot() : base(1, 1) {}

    public override void _Ready() {
        base._Ready();
        this.SetProcess(tickMode == TickMode.PROCESS);
        this.SetPhysicsProcess(tickMode == TickMode.PHYSICS);
        if(randomSeed != 0) random = new Random(randomSeed);
    }

    public override void _Process(float delta) {
        /// Process when in the PROCESS tick mode
        Tick(this, blackboard, delta);
    }

    public override void _PhysicsProcess(float delta) {
        /// Process when in the PHYSICS tick mode
        Tick(this, blackboard, delta);
    }

    public void ManualTick(float deltaTime){
        /// Process when in the MANUAL tick mode. Must be called manually
        Tick(this, blackboard, deltaTime);
    }

    public void SetCheckpoint(BehaviourTreeNode n_checkpoint){
        checkpoint = n_checkpoint;
    }

    public override TickResult Tick(BehaviourTreeRoot root, Dictionary<string, object> blackboard, float deltaTime) {
        if (maxTicksBeforeReevaluation > 0){
            if (tickCount++ >= maxTicksBeforeReevaluation){
                tickCount = 0;
                checkpoint = null;
            }
        }
        if (maxTimeBeforeReevaluation > 0){
            timer += deltaTime;
            if(timer > maxTimeBeforeReevaluation){
                do timer -= maxTimeBeforeReevaluation; // should only run once most of the time.
                while(timer > maxTimeBeforeReevaluation);
                checkpoint = null;
            }
        }
        if (checkpoint == null) checkpoint = GetChild<BehaviourTreeNode>(0);
        checkpoint.Tick(root, blackboard, deltaTime); // tick off of the checkpoint
        return TickResult.SUCCESS;
    }
}
