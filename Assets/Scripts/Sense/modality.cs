public abstract class Modality {

    public float maximumRange;
    public float attenuation;
    public float inverseTransmissionSpeed;

    public abstract bool extraChecks(Signal signal, Sensor sensor);
}