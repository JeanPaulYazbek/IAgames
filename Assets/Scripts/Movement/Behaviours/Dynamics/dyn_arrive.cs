using UnityEngine;


public class dyn_arrive : MonoBehaviour
{



    //Los objetos necesarios enemigo y agente
    public string enemyName;
    static_data enemy;
    public static_data agent;

    //Estructuras estaticas del agente
    public Kinetics kineticsAgent;
    public SteeringOutput steeringAgent;
    float maxspeed;
    public Kinetics kineticsEnemy;


     //valores por defecto de d arrive 

    public float targetRadius = 1f;
    public float slowRadius = 30f;

    public float timeTotarget = 0.1f;
    public float MaxAcceleration = 20f;

    //Movimientos
    public Arrive arrive;


    void Start (){

        //Inicializamos las estructuras necesarias de otros componentes
        enemy = GameObject.Find(enemyName).GetComponent<static_data>();
        kineticsEnemy = enemy.kineticsAgent;

        kineticsAgent = agent.kineticsAgent;
        steeringAgent = agent.steeringAgent;
        maxspeed = agent.maxspeed;

        //Inicializamos movimientos
        arrive = new Arrive(kineticsAgent, kineticsEnemy, MaxAcceleration, maxspeed, targetRadius, slowRadius,timeTotarget);
    }

    void Update (){

        
        //Perseguimos al enemigo
        // con arrive accels
        steeringAgent.UpdateSteering(arrive.getSteering());

        
        
        
    }

}
