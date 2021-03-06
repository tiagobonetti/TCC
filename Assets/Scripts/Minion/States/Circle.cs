﻿using UnityEngine;

namespace Assets.Scripts.Minion.States
{
    public class Circle : MinionState
    {
        const float angleStep = 30f * Mathf.Deg2Rad; // as radians 
        const float spiralStep = 0.8f; // close in by (spiralStep * Distance)

        TimerBehaviour timer;
        public Circle(MinionFsm fsm) : base(fsm)
        {
            Name = "CIRCLE";
            timer = new TimerBehaviour(this);
            timer.TotalTime = 1f;
            timer.OnFinish = () => NextState();
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            UpdateDestination();
            Look();
        }
        void UpdateDestination()
        {
            var relativePos = (Transform.position - Target.Transform.position); // minion position relative to target 
            if (Vector3.Dot(relativePos.normalized, Target.Transform.forward) < -0.9f)
            {
                // When dot < -0.9 minion is behind target so just follow;
                Minion.NavmeshMove(Target.Transform.position, updateRotation: false);
            }
            else
            {
                // rotate the relativePos around the target towards it's back 
                var destination = Vector3.RotateTowards(relativePos, Target.Transform.forward, -angleStep, 0f);
                destination = destination * spiralStep;   // close in as we go around
                destination += Target.Transform.position; // get the global destination
                Minion.NavmeshMove(destination, updateRotation: false);
            }
        }
        public override void Enter(string lastStateName, string nextStateName, float additionalDeltaTime = 0, params object[] args)
        {
            base.Enter(lastStateName, nextStateName, additionalDeltaTime, args);
            Animator.CrossFade("MoveLow", 0.3f, -1, 0);
        }
        public override void Exit(string lastStateName, string nextStateName, float additionalDeltaTime, params object[] args)
        {
            base.Exit(lastStateName, nextStateName, additionalDeltaTime, args);
            Minion.Stop();
        }
    }
}