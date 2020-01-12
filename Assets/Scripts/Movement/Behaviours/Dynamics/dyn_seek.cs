using UnityEngine;


public class dyn_seek : MonoBehaviour
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
    public float maxSeekAccel = 10f;
    public int seek_or_flee = 1;

    //Movimientos
    public Seek seek; 


    void Start (){

        //Inicializamos las estructuras necesarias de otros componentes
        enemy = GameObject.Find("Enemy").GetComponent<static_data>();
        kineticsEnemy = enemy.kineticsAgent;

        kineticsAgent = agent.kineticsAgent;
        steeringAgent = agent.steeringAgent;
        maxspeed = agent.maxspeed;

        //Inicializamos movimientos
        seek = new Seek(kineticsAgent, kineticsEnemy, maxSeekAccel);

    }

    void Update (){

        
        kineticsAgent.UpdateKinetics(steeringAgent, Time.deltaTime,maxspeed);

        //Perseguimos al enemigo
        // con seek aceleracion
        steeringAgent.UpdateSteering(seek.getSteering(seek_or_flee));

        
        
        
    }

}
