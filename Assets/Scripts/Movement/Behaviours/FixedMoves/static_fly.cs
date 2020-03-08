using UnityEngine;
using System;

//este componente hace que un pokemon vuele de un punto a otro
//el 
public class static_fly : MonoBehaviour
{


    //Estructuras estaticas del agente
    public Kinetics kineticsAgent;
    public SteeringOutput steeringAgent;
    public static_data agent;



    //Puntos a perseguir
    public Vector3 pointA;
    public Vector3 pointB;

    Vector3 currentTarget;
    Vector3 otherTarget;

    //valores por defecto de seek
    public float maxSeekAccel = 10f;
    Seek seek;
   

    //medidas
    public float minSize = 0.8f;


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

        float height = transform.position.z;

        //si alcanzamos un punto debemos cambiar de target al otro punto
        float distance = (currentTarget - kineticsAgent.transform.position).magnitude;

        if (distance <= 1f){//intercambiamos targets
            Vector3 swap = currentTarget;
            currentTarget = otherTarget;
            otherTarget = swap;
        }

        //ajustamos el tamanno del pokemon de acuerdo a la altura
        //float targetSize = Math.Abs(height)*(0.4f);
        float targetSize = (float)Math.Log(Math.Abs(height));
        if (targetSize < minSize){
            targetSize = minSize;
        }

        float caught = 1f;
        if(transform.localScale.x == 0){//si ya lo atraparon no queremos seguir cambiando su size
            caught = 0f;
        }
        transform.localScale = new Vector3(targetSize*caught , targetSize*caught , 1f*caught);

        //hacemos seek al punto actuall
        steeringAgent.UpdateSteering(seek.getSteering2(currentTarget, 1));
       
      
    }

}
