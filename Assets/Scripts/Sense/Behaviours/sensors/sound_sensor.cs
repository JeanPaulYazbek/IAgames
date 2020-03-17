using UnityEngine;
public class sound_sensor : MonoBehaviour {

    public SoundSensor sensor;
    public float threshold;

    public bool player;

    public string[] careToHear;//que nos interesa oir

    void Awake(){
        sensor = new SoundSensor(transform, threshold, player, careToHear);
    }

}