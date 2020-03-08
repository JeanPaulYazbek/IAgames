using UnityEngine;
public class smell_sensor : MonoBehaviour {

    public SmellSensor sensor;
    public float threshold;

    void Awake(){
        sensor = new SmellSensor(transform, threshold);
    }

}