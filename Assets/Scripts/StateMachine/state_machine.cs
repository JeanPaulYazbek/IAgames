using System.Collections.Generic;

//esta clase representa una maquina de estados
public class StateMachine{
    State[] states;
    State initialState;
    State currentState;

    public StateMachine(State[] States, State InitialState){
        states = States;

        //el estado inicial sera uno por defecto sin acciones que siempre se mueve al InitialState
        //esto lo hice asi porque aunque no lo parezca es mas general asi
        List<Action> emptyList = new List<Action>();
        State start = new State(emptyList, emptyList, emptyList);
        Transition firstTrans = new Transition(new TrueCondition(), emptyList, InitialState);
        start.transitions = new List<Transition> { firstTrans };
        initialState = start;
        currentState = start;

    }

    //Funcion que actualiza la maquina de estados
    //devuelve las acciones que se deben realizar
    public List<Action> Update(){


        Transition triggeredTrasition = null;

        //Si alguna de las transiciones del estado actual se activa
        foreach(var transition in currentState.GetTransitions()){
            if (transition.IsTriggered()){
                triggeredTrasition = transition;
                break;
            }
        }

        //Si alguna se activo nos cambiamos de estado
        if( !(triggeredTrasition is null)){
            State targetState = triggeredTrasition.GetTargetState();

            List<Action> actions = new List<Action>();

            actions.AddRange(currentState.GetExitAction());
            actions.AddRange(triggeredTrasition.GetAction());
            actions.AddRange(targetState.GetEntryAction());

            currentState = targetState;
            return actions;

        }

        return currentState.GetAction();
    }

    //Funcion que corre las acciones de un update
    public void RunMachine(){
        List<Action> actions = Update();
        foreach(var action in actions){
            action.DoAction();
        }
    }
}