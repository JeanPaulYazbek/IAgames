using UnityEngine;
using System;

public class KinematicArrive : Behavior
{

    public Kinetics character;
    public Kinetics target;
    public float maxspeed;
    public float timeToTarget;
    public float radius;


    public KinematicArrive(Kinetics Character, Kinetics Target, float Maxspeed, float Time, float Radius) {
        character = Character;
        target = Target;
        maxspeed = Maxspeed;
        timeToTarget = Time;
        radius = Radius;
    }


    //funcion que realiza seeking de un punto y se detiene si lo alcanza en un radio
    //dado
    public KinematicSteeringOutput getSteering(){

        
        //velocidades de salida
        KinematicSteeringOutput steering = new KinematicSteeringOutput();
        
   
        steering.velocity = target.transform.position - character.transform.position;

        if (steering.velocity.magnitude<radius){//si ya estamos en el radio
            steering.rotation = 0f;
            steering.velocity = Vector3.zero;
            return steering;
        }

        //tenemos que ajustar la velocidad para que
        //cada vez que este mas cerca vaya mas lento
        steering.velocity /= timeToTarget;
       
        if (steering.velocity.magnitude >maxspeed){
            steering.velocity.Normalize();
            steering.velocity *= maxspeed;
        }
        
        character.GetNewOrietation(steering.velocity);

        steering.rotation = 0f;
        return steering;

    }
    
}

