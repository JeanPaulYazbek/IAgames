using UnityEngine;
public class SightSensor : Sensor {

    // Booleanos que representa que el sensor sintio algo
    public bool notified = false;
    // Representa el tipo de aroma que se sintion por ejemplo "Poison" para venenoso
    public string sightType;
    // Representa que tan ancho es el cono de tu vision
    public float width;

    public SightSensor(Transform Transform, float Threshold, float Width) : 
    base(Transform, Threshold){
        width = Width;
    }

    public override bool DetectsModality(Modality modality){
        return (modality.senseType == "Sight");
    }

    public override void Notify(Signal signal){
        notified = true;
        sightType = signal.modality.description;
    }

    public override void ResetSensor(){
        notified = false;
        sightType = "";
    }
}