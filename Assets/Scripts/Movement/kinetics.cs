using UnityEngine;
using System;

public class Kinetics
{

    public float rotation = 0f;//velocidad angular
    public Vector3 velocity;//velocidad

    public Transform transform;//aqui esta la posicion y orientacion

    Vector3 zVector = new Vector3(0f,0f,1f);//vector unitario en z de utilidad

    //CONSTRUCTOR
    public Kinetics(float Rotation, Vector3 Velocity, Transform Transform)
    {
        
        rotation = Rotation;
        velocity = Velocity;
        transform = Transform;
       
    }

    //Funcion que actualiza ciertos valores dependiendo de las aceleraciones
    // y el tiempo
    public void UpdateKinetics(SteeringOutput steering, float time, float maxspeed){

        //Actualizamos posicion y orientacion
        transform.position += velocity * time;
        transform.eulerAngles += zVector * rotation*time;

        //Actualizamos velocidad y rotacion 
        velocity += steering.linear * time;
        rotation +=  steering.angular * time;

        //si nos pasamos de una velocidad maxima lo convertimos a la maxima
        if (velocity.magnitude > maxspeed){
            velocity.Normalize();
            velocity *= maxspeed;
        }
    }

    //funcion que actualiza velocidades dadas nuevas velocidades
    public void UpdateSteeringOutput(KinematicSteeringOutput newVelocities){
        velocity =  newVelocities.velocity;
        rotation = newVelocities.rotation;
    }


    //funcion que modifica la orientacion cuando cambias velocidad
    public void GetNewOrietation(Vector3 velocity) {

        //Debug.Log(Transform.eulerAngles);
        if (velocity.magnitude > 0) {
            //hay que multiplicar por 53 para convertir los radianes a angulos
            transform.eulerAngles = zVector*57* Convert.ToSingle(Math.Atan2(-velocity.x, velocity.y));
        }
    }

    //funcion que convierte orientacion en vector
    public Vector3 OrientationToVector(){

        //hay que dividir entre 53 para convertir los angulos a radianes
        return new Vector3(-(float)Math.Sin(transform.eulerAngles.z/57), (float)Math.Cos(transform.eulerAngles.z/57),0f);

    }

    public void SetRandomOrientation(){
        
        transform.eulerAngles = new Vector3 (0f, 0f, UnityEngine.Random.Range(-360f, 360f));
    }
}
