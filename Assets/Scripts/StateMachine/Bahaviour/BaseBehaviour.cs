﻿using UnityEngine;
using System.Collections.Generic;

public class BaseBehaviour : IStateBehaviour
{
    public IState State { get; protected set; }
    public BaseBehaviour(IState state)
    {
        State = state;
        State.AddBehaviour(this);
    }
    public string BehaviourName { get; protected set; }
    public string DebugString
    {
        get { return BehaviourName; }
    }
    public virtual void PreUpdate()
    {
    }
    public virtual void FixedUpdate()
    {
    }
    public virtual void Enter(string lastStateName, string nextStateName, float additionalDeltaTime = 0f, params object[] args)
    {
    }
    public virtual void Exit(string lastStateName, string nextStateName, float additionalDeltaTime = 0f, params object[] args)
    {
    }
    public virtual void OnTriggerEnter(Collider colllider)
    {
    }
    public virtual void OnCollisionEnter(Collision collision)
    {
    }
}