using UnityEngine;
public class SoundSensor : Sensor {

    // Booleanos que representa que el sensor sintio algo
    public bool notified = false;
    // Representa el tipo de aroma que se sintion por ejemplo "Poison" para venenoso
    public string soundType;
    // Representa la intensidad del sonido detectado
    public float soundIntensity;

    // Tipos de cosas que nos importan si oimos
    public string[] careToHear;

    // Representa true si el jugador esta usando el sensor
    public bool player;
    

    public SoundSensor(Transform Transform, float Threshold, bool Player, string[] CareToHear) : 
    base(Transform, Threshold){
        player = Player;
        careToHear = CareToHear;
    }

    public override bool DetectsModality(Modality modality){
        
        if(modality.senseType != "Sound"){//si no es una sennal de sonido es rechazada
            return false;
        }

        //Solamente nos interessa una sennal si lo que vemos es de nuestro interes
        foreach(var name in careToHear){
            if(modality.description == name){
                return true;
            }
        }
        return false;
    }

    // El sensor de sonido ademas de marcar los que se oyo algo
    // y que se oyo debe reproducir el volumen adecuado de audio si 
    // es el player quien lo esta usando
    public override void Notify(Signal signal){
        notified = true;
        soundType = signal.modality.description;

        float max = signal.strength;
        //Distancia entre el origen de la sennal y el sensor
        float distance =  Vector3.Distance(signal.transform.position, transform.position);
        //Intensidad del sonido recibido
        float intensity = signal.Intensity(distance);

        //Guardamos la intensidad para las notificaciones
        soundIntensity = intensity;

        //Si jugador editamos volumen de audio
        if(player){
            

            //Nuevo volumen
            float newVolume = intensity/(max);

            //Si estamos muy lejos para escuchar
            if(distance > signal.modality.maximumRange - 2){
                newVolume = 0;
            }

            //Cambiamos el volumen
            SoundModality soundModality = (SoundModality)signal.modality;
            soundModality.audio.volume = newVolume;

        }
    }

    public override void ResetSensor(){
        notified = false;
        soundType = "";
        soundIntensity = 0f;
    }

}