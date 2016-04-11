﻿using System;
using System.Collections.Generic;

public abstract class BaseFsm : IState
{
    public Dictionary<string, IState> dict;
    public IState Current { get; protected set; }
    public string Name { get; protected set; }
    public string Debug
    {
        get
        {
            return Name + "/" + Current.Debug;
        }
    }
    public BaseFsm Fsm { get; protected set; }

    public void AddState(IState state)
    {
        dict.Add(state.Name, state);
    }

    public virtual void ChangeState(string name, float additionalDeltaTime = 0f, params object[] args)
    {
        ChangeState(Current.Name, name, additionalDeltaTime, args);
    }

    public virtual void ChangeState(string lastStateName, string nextStateName, float additionalDeltaTime, params object[] args)
    {
        Current.Exit(lastStateName, nextStateName, additionalDeltaTime, args);
        Current = dict[nextStateName];
        Current.Enter(lastStateName, nextStateName, additionalDeltaTime, args);
    }

    public BaseFsm(BaseFsm fsm = null)
    {
        Fsm = fsm;
        dict = new Dictionary<string, IState>();
    }

    public virtual void PreUpdate()
    {
        // dict["COMMON"].PreUpdate();
        Current.PreUpdate();
    }

    public virtual void FixedUpdate()
    {
       //dict["COMMON"].FixedUpdate();
        Current.FixedUpdate();
    }

    public virtual void Enter(string lastStateName, string nextStateName, float additionalDeltaTime, params object[] args)
    {
        Current.Enter(lastStateName, nextStateName, additionalDeltaTime, args);
    }

    public virtual void Exit(string lastStateName, string nextStateName, float additionalDeltaTime, params object[] args)
    {
        Current.Exit(lastStateName, nextStateName, additionalDeltaTime, args);
    }
}