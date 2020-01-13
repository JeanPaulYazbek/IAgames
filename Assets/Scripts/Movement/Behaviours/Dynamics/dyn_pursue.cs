using UnityEngine;


public class dyn_pursue : MonoBehaviour
{



    //Los objetos necesarios enemigo y agente
    static_data enemy;
    public static_data agent;

    //Estructuras estaticas del agente
    public Kinetics kineticsAgent;
    public SteeringOutput steeringAgent;
    public Kinetics kineticsEnemy;


    //valores por defecto de los seek
    public float maxPrediction = 10f;

    //Movimientos
    public Pursue pursue; 

    public int seek_or_flee = 1;


    void Start (){

        //Inicializamos las estructuras necesarias de otros componentes
        enemy = GameObject.Find("Enemy").GetComponent<static_data>();
        kineticsEnemy = enemy.kineticsAgent;

        kineticsAgent = agent.kineticsAgent;
        steeringAgent = agent.steeringAgent;

        //Inicializamos movimientos
        pursue = new Pursue(kineticsAgent, kineticsEnemy, maxPrediction);

    }

    void Update (){

        
        //Perseguimos al enemigo
        // con seek aceleracion
        steeringAgent.UpdateSteering(pursue.getSteeringP(seek_or_flee));

        
        
        
    }

}
