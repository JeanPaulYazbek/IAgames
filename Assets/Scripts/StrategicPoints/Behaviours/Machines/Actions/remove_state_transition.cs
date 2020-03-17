//esta accion ayuda a quitarle cierta transicion a un estado
public class RemoveStateTransition : Action {

    Transition transitionToRemove;
    State state;

    public RemoveStateTransition(Transition TransitionToRemove, State State){
        transitionToRemove = TransitionToRemove;
        state = State;
    }

    public override void DoAction(){

        state.transitions.Remove(transitionToRemove);
    }
}