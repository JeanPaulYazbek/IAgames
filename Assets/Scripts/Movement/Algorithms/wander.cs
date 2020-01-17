using UnityEngine;
using System;

public class Wander : Face
{

    public float wanderOffset;
    public float wanderRadius;
    public float wanderRate;//lo mas que puedes cambair la orientacion
    public float wanderOrientation;//la orientacion de target
    public float maxAccel;

    public Wander(Kinetics Character,float WanderOffset, float WanderRadius, float WanderRate, float WanderOrientation, float MaxAccel)
    : base(Character, Character)
    {
        wanderOffset = WanderOffset;
        wanderRadius = WanderRadius;
        wanderRate = WanderRate;
        wanderOrientation = WanderOrientation;
        maxAccel = MaxAccel;
    }

    public SteeringOutput getSteeringW(){

        SteeringOutput steering = new SteeringOutput(Vector3.zero, 0f);

        //ajustamos una orientacion al azar dentro del rango
        wanderOrientation += (UnityEngine.Random.Range(-1.0f, 1.0f))*wanderRate;

        float targetOrientation = wanderOrientation + character.transform.eulerAngles.z;

        // el comienzo del circulo va a estar al frente
        // de nuestro character 
        Vector3 targetPos = character.transform.position + 
                            wanderOffset*character.OrientationToVector();

        // giramos un poco mas las posicion en base al radio
        Vector3 orienAsVector = new Vector3(-(float)Math.Sin(targetOrientation/53), (float)Math.Cos(targetOrientation/53),0f);
        targetPos += wanderRadius *  orienAsVector;

        //actualiza aceleracion angular
        steering = getSteeringF2(targetPos);

        //despues de unas llamadas ya tendremos velocidad angular
        //osea que la orientacion cambia, aceleramos hacia esa direccion
        steering.linear = maxAccel*character.OrientationToVector();

        return steering;

        

    }

}