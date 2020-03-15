using UnityEngine;
public class sight_sensor : MonoBehaviour {

    public SightSensor sensor;
    public float threshold;//largo de la vision
    public float widthAngle;//ancho de la vision

    public string[] careToSee;//que nos interesa ver

    void Awake(){
        sensor = new SightSensor(transform, threshold, widthAngle, careToSee);
    }

}