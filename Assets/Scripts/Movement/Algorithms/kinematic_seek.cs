using UnityEngine;
using System;

public class KinematicSeek 
{

    public Kinetics character;
    public Kinetics target;
    public float maxspeed;


    public KinematicSeek(Kinetics Character, Kinetics Target, float Maxspeed) {
        character = Character;
        target = Target;
        maxspeed = Maxspeed;
    }

    public KinematicSteeringOutput getSteering(int seek_or_flee){
        return getSteering2(target.transform.position, seek_or_flee);
    }

    //funcion que realiza seeking de un punto si le pasas 1 y flee si le pasas
    //0
    public KinematicSteeringOutput getSteering2(Vector3 targetPosition, int seek_or_flee){

       
        //velocidades de salida
        KinematicSteeringOutput steering = new KinematicSteeringOutput();

        if (seek_or_flee == 1){
            steering.velocity = targetPosition - character.transform.position;
        }else{
            steering.velocity = character.transform.position - targetPosition;
        }
             
        steering.velocity.Normalize();
       
        steering.velocity *= maxspeed;

      
        character.GetNewOrietation(steering.velocity);

        steering.rotation = 0f;
       
        return steering;

    }
    
}

