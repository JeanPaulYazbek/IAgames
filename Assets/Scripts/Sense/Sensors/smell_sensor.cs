using UnityEngine;
public class SmellSensor : Sensor {

    // Booleanos que representa que el sensor sintio algo
    public bool notified = false;
    // Representa el tipo de aroma que se sintion por ejemplo "Poison" para venenoso
    public string smellType;

    public SmellSensor(Transform Transform, float Threshold) : 
    base(Transform, Threshold){}

    public override bool DetectsModality(Modality modality){
        return (modality.senseType == "Smell");
    }

    public override void Notify(Signal signal){
        notified = true;
        smellType = signal.modality.description;
    }

    public override void ResetSensor(){
        notified = false;
        smellType = "";
    }
}