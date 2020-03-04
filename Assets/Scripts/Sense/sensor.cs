using UnityEngine;
public abstract class Sensor{

    //Informacion del personaje que tendra su sensor
    public Transform transform;
    //Funcion que dice si una modalidad es igual a la modalidad del sensor
    public abstract bool DetectsModality(Modality modality);
    //Minima intensidad de lo que queremos sentir para que pueda ser sentido
    public float threshold;
    //Funcion que avisa que hubo algo sentido
    public abstract void Notify(Signal signal);

    public Sensor(Transform Transform, float Threshold){
        transform = Transform;
        threshold = Threshold;
    }


}