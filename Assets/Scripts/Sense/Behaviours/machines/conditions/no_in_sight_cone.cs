using UnityEngine;
//Condicion para saber si el dueño de la ultima
//sennal enviada se alejo lo suficiente
public class NoInSightCone : Condition {

    SightSensor sensor;
    Utilities utilities = new Utilities(); 

    public NoInSightCone(SightSensor Sensor){
        sensor = Sensor;
    }

    public override bool Test(){

        
        Vector3 signalPos = sensor.detectedSignal.transform.position;
        Vector3 sensorPos = sensor.transform.position;
        float sensorOrien = sensor.transform.eulerAngles.z;
        SightSensor sigthSensor = (SightSensor) sensor;
        float width = sigthSensor.width;
        float large = sensor.threshold;
        bool incone =  utilities.CheckSightCone(sensorPos, large, width, sensorOrien, signalPos);
        return !(incone);
    }
}