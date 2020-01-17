using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dyn_wander : MonoBehaviour
{

    
    public static_data agent;

    //Estructuras estaticas del agente
    public Kinetics kineticsAgent;
    public SteeringOutput steeringAgent;

    //datos del wander

    public float wanderOffset = 0f;
    public float wanderRadius = 20f;
    public float wanderRate = 20f;//lo mas que puedes cambair la orientacion
    float wanderOrientation;//la orientacion de target
    public float maxAccel = 1f;


    public Wander wander;
    // Start is called before the first frame update
    void Start()
    {
        kineticsAgent = agent.kineticsAgent;
        steeringAgent = agent.steeringAgent;

        //el target ficticio empieza con una orientacion al azar
        wanderOrientation = (UnityEngine.Random.Range(-1.0f, 1.0f))*360;

        //comenzamos con una orientacion del personaje al azar
        kineticsAgent.SetRandomOrientation();

        
        wander = new Wander(kineticsAgent, wanderOffset, wanderRadius, wanderRate, wanderOrientation, maxAccel);
        
    }

    // Update is called once per frame
    void Update()
    {
        steeringAgent.UpdateSteering(wander.getSteeringW());
        
    }
}
