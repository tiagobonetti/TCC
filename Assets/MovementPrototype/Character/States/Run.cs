﻿using UnityEngine;

namespace Assets.MovementPrototype.Character.States
{
    public class Run : CMovement
    {
        float moveSpeed = 6f;

        public Run(CFsm fsm) : base(fsm)
        {
            Name = "RUN";
        }

        public override void Enter(string lastStateName, string nextStateName, float additionalDeltaTime, params object[] args)
        {
            base.Enter(lastStateName, nextStateName, additionalDeltaTime, args);
        }

        public override void PreUpdate()
        {
            if (Character.input.run <= 0.25f)
            {
                Character.fsm.ChangeState("LOCK");
            }
            else if (Input.buffer.NextEventIs<InputEvent.Attack>())
            {
                var evt = Input.buffer.Pop<InputEvent.Attack>();
                Fsm.ChangeState("ATTACK", 0f, evt);
            }
            else if (Input.buffer.NextEventIs<InputEvent.Block>())
            {
                Input.buffer.Pop<InputEvent.Block>();
                Fsm.ChangeState("BLOCK/WINDUP");
            }
            else if (Input.buffer.NextEventIs<InputEvent.Dash>())
            {
                var evt = Input.buffer.Pop<InputEvent.Dash>();
                Fsm.ChangeState("DASH", 0f, evt);
            }
        }

        public override void FixedUpdate()
        {
            if (Character.input.move.vector.magnitude > 0.25)
            {
                Character.LookForward(lookTurnRate);
            }
            Move(moveSpeed);
        }
    }
}