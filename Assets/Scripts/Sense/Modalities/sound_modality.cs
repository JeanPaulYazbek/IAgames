public class SoundModality : Modality {

    public SoundModality(float MaximumRange, float Attenuation, float InverseTransmissionSpeed, string Description) : 
    base(MaximumRange, Attenuation, InverseTransmissionSpeed, Description, "Sound"){}

    public override bool ExtraChecks(Signal signal, Sensor sensor){
        return true;
    }

}