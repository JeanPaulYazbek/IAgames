using UnityEngine;


public class kin_seek : MonoBehaviour
{



    //Los objetos necesarios enemigo y agente
    public string enemyName;
    static_data enemy;
    public static_data agent;

    //Estructuras estaticas del agente
    public Kinetics kineticsAgent;
    public SteeringOutput steeringAgent;
    public Kinetics kineticsEnemy;


    //valores por defecto de los seek
    public float maxSeekSpeed = 10f;

    //Movimientos
    public KinematicSeek kinematicSeek; 


    void Start (){

        //Inicializamos las estructuras necesarias de otros componentes
        enemy = GameObject.Find(enemyName).GetComponent<static_data>();
        kineticsEnemy = enemy.kineticsAgent;

        kineticsAgent = agent.kineticsAgent;
        steeringAgent = agent.steeringAgent;

        //Inicializamos movimientos
        kinematicSeek = new KinematicSeek(kineticsAgent, kineticsEnemy, maxSeekSpeed);
       

    }

    void Update (){

        

        //Perseguimos al enemigo
        // con seek velocidad
        kineticsAgent.UpdateSteeringOutput(kinematicSeek.getSteering(1));

        
        
        
    }

}
