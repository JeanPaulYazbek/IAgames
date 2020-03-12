using UnityEngine;
public class sight_sensor : MonoBehaviour {

    public SightSensor sensor;
    public float threshold;
    public float widthAngle;

    void Awake(){
        sensor = new SightSensor(transform, threshold, widthAngle);
    }

}