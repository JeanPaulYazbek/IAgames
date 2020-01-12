using UnityEngine;


public class kin_seek : MonoBehaviour
{



    //Los objetos necesarios enemigo y agente
    static_data enemy;
    public static_data agent;

    //Estructuras estaticas del agente
    public Kinetics kineticsAgent;
    public SteeringOutput steeringAgent;
    float maxspeed;
    public Kinetics kineticsEnemy;


    //valores por defecto de los seek
    public float maxSeekSpeed = 10f;

    //Movimientos
    public KinematicSeek kinematicSeek; 


    void Start (){

        //Inicializamos las estructuras necesarias de otros componentes
        enemy = GameObject.Find("Enemy").GetComponent<static_data>();
        kineticsEnemy = enemy.kineticsAgent;

        kineticsAgent = agent.kineticsAgent;
        steeringAgent = agent.steeringAgent;
        maxspeed = agent.maxspeed;

        //Inicializamos movimientos
        kinematicSeek = new KinematicSeek(kineticsAgent, kineticsEnemy, maxSeekSpeed);
       

    }

    void Update (){

        

        //Perseguimos al enemigo
        // con seek velocidad
        kineticsAgent.UpdateSteeringOutput(kinematicSeek.getSteering(1));

        
        
        
    }

}
