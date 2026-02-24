using UnityEngine;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;

public class AIPushDownStateMachine
{
    Stack<AIState> stateStack = new Stack<AIState>();
    private Dictionary<string, AIState> states = new Dictionary<string, AIState>();


    public AIState CurrentState { get { return (stateStack.Count > 0) ? stateStack.Peek() : null; } }

    public void Update()
    {
        CurrentState?.OnUpdate();
    }

    public void AddState(AIState state)
    {
        if (states.ContainsKey(state.Name))
        {
            Debug.LogError($"State '{state.Name}' already exists in the state machine.");
            return;
        }

        states[state.Name] = state;
    }

    public void PushState<T>()
    {
        PushState(typeof(T).Name);
    }

    public void PushState(string name)
    {
        var nextState = states[name];

        // exit current state
        CurrentState?.OnExit();

        nextState = states[name];

        stateStack.Push(nextState);

        CurrentState.OnEnter();
    }
    public void SetState<T>()
    {
        SetState(typeof(T).Name);
    }

    public void SetState(string name)
    {
        if (!states.ContainsKey(name))
        {
            Debug.LogError($"State '{name}' does not exist in the state machine.");
            return;
        }

        while (stateStack.Count > 0) 
        {
            stateStack.Pop();
        }
        var newState = states[name];
        //push the new state
        stateStack.Push(newState);
        //enter the new state (current)
        newState.OnEnter();
    }

    public void PopState()
    {
        if (stateStack.Count == 0) return;

        //call exit on current
        CurrentState.OnExit();
        // pop current state
        var newState = stateStack.Pop();
        // enter new state
        CurrentState?.OnEnter();
    }

    public string GetString()
    {
        string str = "";

        var array = stateStack.ToArray();
        for (int i = 0; i < array.Length; i++)
        {
            str += array[i].Name;
            if (i < array.Length - 1) str += "\n";
        }

        return str;
    }
}
