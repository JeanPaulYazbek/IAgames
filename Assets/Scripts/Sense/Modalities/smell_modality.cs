public class SmellModality : Modality {

    public SmellModality(float MaximumRange, float Attenuation, float InverseTransmissionSpeed, string Description) : 
    base(MaximumRange, Attenuation, InverseTransmissionSpeed, Description, "Smell"){}

    public override bool ExtraChecks(Signal signal, Sensor sensor){
        return true;
    }

}