using System.Collections.Generic;

//esta clase representa una maquina de estados
public class StateMachine{
    State[] states;
    State initialState;
    State currentState;

    public StateMachine(State[] States, State InitialState){
        states = States;
        initialState = InitialState;
        currentState = InitialState;

    }

    public List<Action> Update(){


        Transition triggeredTrasition = null;

        foreach(var transition in currentState.GetTransitions()){
            if (transition.IsTriggered()){
                triggeredTrasition = transition;
                break;
            }
        }

        if( !(triggeredTrasition is null)){
            State targetState = triggeredTrasition.GetTargetState();

            List<Action> actions;

            actions = currentState.GetExitAction();
            actions.AddRange(triggeredTrasition.GetAction());
            actions.AddRange(targetState.GetEntryAction());

            currentState = targetState;
            return actions;

        }

        return currentState.GetAction();
    }

    public void RunMachine(){
        List<Action> actions = Update();
        foreach(var action in actions){
            action.DoAction();
        }
    }
}