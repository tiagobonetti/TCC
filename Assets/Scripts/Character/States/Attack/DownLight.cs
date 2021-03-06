﻿using Assets.Scripts.Common;

namespace Assets.Scripts.Character.States.Attack
{
    public class DownLightWindUp : BaseWindUp
    {
        private bool holding;
        public DownLightWindUp(CharacterFsm fsm) : base(fsm)
        {
            Name = "DOWN/LIGHT/WINDUP";

            timer.TotalTime = 0.25f;
            timer.OnFinish = () => Fsm.ChangeState(holding ? "DOWN/HEAVY/WINDUP" : "DOWN/LIGHT/SWING");

            animation.TotalTime = 0.6f;
            animation.PlayTime = 0.65f;
            animation.Name = "AttackVerticalWindup";
        }
        public override void PreUpdate()
        {
            base.PreUpdate();
            if (holding && Character.input.attack == false)
            {
                holding = false;
            }
        }
        public override void Enter(string lastStateName, string nextStateName, float additionalDeltaTime, params object[] args)
        {
            base.Enter(lastStateName, nextStateName, additionalDeltaTime, args);
            holding = true;
        }
        public override void Exit(string lastStateName, string nextStateName, float additionalDeltaTime = 0, params object[] args)
        {
            base.Exit(lastStateName, nextStateName, additionalDeltaTime, args);
            // if (holding)
            // {
            //    TODO: Add visual feedback  
            //    UnityEngine.Debug.Log("Promoted to Heavy!");
            // }

        }
    }
    public class DownLightSwing : BaseSwing
    {
        public DownLightSwing(CharacterFsm fsm) : base(fsm)
        {
            Name = "DOWN/LIGHT/SWING";

            timer.TotalTime = 0.2f;
            timer.OnFinish = () => Fsm.ChangeState("DOWN/LIGHT/RECOVER");

            animation.TotalTime = 0.3f;
            animation.PlayTime = 0.2f;
            animation.Name = "AttackVerticalSwing";

            Displacement = 0.8f;

            Damage = 1;
            Direction = AttackDirection.Vertical;
            IsHeavy = false;
        }
    }
    public class DownLightRecover : BaseRecover
    {
        public DownLightRecover(CharacterFsm fsm) : base(fsm)
        {
            Name = "DOWN/LIGHT/RECOVER";

            timer.TotalTime = 0.2f;
            timer.OnFinish = () => Fsm.ChangeState("MOVEMENT");

            animation.TotalTime = 0.3f;
            animation.PlayTime = 0.2f;
            animation.Name = "AttackVerticalRecover";
        }
    }
}

