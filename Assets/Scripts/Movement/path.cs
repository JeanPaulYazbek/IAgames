using UnityEngine;
using System;

public class Path {

    public float Y_coord(float x){
        // en este caso la funcion es una regla
        return (float)System.Math.Sin(x);

    }

    public Vector3 GetPosition(float x){

        return new Vector3(x, Y_coord(x),0f);

    }

    public float GetParam(float x){

        return x;
    }

}