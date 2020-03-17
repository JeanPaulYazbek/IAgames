using System.Collections.Generic;
//esta accion ayuda a quitarle la primera accion a una lista de acciones
public class RemoveAction : Action {

    Action actionToRemove;
    
    List<Action> actions;

    public RemoveAction(Action ActionToRemove, List<Action> Actions){
        actionToRemove = ActionToRemove;
        actions = Actions;
    }

    public override void DoAction(){
        actions.Remove(actionToRemove);
    }
}