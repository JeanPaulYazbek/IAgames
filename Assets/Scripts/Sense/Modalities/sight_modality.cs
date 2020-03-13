using UnityEngine;
public class SightModality : Modality {

    public obstacle_data[] obstacles;

    public SightModality(float MaximumRange, float Attenuation, float InverseTransmissionSpeed, string Description, obstacle_data[] Obstacles) : 
    base(MaximumRange, Attenuation, InverseTransmissionSpeed, Description, "Sight"){
        obstacles = Obstacles;
    }

    Utilities utilities = new Utilities();


    public override bool ExtraChecks(Signal signal, Sensor sensor){
        Vector3 signalPos = signal.transform.position;
        Vector3 sensorPos = sensor.transform.position;
        float sensorOrien = sensor.transform.eulerAngles.z;
        SightSensor sigthSensor = (SightSensor) sensor;
        float width = sigthSensor.width;
        float large = sensor.threshold;
        return utilities.CheckSightCone(sensorPos, large, width, sensorOrien, signalPos) && !utilities.LineSegmentIntersectionObstacleType(signalPos, sensorPos, obstacles, new string[] {"Tree1", "Tree2"});
    }


}