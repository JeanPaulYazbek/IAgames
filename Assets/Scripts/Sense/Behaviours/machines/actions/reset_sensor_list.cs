using System.Collections.Generic;

// Esta accion toma una lista de acciones tipo ressetSensor 
// y los aplica todos
public class ResetSensorList : Action {

    List<ResetSensor> resets;


    public ResetSensorList(List<ResetSensor> Resets){
        
        resets = Resets;
    }
    public override void DoAction(){
        foreach (var reset in resets)
        {
            reset.DoAction();
        }
    }
}