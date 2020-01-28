using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//componente que agregado a cualquier cosa que tenga un transform hace que haga align al target
public class dyn_align : MonoBehaviour
{

    //Los objetos necesarios enemigo y agente
    public string enemyName;
    static_data enemy;
    public static_data agent;

    //Estructuras estaticas del agente
    public Kinetics kineticsAgent;
    public SteeringOutput steeringAgent;

    public Kinetics kineticsEnemy;

    //valores por defecto del align
    public float maxAngularAccel = 10f;
    public float maxRotation = 50f;
    //radio de angulo aceptable
    public float targetRadius = 1f;
    public float slowRadius = 30f;
    public float timeToTarget = 0.1f;


    //Movimientos
    public Align align; 
    
    // Start is called before the first frame update
    void Start()
    {
        //Inicializamos las estructuras necesarias de otros componentes
        enemy = GameObject.Find(enemyName).GetComponent<static_data>();
        kineticsEnemy = enemy.kineticsAgent;

        kineticsAgent = agent.kineticsAgent;
        steeringAgent = agent.steeringAgent;

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
