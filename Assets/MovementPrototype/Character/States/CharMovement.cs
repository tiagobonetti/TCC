﻿using UnityEngine;

namespace Assets.MovementPrototype.Character.States
{
    public class CharMovement : CharState
    {
        public const float minVelocity = 0.25f;
        public float maxAcceleration = 2f;
        public float lookTurnRate = 1f;
        public float lockTurnRate = 1f;
        public Vector3 velocity;

        public CharMovement(CharFsm fsm) : base(fsm)
        {
            Name = "MOVEMENT";
        }

        public override void Enter(string lastStateName, string nextStateName, float additionalDeltaTime, params object[] args)
        {
            Character.ChangeVelocity(Vector3.zero);
            Character.animator.Play("Idle");
            base.Enter(lastStateName, nextStateName, additionalDeltaTime, args);
        }

        public override void PreUpdate()
        {
            if (Character.input.run >= 0.25f)
            {
                Character.fsm.ChangeState("RUN");
            }
            else
            {
                Character.fsm.ChangeState("LOCK");
            }
        }

        public void Move(float speed = 4f)
        {
            if (Character.input.move.vector.magnitude > 0.25)
            {
                if (Input.move.isActive)
                {
                    // Calculate how fast we should be moving
                    var inputVelocity = Input.move.vector * speed;
                    // Calculate the delta velocity
                    var velocityChange = inputVelocity - velocity;
                    velocityChange.y = 0;
                    // Limit acceleration
                    if (velocityChange.magnitude > maxAcceleration)
                    {
                        velocityChange = velocityChange.normalized * maxAcceleration;
                    }
                    velocity += velocityChange;
                    Character.Move(Transform.position + (velocity * Time.fixedDeltaTime));
                }
            }
        }

        public void MoveRun(float speed = 4f)
        {
            if (Character.input.move.vector.magnitude > 0.25)
            {
                if (Input.move.isActive)
                {
                    // Calculate how fast we should be moving
                    var inputVelocity = Input.move.vector * speed;
                    // Calculate the delta velocity
                    var velocityChange = inputVelocity - velocity;
                    velocityChange.y = 0;
                    // Limit acceleration
                    if (velocityChange.magnitude > maxAcceleration)
                    {
                        velocityChange = velocityChange.normalized * maxAcceleration;
                    }
                    velocity += velocityChange;
                    Character.Move(Transform.position + (velocity * Time.fixedDeltaTime));
                }
            }
        }
    }
}