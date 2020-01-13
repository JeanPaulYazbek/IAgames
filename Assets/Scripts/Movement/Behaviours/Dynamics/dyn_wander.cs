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

    public float wanderOffset = 5f;
    public float wanderRadius = 2f;
    public float wanderRate = 10f;//lo mas que puedes cambair la orientacion
    public float wanderOrientation = 0f;//la orientacion de target
    public float maxAccel = 30f;


    public Wander wander;
    // Start is called before the first frame update
    void Start()
    {
        kineticsAgent = agent.kineticsAgent;
        steeringAgent = agent.steeringAgent;

        wander = new Wander(kineticsAgent, wanderOffset, wanderRadius, wanderRate, wanderOrientation, maxAccel);
        
    }

    // Update is called once per frame
    void Update()
    {
        steeringAgent.UpdateSteering(wander.getSteeringW());
        
    }
}
