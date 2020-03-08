using UnityEngine;
//Condicion para saber si para un sensor dado
//detecta un olor venenoso
public class SmelledPoison : Condition {

    SmellSensor sensor;

    public SmelledPoison(SmellSensor Sensor){
        sensor = Sensor;
    }

    public override bool Test(){

        return sensor.notified && sensor.smellType == "Poison";
    }
}