//Accion que marca el tiempo de comienzo de 
//una condicion de time out
public class SetTimer : Action {

    TimeOut timeOut;

    public SetTimer(TimeOut TimeOut){
        timeOut = TimeOut;
    }

    public override void DoAction(){
        timeOut.SetStartTime();
    }
    
}