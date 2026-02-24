using System.Collections.Generic;
using UnityEngine;

public class AIStateMachine 
{
    private Dictionary<string, AIState> states = new Dictionary<string, AIState>();

    public AIState CurrentState { get; private set; }

    public string GetString()
    {
        return (CurrentState != null) ? CurrentState.Name : "";
    }
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

        var nextState = states[name];

        if (nextState == CurrentState || nextState == null)
        {
            Debug.LogWarning($"State '{name}' is already the current state.");
            return;
        }

        //exit current state
        CurrentState?.OnExit();

        //enter current state
        CurrentState = nextState;
        CurrentState?.OnEnter();

    }

}
