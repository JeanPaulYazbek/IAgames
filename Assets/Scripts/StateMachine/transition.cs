using System.Collections.Generic;


//esta clase ayudara a implementar transiciones
public class Transition {

    Condition condition;
    List<Action> action;
    State targetState;

    public Transition(Condition Condition, List<Action> Action, State TargetState){
        condition = Condition;
        action = Action;
        targetState = TargetState;
    }

    //Funcion que devuelve si se dispara el trigger de la transicion
    public bool IsTriggered(){
        return condition.Test();
    }
    //Funcion que devuelve la accion que se debe hacer durante el estado
    public List<Action> GetAction(){
        return action;
    }

    //Funcion que devuevle el target de la transicion
    public State GetTargetState(){
        return targetState;
    }

    
   


}