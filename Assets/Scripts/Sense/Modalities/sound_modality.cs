using UnityEngine;
public class SoundModality : Modality {

    public AudioSource audio;//Audio modality must have something to play

    public SoundModality(float MaximumRange, float Attenuation, float InverseTransmissionSpeed, string Description, AudioSource Audio) : 
    base(MaximumRange, Attenuation, InverseTransmissionSpeed, Description, "Sound"){
        audio = Audio;
    }

    public override bool ExtraChecks(Signal signal, Sensor sensor){
        return true;
    }

}