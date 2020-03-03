using UnityEngine;
public abstract class Sensor{

    public Transform transform;
    public abstract bool DetectsModality(Modality modality);

    public float threshold;

    public abstract void Notify(Signal signal);


}