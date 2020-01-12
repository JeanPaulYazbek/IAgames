using UnityEngine;

public class static_enemy : MonoBehaviour
{


    //Estructuras estaticas del agente
    public Kinetics kineticsAgent;
    public SteeringOutput steeringAgent;


    //Valores por defecto para las estructuras
    public Vector3 defaultSpeed = new Vector3(1,0,0);
    public float defaultRotation = 30f;
    public Vector3 defaultLinear = new Vector3(10,10,0);
    public float defaultAngular = 60f;
    public float maxspeed = 50f;

    //movimiento

    public KinematicWandering kinWander;


    void Awake(){

        steeringAgent = new SteeringOutput(defaultLinear, defaultAngular);
        kineticsAgent = new Kinetics(defaultRotation, defaultSpeed, transform);

        //iniciamos wandering
        //kinWander = new KinematicWandering(kineticsAgent,5f,30f);

    }
    void Start (){

        
    }

    void Update (){

        //actualizamos kinetics
        kineticsAgent.UpdateKinetics(steeringAgent, Time.deltaTime, maxspeed);

        //hacemos wandering
        //kineticsAgent.UpdateSteeringOutput(kinWander.getSteering());
       
    

    }

}
