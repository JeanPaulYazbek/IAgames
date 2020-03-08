using UnityEngine;

// Esta case representa las sennales
public class Signal{

    public float strength;//Intensidad inicial desde el origen de la sennal
    public Transform transform;//Posiscion de la sennal
    public Modality modality;//Modalidad para  la sennal


    public Signal(float Strength, Transform Transform, Modality Modality){
        strength = Strength;
        transform = Transform;
        modality = Modality;
    }
}