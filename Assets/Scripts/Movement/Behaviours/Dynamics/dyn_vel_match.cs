using UnityEngine;


public class dyn_vel_match : MonoBehaviour
{



    //Los objetos necesarios enemigo y agente
    public string enemyName;
    static_data enemy;
    public Kinetics kineticsEnemy;

    //Estructuras estaticas del agente
    public static_data agent;
    public Kinetics kineticsAgent;
    public SteeringOutput steeringAgent;
    


     //valores por defecto de d arrive 

    public float timeTotarget = 0.1f;
    public float MaxAcceleration = 20f;

    //Movimientos
    public VelocityMatch velMatch;


    void Start (){

        //Inicializamos las estructuras necesarias de otros componentes
        enemy = GameObject.Find(enemyName).GetComponent<static_data>();
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
