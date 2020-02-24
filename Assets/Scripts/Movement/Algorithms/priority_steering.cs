using System;
using UnityEngine;
public class PrioritySteering {

    //hay un caso muy particular en el cual algun comportamiento no devuelve
    //cambios pero para reflejarlo ese comportamiento manda el mismo steering 
    //que el personaje ya tiene, para poder incluir este caso en los que estan 
    //dentro del if de epsilon (este caso tambien representa no cambio) 
    //debemos poder comparar con el personaje actual
    SteeringOutput characterSteering;

    //vamos a agrupar diferentes behaviors en diferentes blends
    //OJO: el orden en el que estan en el arreglo tambien les asigna una prioridad
    BlendedSteering[] groups;

    int n ;//cantidad de grupos 

    //numero pequenno que usaremos para saber cuando el resultado de un grupo vale la pena
    float epsilon;

    public PrioritySteering(BlendedSteering[] Groups, SteeringOutput CharacterSteering){
        groups = Groups;
        epsilon = 0.1f;
        n = groups.Length;
        characterSteering = CharacterSteering;

    }

    public SteeringOutput getSteering(){

     
        SteeringOutput steering = new SteeringOutput(Vector3.zero, 0f);
        for(int i = 0; i<n; i++){
            //sacamos el steering de un grupo
            steering = groups[i].getSteering();
           

            //si alguna de las aceleraciones del grupo es lo suficientemente relevante
            if ((steering.linear.magnitude > epsilon || Math.Abs(steering.angular) > epsilon)
            && (steering.linear != characterSteering.linear || steering.angular != characterSteering.angular)){

                return steering;
            }
            
        }

        //si ninguna era relevante devolvemos la ultima
        return steering;


    }
}