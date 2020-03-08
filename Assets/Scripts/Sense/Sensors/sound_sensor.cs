using UnityEngine;
public class SoundSensor : Sensor {

    // Booleanos que representa que el sensor sintio algo
    public bool notified = false;
    // Representa el tipo de aroma que se sintion por ejemplo "Poison" para venenoso
    public string soundType;

    public SoundSensor(Transform Transform, float Threshold) : 
    base(Transform, Threshold){}

    public override bool DetectsModality(Modality modality){
        return (modality.senseType == "Sound");
    }

    public override void Notify(Signal signal){
        notified = true;
        soundType = signal.modality.description;
    }

    public override void ResetSensor(){
        notified = false;
        soundType = "";
    }
}