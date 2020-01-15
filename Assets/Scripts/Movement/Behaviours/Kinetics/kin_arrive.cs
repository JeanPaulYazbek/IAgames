using UnityEngine;


public class kin_arrive : MonoBehaviour
{



    //Los objetos necesarios enemigo y agente
    public string enemyName;
    static_data enemy;
    public static_data agent;
    
    float maxspeed;

    //Estructuras estaticas del agente
    public Kinetics kineticsAgent;
    public SteeringOutput steeringAgent;
    public Kinetics kineticsEnemy;


    //valores por defecto de k arrive
    public float timeTotarget = 2f;
    public float radius = 2f;
    public float maxSeekSpeed = 10f;

    //Movimientos
    public KinematicArrive kinematicArrive; 


    void Start (){

        //Inicializamos las estructuras necesarias de otros componentes
        enemy = GameObject.Find(enemyName).GetComponent<static_data>();
        kineticsEnemy = enemy.kineticsAgent;

        kineticsAgent = agent.kineticsAgent;
        steeringAgent = agent.steeringAgent;
        maxspeed = agent.maxspeed;

        //Inicializamos movimientos
        kinematicArrive = new KinematicArrive(kineticsAgent, kineticsEnemy, maxSeekSpeed, timeTotarget, radius);

    }

    void Update (){

        

        //Perseguimos al enemigo
        // con seek velocidad
        kineticsAgent.UpdateSteeringOutput(kinematicArrive.getSteering());

        
        
        
    }

}
