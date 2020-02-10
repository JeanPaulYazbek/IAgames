using System.Collections.Generic;
//esta clase ayudara a implementar estados
public class State {

    List<Action> action;
    List<Action> entryAction;
    List<Action> exitAction;
    public List<Transition> transitions;


    public State(List<Action> Action, List<Action> EntryAction, List<Action> ExitAction){
        action = Action;
        entryAction = EntryAction;
        exitAction = ExitAction;
        transitions = null;
    }

    //Funcion que devuelve la accion que se debe hacer durante el estado
    public List<Action> GetAction(){
        return action;
    }

    //Funcion que devuelve la accion  que se debe hacer una vez al entrar al estado
    public List<Action> GetEntryAction(){
        return entryAction;
    }
    //Funcion que devuelve la accion que se debe hacer al salir del estado
    public List<Action> GetExitAction(){
        return exitAction;
    }
    //Funcion que devuelve las transiciones del estado
    public List<Transition> GetTransitions(){
        return transitions;
    }


}