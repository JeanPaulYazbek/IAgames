using UnityEngine;
//Condicion para saber si para un sensor dado
//detecta un tipo de olor dado
public class SmelledSomething : Condition {

    SmellSensor sensor;
    string smellType;

    public SmelledSomething(SmellSensor Sensor, string SmellType){
        sensor = Sensor;
        smellType = SmellType;
    }

    public override bool Test(){

        return sensor.notified && sensor.smellType == smellType;
    }
}