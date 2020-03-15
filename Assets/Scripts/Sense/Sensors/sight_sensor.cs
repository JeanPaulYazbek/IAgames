using UnityEngine;
public class SightSensor : Sensor {

    //DATOS NOTIFICACION
    // Booleanos que representa que el sensor sintio algo
    public bool notified = false;
    // Representa el tipo de aroma que se sintion por ejemplo "Poison" para venenoso
    public string sightType;
    // Necesitamos saber a quien vimos
    public Signal detectedSignal;

    // Tipos de cosas que nos importan si vemos
    public string[] careToSee;


    // Representa que tan ancho es el cono de tu vision
    public float width;
    

    public SightSensor(Transform Transform, float Threshold, float Width, string[] CareToSee) : 
    base(Transform, Threshold){
        width = Width;
        careToSee = CareToSee;
    }

    public override bool DetectsModality(Modality modality){

        if(!(modality.senseType == "Sight")){//si no es una sennal de vision es rechazada
            return false;
        }

        //Solamente nos interessa una sennal si lo que vemos es de nuestro interes
        foreach(var name in careToSee){
            if(modality.description == name){
                return true;
            }
        }
        return false;
 
    }

    public override void Notify(Signal signal){
        notified = true;
        sightType = signal.modality.description;
        detectedSignal = signal;
    }

    public override void ResetSensor(){
        notified = false;
        sightType = "";
    }


}