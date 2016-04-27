﻿using UnityEngine;

namespace Assets.MovementPrototype.Character.States.HoldAttackStates
{
    public abstract class BaseSwing : AnimatedState
    {
        const float speed = 2.5f;
        public int Damage { get; protected set; }
        public BaseSwing(CharFsm fsm) : base(fsm)
        {
            turnRate = 0f;
        }
        public override void Enter(string lastStateName, string nextStateName, float additionalDeltaTime, params object[] args)
        {
            base.Enter(lastStateName, nextStateName, additionalDeltaTime, args);
            Character.SwordTrail.Activate();
            Character.AttackCollider.enabled = true;
        }
        public override void Exit(string lastStateName, string nextStateName, float additionalDeltaTime, params object[] args)
        {
            base.Exit(lastStateName, nextStateName, additionalDeltaTime, args);
            Character.AttackCollider.enabled = false;
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            Character.Move(Transform.position + ((Transform.forward * speed) * Time.fixedDeltaTime));
        }
    }
}
