using System;
using UnityEngine;
public class PrioritySteering {

    //vamos a agrupar diferentes behaviors en diferentes blends
    //OJO: el orden en el que estan en el arreglo tambien les asigna una prioridad
    BlendedSteering[] groups;

    int n ;//cantidad de grupos 

    //numero pequenno que usaremos para saber cuando el resultado de un grupo vale la pena
    float epsilon;

    public PrioritySteering(BlendedSteering[] Groups){
        groups = Groups;
        epsilon = 0.1f;
        n = groups.Length;

    }

    public SteeringOutput getSteering(){

     
        SteeringOutput steering = new SteeringOutput(Vector3.zero, 0f);
        for(int i = 0; i<n; i++){
            //sacamos el steering de un grupo
            steering = groups[i].getSteering();

            //si alguna de las aceleraciones del grupo es lo suficientemente relevante
            if (steering.linear.magnitude > epsilon || Math.Abs(steering.angular) > epsilon){

                return steering;
            }
            
        }

        //si ninguna era relevante devolvemos la ultima
        return steering;


    }
}