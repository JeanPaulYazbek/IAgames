using UnityEngine;
public class sound_sensor : MonoBehaviour {

    public SoundSensor sensor;
    public float threshold;

    void Awake(){
        sensor = new SoundSensor(transform, threshold);
    }

}