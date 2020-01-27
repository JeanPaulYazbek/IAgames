using UnityEngine;
using System;

public class static_jump : MonoBehaviour
{


    //Estructuras estaticas del agente
    public Kinetics kineticsAgent;
    public SteeringOutput steeringAgent;
    public static_data agent;



    //Valores por defecto para las kinetics
    public Vector3 defaultSpeed = new Vector3(0,0,-3f);//esto tiene que ser igual al que pongan a pata en static_data



    //cosas de vuelo
    public float maxHeight = -4f;//altura maxima de vuelo
    public float minHeight = -1f;//altura minima de vuelo


    void Start (){

        //Inicializamos las estructuras necesarias
        
        steeringAgent = agent.steeringAgent;
        kineticsAgent = agent.kineticsAgent;

    }

    void Update() {

        float height = transform.position.z;

        //ajustamos el tamanno del pokemon de acuerdo a la altura
        float targetSize = Math.Abs(height)*(0.4f);
        float minSize = 0.8f;
        if (targetSize < minSize){
            targetSize = minSize;
        }

        float caught = 1f;
        if(transform.localScale.x == 0){//si ya lo atraparon no queremos seguir cambiando su size
            caught = 0f;
        }
        transform.localScale = new Vector3(targetSize*caught , targetSize*caught , 1f*caught);
       
        if(height < maxHeight){//si nos pasamos de la altura maxima
            kineticsAgent.velocity = defaultSpeed*(-1);//usamos velocidad de bajada
        }
        if(height > minHeight){//si bajamos mas que la altura minima
            kineticsAgent.velocity = defaultSpeed;

        }
    }



}
