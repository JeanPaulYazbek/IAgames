using UnityEngine;


public class kin_wander : MonoBehaviour
{



    //Los objetos necesarios enemigo y agente
    public static_data agent;

    //Estructuras estaticas del agente
    public Kinetics kineticsAgent;
    public SteeringOutput steeringAgent;
    float maxspeed;



    //Movimientos
    public KinematicWandering kinWander; 


    void Start (){

        //Inicializamos las estructuras necesarias de otros componentes
        kineticsAgent = agent.kineticsAgent;
        steeringAgent = agent.steeringAgent;
        maxspeed = agent.maxspeed;

        //iniciamos wandering
        kinWander = new KinematicWandering(kineticsAgent,2f,100f);

    }

    void Update (){

        
        //hacemos wandering
        kineticsAgent.UpdateSteeringOutput(kinWander.getSteering());
        
        
        
    }

}
