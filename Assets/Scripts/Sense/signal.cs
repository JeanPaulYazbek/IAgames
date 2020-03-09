using UnityEngine;
using System;
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

    //funcion que calcula la intensidad de una sennal dada la distancia que recorrio
    public float Intensity(float distance){
        return strength * (float)Math.Pow(modality.attenuation, distance);
    }
}