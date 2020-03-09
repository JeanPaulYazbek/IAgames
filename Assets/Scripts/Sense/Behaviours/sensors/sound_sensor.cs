using UnityEngine;
public class sound_sensor : MonoBehaviour {

    public SoundSensor sensor;
    public float threshold;

    public bool player;

    void Awake(){
        sensor = new SoundSensor(transform, threshold, player);
    }

}