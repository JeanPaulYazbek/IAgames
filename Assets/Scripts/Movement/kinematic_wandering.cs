using UnityEngine;
using System;

public class KinematicWandering {

    public Kinetics character;
    public float maxspeed;
    public float maxrotation;

    public KinematicWandering(Kinetics Character, float Maxspeed, float Maxrotation) {
        character = Character;
        maxspeed = Maxspeed;
        maxrotation = Maxrotation;
    }


    //funcion que realiza seeking de un punto y se detiene si lo alcanza en un radio
    //dado
    public KinematicSteeringOutput getSteering(){

        
        //velocidades de salida
        KinematicSteeringOutput steering = new KinematicSteeringOutput();

        //sacamos la nueva velocidad en base a la orientacion
        steering.velocity = maxspeed*character.OrientationToVector();

        //actualizamos la rotacion al azar
        steering.rotation = (UnityEngine.Random.Range(-1.0f, 1.0f))*maxrotation;

        return steering;
   
        

    }

    
    
    
}

