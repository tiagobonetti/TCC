﻿using UnityEngine;
using XInputDotNetPure;

static class PlayerIndexExtensions
{
    public static XInputDotNetPure.PlayerIndex toXInput(this PlayerIndex id)
    {
        switch (id)
        {
            default:
            case PlayerIndex.One:
                return XInputDotNetPure.PlayerIndex.One;
            case PlayerIndex.Two:
                return XInputDotNetPure.PlayerIndex.Two;
            case PlayerIndex.Three:
                return XInputDotNetPure.PlayerIndex.Three;
            case PlayerIndex.Four:
                return XInputDotNetPure.PlayerIndex.Four;
        }
    }
}

public class GamePadInput : BaseInput
{
    GamePadState state;
    GamePadState lastState;

    XInputDotNetPure.PlayerIndex xinputId;
    public override PlayerIndex id
    {
        get { return base.id; }
        set
        {
            base.id = id;
            xinputId = value.toXInput();
            name = "GamePad" + value;
        }
    }

    static readonly float triggerThreshold = 0.2f;

    bool PressedB
    {
        get { return state.Buttons.B == ButtonState.Pressed && lastState.Buttons.B == ButtonState.Released; }
    }
    bool PressedX
    {
        get { return state.Buttons.X == ButtonState.Pressed && lastState.Buttons.X == ButtonState.Released; }
    }
    bool PressedA
    {
        get { return state.Buttons.A == ButtonState.Pressed && lastState.Buttons.A == ButtonState.Released; }
    }
    bool PressedY
    {
        get { return state.Buttons.Y == ButtonState.Pressed && lastState.Buttons.Y == ButtonState.Released; }
    }
    bool PressedRS
    {
        get { return state.Buttons.RightShoulder == ButtonState.Pressed && lastState.Buttons.RightShoulder == ButtonState.Released; }
    }
    bool PressedLS
    {
        get { return state.Buttons.LeftShoulder == ButtonState.Pressed && lastState.Buttons.LeftShoulder == ButtonState.Released; }
    }
    bool PressedRT
    {
        get { return state.Triggers.Right > triggerThreshold && (lastState.Triggers.Right <= triggerThreshold); }
    }

    public override void Update()
    {
        lastState = state;
        state = GamePad.GetState(xinputId);

        buffer.Update();

        move.horizontal = state.ThumbSticks.Left.X;
        move.vertical = state.ThumbSticks.Left.Y;

        look.horizontal = state.ThumbSticks.Right.X;
        look.vertical = state.ThumbSticks.Right.Y;

        // Modifier
        run = state.Triggers.Left;

        // Actions
        attack = state.Triggers.Right > triggerThreshold;
        var attacked = PressedRT;

        block = state.Buttons.RightShoulder == ButtonState.Pressed;
        var blocked = PressedRS;

        highStance = state.Buttons.LeftShoulder == ButtonState.Pressed;
        //var highStanced = PressedRS; //that's not even a word.

        dash = state.Buttons.B == ButtonState.Pressed;
        var dashed = PressedB;


        if (dashed)
        {
            buffer.Push(new InputEvent.Dash(move));
        }
        else if (blocked)
        {
            buffer.Push(new InputEvent.Block());
        }
        else if (attacked)
        {
            buffer.Push(new InputEvent.Attack(move));
        }
    }

    public override void FixedUpdate()
    {
    }

    public override string Debug
    {
        get
        {
            string text = "";
            text += string.Format("IsConnected {0}\n", state.IsConnected);
            text += string.Format("Stick Left  {0,4:0.0} {1,4:0.0}\n", state.ThumbSticks.Left.X, state.ThumbSticks.Left.Y);
            text += string.Format("Stick Right {0,4:0.0} {1,4:0.0}\n", state.ThumbSticks.Right.X, state.ThumbSticks.Right.Y);
            text += string.Format("Shoulders {0,8} {1,8}\n", state.Buttons.LeftShoulder, state.Buttons.RightShoulder);
            text += string.Format("Triggers  {0,4:0.0} {1,4:0.0}\n", state.Triggers.Left, state.Triggers.Right);
            return text;
        }
    }

}