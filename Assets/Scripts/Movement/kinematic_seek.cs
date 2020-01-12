using UnityEngine;
using System;

public class KinematicSeek {

    public Kinetics character;
    public Kinetics target;
    public float maxspeed;


    public KinematicSeek(Kinetics Character, Kinetics Target, float Maxspeed) {
        character = Character;
        target = Target;
        maxspeed = Maxspeed;
    }

    //funcion que realiza seeking de un punto si le pasas 1 y flee si le pasas
    //0
    public KinematicSteeringOutput getSteering(int seek_or_flee){

       
        //velocidades de salida
        KinematicSteeringOutput steering = new KinematicSteeringOutput();

        if (seek_or_flee == 1){
            steering.velocity = target.transform.position - character.transform.position;
        }else{
            steering.velocity = character.transform.position - target.transform.position;
        }
             
        steering.velocity.Normalize();
        //Debug.Log(newVelocity);
        steering.velocity *= maxspeed;

       // Debug.Log(newVelocity);
        character.GetNewOrietation(steering.velocity);

        steering.rotation = 0f;
        //Debug.Log(newVelocity);
        return steering;

    }
    
}

