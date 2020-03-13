using UnityEngine;
//Condicion para saber si para un sensor dado
//observa algo
public class SawSomething : Condition {

    SightSensor sensor;
    string sightType;

    public SawSomething(SightSensor Sensor, string SightType){
        sensor = Sensor;
        sightType = SightType;
    }

    public override bool Test(){

        return sensor.notified && sensor.sightType == sightType;
    }
}