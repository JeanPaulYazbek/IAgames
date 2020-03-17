using UnityEngine;
//Condicion para saber si escuchamos algo con cierta intensidad
public class HeardSomething : Condition {

    SoundSensor sensor;
    string soundType;
    float minIntensity;

    public HeardSomething(SoundSensor Sensor, string SoundType, float offset){
        sensor = Sensor;
        soundType = SoundType;
        //El offset representa cuanto mas de lo minimos aceptamos 
        minIntensity = Sensor.threshold + offset;
    }

    public override bool Test(){

        return sensor.notified && sensor.soundType == soundType && sensor.soundIntensity > minIntensity;
    }
}