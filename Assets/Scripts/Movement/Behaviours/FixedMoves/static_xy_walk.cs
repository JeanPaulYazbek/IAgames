using UnityEngine;
using System;

//este componente hace que un pokemon vuele de un punto a otro
//el 
public class static_xy_walk : MonoBehaviour
{


    //Estructuras estaticas del agente
    Kinetics kineticsAgent;
    SteeringOutput steeringAgent;
    public static_data agent;



    //Puntos a perseguir
    public Vector3 pointA;
    public Vector3 pointB;

    Vector3 currentTarget;
    Vector3 otherTarget;

    //valores por defecto de seek
    public float maxSeekAccel = 10f;
    Seek seek;
   

    void Start (){

        //Inicializamos las estructuras necesarias
        
        steeringAgent = agent.steeringAgent;
        kineticsAgent = agent.kineticsAgent;

        currentTarget = pointA;//default target
        otherTarget = pointB;

        //Inicializamos movimientos
        seek = new Seek(kineticsAgent, kineticsAgent, maxSeekAccel);

    }

    void Update() {

        

        //si alcanzamos un punto debemos cambiar de target al otro punto
        float distance = (currentTarget - kineticsAgent.transform.position).magnitude;

        if (distance <= 1f){//intercambiamos targets
            Vector3 swap = currentTarget;
            currentTarget = otherTarget;
            otherTarget = swap;
        }
        

        //hacemos seek al punto actuall
        steeringAgent.UpdateSteering(seek.getSteering2(currentTarget, 1));
       
      
    }

}
