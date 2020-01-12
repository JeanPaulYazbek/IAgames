using UnityEngine;


public class dyn_vel_match : MonoBehaviour
{



    //Los objetos necesarios enemigo y agente
    static_data enemy;
    public static_data agent;

    //Estructuras estaticas del agente
    public Kinetics kineticsAgent;
    public SteeringOutput steeringAgent;
    public Kinetics kineticsEnemy;


     //valores por defecto de d arrive 

    public float timeTotarget = 0.1f;
    public float MaxAcceleration = 20f;

    //Movimientos
    public VelocityMatch velMatch;


    void Start (){

        //Inicializamos las estructuras necesarias de otros componentes
        enemy = GameObject.Find("Enemy").GetComponent<static_data>();
        kineticsEnemy = enemy.kineticsAgent;

        kineticsAgent = agent.kineticsAgent;
        steeringAgent = agent.steeringAgent;

        //Inicializamos movimientos
        velMatch = new VelocityMatch(kineticsAgent, kineticsEnemy, MaxAcceleration,timeTotarget);
    }

    void Update (){

        //Perseguimos al enemigo
        // con arrive accels
        steeringAgent.UpdateSteering(velMatch.getSteering());
        
        
        
    }

}
