using UnityEngine;
using System.Collections.Generic;

//componente que te permite evadir obstaculos
public class dyn_obstacle_avoidance : MonoBehaviour
{



    //Los objetos necesarios de obstaculos
    List<Transform> obs_trans = new List<Transform>();
    GameObject[] obstacles;


    //Estructuras estaticas del agente
    public static_data agent;
    public Kinetics kineticsAgent;
    public SteeringOutput steeringAgent;


    //valores por defecto de obs avoid
    public float avoidDistance = 10f;    
    public float lookAhead = 2f;


    //Movimientos
    public ObstacleAvoidance obstacleAvoidance;


    void Start (){

        

        obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        //Inicializamos las estructuras necesarias de otros componentes
        for (int i = 0; i<obstacles.Length; i++){
            obs_trans.Add(obstacles[i].transform);
        }
    
        kineticsAgent = agent.kineticsAgent;
        steeringAgent = agent.steeringAgent;

        Transform[] obstacles_transforms = obs_trans.ToArray();
        //Inicializamos movimientos
        obstacleAvoidance = new ObstacleAvoidance(kineticsAgent, obstacles_transforms, avoidDistance, lookAhead, steeringAgent);

    }

    void Update (){

        //Perseguimos al enemigo
        // con seek aceleracion
        steeringAgent.UpdateSteering(obstacleAvoidance.getSteering());
    }

}
