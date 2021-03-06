﻿using UnityEngine;
using UnityEngine.Assertions;
using BaseSwing = Assets.MovementPrototype.Character.States.HoldAttackStates.BaseSwing;
using HeavySwing = Assets.MovementPrototype.Character.States.HoldAttackStates.HeavySwing;
using HeavyAttack = Assets.MovementPrototype.Character.States.HoldAttackStates.HeavyAttack;

namespace Assets.MovementPrototype.Character.States.HoldBlockStates
{
    public static class VectorExtensions
    {
        public static Vector3 xz(this Vector3 v)
        {
            return new Vector3(v.x, 0, v.z);
        }
    }
    public class BlockMidSwing : AnimatedState
    {
        private bool holding;
        private float minSwingTime = 0.3f;
        public BlockMidSwing(CharFsm fsm) : base(fsm)
        {
            Name = "BLOCK/MID/SWING";
            nextState = "BLOCK/MID/RECOVER";
            totalTime = 60000000000000f;
            canPlayerMove = true;
            moveSpeed = 0.75f;
            turnRate = 0.25f;
            Animation = "BlockMidSwing";
        }
        public override void PreUpdate()
        {
            base.PreUpdate();
            if (holding)
            {
                if (Character.input.block == false)
                {
                    holding = false;
                }
            }
            else
            {
                if (elapsed > minSwingTime)
                {
                    UnityEngine.Debug.Log("Early end");
                    Fsm.ChangeState(nextState);
                }
            }
        }
        public override void Enter(string lastStateName, string nextStateName, float additionalDeltaTime, params object[] args)
        {
            base.Enter(lastStateName, nextStateName, additionalDeltaTime, args);
            Character.BlockMidCollider.enabled = true;
            holding = true;
        }
        public override void Exit(string lastStateName, string nextStateName, float additionalDeltaTime, params object[] args)
        {
            base.Exit(lastStateName, nextStateName, additionalDeltaTime, args);
            Character.BlockMidCollider.enabled = false;

        }
        public override void OnTriggerEnter(Collider collider)
        {
            if (collider.name == "Attack Collider")
            {
                var otherCharacter = collider.transform.parent.parent.GetComponent<CharController>();
                if (!ReferenceEquals(Character, otherCharacter))
                {
                    var attackerState = otherCharacter.fsm.Current as BaseSwing;
                    if (attackerState != null && (attackerState.Name != "DOWN/LIGHT/SWING" &&
                                                  attackerState.Name != "DOWN/HEAVY/SWING"))
                    {

                        RaycastHit hitInfo;
                        Assert.IsTrue(attackerState.GetCollisionPoint(out hitInfo), "IT SHOULD HAVE HIT BUT IT DID NOT HIT SEND HELP");
                        Vector3 myForward = Transform.forward.xz().normalized;
                        Vector3 otherForward = (hitInfo.point - Transform.position).xz().normalized;
                        if (Mathf.Abs(Vector3.Angle(myForward, otherForward)) <= defenseAngle/2f)
                        {
                            Character.ShowBlockSpark(collider.transform.position);
                            otherCharacter.fsm.ChangeState("STAGGER");
                            if (attackerState is HeavyAttack)
                            {
                                Fsm.ChangeState("STAGGER");
                            }
                            return;
                        }
                    }
                }
            }

            // otherwise defer to base
            base.OnTriggerEnter(collider);
        }
    }
}
