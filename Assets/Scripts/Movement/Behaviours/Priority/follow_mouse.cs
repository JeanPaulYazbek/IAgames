using UnityEngine;
using System.Collections.Generic;

//analogo a avoid_with_arrive pero el target de arrive siempre es el mouse
public class follow_mouse : MonoBehaviour
{

    //MIS ESTRUCTURAS
    public static_data agent;
    public Kinetics kineticsAgent;
    public SteeringOutput steeringAgent;

    //ARRIVE 
    
    //Los objetos necesarios enemigo y agente
    GameObject enemy;//gameobject dummy que solo usaremos para tener un transform, y guardar la posicion del mouse ahi
    Kinetics kineticsEnemy;


    public float targetRadiusArrive = 1f;
    public float slowRadiusArrive = 30f;

    public float timeTotargetArrive = 0.1f;
    public float maxAccelerationArrive = 20f;

    float maxspeedArrive;


    public Arrive arrive;
    

    //AVOID 

    //Los objetos necesarios de obstaculos
    List<Transform> obs_trans = new List<Transform>();
    GameObject[] obstacles;

    public float avoidDistance = 10f;    
    public float lookAheadAvoid = 2f;
    public ObstacleAvoidance obstacleAvoidance;
    


    //FACE 

    //usando el mismo enemy que arrive
    public Face face;
   
    

    //BLENDINGs

    public float maxAccelBlend = 30f;
    public float maxAngularBlend = 10f;

    Behavior[] behaviorsAvoid = new Behavior[1];//este tendra solo avoid
    Behavior[] behaviorsArrive = new Behavior[2];//en este mezclaremos arrive y face


    BlendedSteering blendAvoid; 
    BlendedSteering blendArrive; 

    
    //PRIORITY

    BlendedSteering[] groups =  new BlendedSteering[2];//solo combinaremos dos grupos

    PrioritySteering priority;



    // Start is called before the first frame update
    void Start()
    {
        //DATOS DE PERSONAJE
        kineticsAgent = agent.kineticsAgent;
        steeringAgent = agent.steeringAgent;


        //ARRIVE INITIALIZATION

        enemy = new GameObject();//creamos un enemigo ficticio que sera el mouse
        enemy.transform.position = Vector3.zero;
        kineticsEnemy = new Kinetics(0f, Vector3.zero, enemy.transform);

        arrive = new Arrive(kineticsAgent, kineticsEnemy, maxAccelerationArrive, agent.maxspeed, targetRadiusArrive, slowRadiusArrive,timeTotargetArrive);
        arrive.weigth = 1f;

        //AVOID INITIALIZATION

        obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        
        for (int i = 0; i<obstacles.Length; i++){
            obs_trans.Add(obstacles[i].transform);
        }

        Transform[] obstacles_transforms = obs_trans.ToArray();
        
        obstacleAvoidance = new ObstacleAvoidance(kineticsAgent, obstacles_transforms, avoidDistance, lookAheadAvoid, steeringAgent);
        obstacleAvoidance.weigth = 1f;
       
        //FACE INITIALIZATION

        face = new Face(kineticsAgent, kineticsEnemy);
        face.weigth = 1f;
       


        //BLEND INITIALIZATION

        behaviorsArrive[0] = arrive;
        behaviorsArrive[1] = face;

        behaviorsAvoid[0] = obstacleAvoidance;


        blendArrive = new BlendedSteering(behaviorsArrive, maxAccelBlend, maxAngularBlend);
        blendAvoid = new BlendedSteering(behaviorsAvoid, maxAccelBlend, maxAngularBlend);

        //PRIORITY INITIALIZATION

        groups[0] = blendAvoid;//OJO avoid tiene prioridad por eso va primero
        groups[1] = blendArrive;
        
        priority = new PrioritySteering(groups, steeringAgent);
        
        
        

        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 screenPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        screenPoint.z = 0f;
        kineticsEnemy.transform.position = screenPoint;
        steeringAgent.UpdateSteering(priority.getSteering());
        
        
    }
}
