public class ResetSensor : Action {

    Sensor sensor;

    public ResetSensor(Sensor Sensor){
        sensor = Sensor;
    }
    public override void DoAction(){
        sensor.ResetSensor();
    }
}