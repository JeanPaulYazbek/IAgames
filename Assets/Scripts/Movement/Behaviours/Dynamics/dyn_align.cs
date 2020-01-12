using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dyn_align : MonoBehaviour
{

    //Los objetos necesarios enemigo y agente
    static_data enemy;
    public static_data agent;

    //Estructuras estaticas del agente
    public Kinetics kineticsAgent;
    public SteeringOutput steeringAgent;
    float maxspeed;
    public Kinetics kineticsEnemy;

    //valores por defecto del align
    public float maxAngularAccel = 10f;
    public float maxRotation = 50f;
    //radio de angulo aceptable
    public float targetRadius = 2f;
    public float slowRadius = 30f;
    public float timeToTarget = 0.1f;


    //Movimientos
    public Align align; 
    
    // Start is called before the first frame update
    void Start()
    {
        //Inicializamos las estructuras necesarias de otros componentes
        enemy = GameObject.Find("Enemy").GetComponent<static_data>();
        kineticsEnemy = enemy.kineticsAgent;

        kineticsAgent = agent.kineticsAgent;
        steeringAgent = agent.steeringAgent;
        maxspeed = agent.maxspeed;

        //Inicializamos movimientos
        align = new Align(kineticsAgent, kineticsEnemy, maxAngularAccel, maxRotation, targetRadius, slowRadius, timeToTarget);
    }

    // Update is called once per frame
    void Update()
    {
 

        // igualamos orietacion
        steeringAgent.UpdateSteering(align.getSteering());

    }
}
