using UnityEngine;
using System.Collections.Generic;

//componente que permite mantenerse alejado de una lista de targets
public class dyn_separation : MonoBehaviour
{



    //Los objetos necesarios enemigo y agente
    public string[] targets_names;

    List<Kinetics> targetsKins = new List<Kinetics>();


    //Estructuras estaticas del agente
    public static_data agent;
    public Kinetics kineticsAgent;
    public SteeringOutput steeringAgent;


    //valores por defecto de separation
    public float threshold = 5f;    
    public float maxAccel = 5f;


    //Movimientos
    public Separation separation;


    void Start (){

        static_data target;
        //Inicializamos las estructuras necesarias de otros componentes
        for (int i = 0; i<targets_names.Length; i++){
            target = GameObject.Find(targets_names[i]).GetComponent<static_data>();
            targetsKins.Add(target.kineticsAgent);
        }
    
        kineticsAgent = agent.kineticsAgent;
        steeringAgent = agent.steeringAgent;

        Kinetics[] targets =  targetsKins.ToArray();
        //Inicializamos movimientos
        separation =  new Separation(kineticsAgent, targets, threshold, maxAccel);

    }

    void Update (){

        //Perseguimos al enemigo
        // con seek aceleracion
        steeringAgent.UpdateSteering(separation.getSteering());
    }

}
