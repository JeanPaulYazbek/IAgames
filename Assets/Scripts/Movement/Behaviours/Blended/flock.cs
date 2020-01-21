using UnityEngine;
using System.Collections.Generic;

public class flock : MonoBehaviour
{

    //MIS ESTRUCTURAS
    public static_data agent;
    public Kinetics kineticsAgent;
    public SteeringOutput steeringAgent;

    //SEPARATION ARGS
    public string[] targets_names_sepa;
    List<Kinetics> targets_kin_sepa = new List<Kinetics>();

    public float threshold = 5f;    
    public float maxAccelSep = 5f;

    public Separation separation;

    //VELOCITY MATCH ARG

    public string enemyNameVel;
    static_data enemyVel;
    public Kinetics kineticsEnemyVel;

    public float timeTotargetVel = 0.1f;
    public float MaxAccelVel = 5f;

    public VelocityMatch velMatch;


    //COHESION ARGS

    public string[] targets_names_cohe;
    List<Transform> targets_trans_cohe = new List<Transform>();

    public float maxAccelCohe = 5f;
    public float maxSpeedCohe ;

    public float targetRadiusCohe = 1f;//radio pequenno
    public float slowRadiusCohe = 5f;//radio grande

    public float timeToTargetCohe = 0.1f;

    public Cohesion cohesion;

    //BLENDING ARGS

    public float maxAccelBlend = 30f;
    public float maxAngularBlend = 10f;




    // Start is called before the first frame update
    void Start()
    {
        //DATOS DE PERSONAJE
        kineticsAgent = agent.kineticsAgent;
        steeringAgent = agent.steeringAgent;


        //SEPARATION INITIALIZATION

        static_data targetSep;
        //Inicializamos las estructuras necesarias de otros componentes
        for (int i = 0; i<targets_names_sepa.Length; i++){
            targetSep = GameObject.Find(targets_names_sepa[i]).GetComponent<static_data>();
            targets_kin_sepa.Add(targetSep.kineticsAgent);
        }

        Kinetics[] targets_sep =  targets_kin_sepa.ToArray();
        separation =  new Separation(kineticsAgent, targets_sep, threshold, maxAccelSep);

        separation.weigth = 2f;


        //VELOCITY MATCH INITIALIZATION

        enemyVel = GameObject.Find(enemyNameVel).GetComponent<static_data>();
        kineticsEnemyVel = enemyVel.kineticsAgent;

        velMatch = new VelocityMatch(kineticsAgent, kineticsEnemyVel, MaxAccelVel,timeTotargetVel);

        velMatch.weigth = 1f;


        //COHESION INITIALIZATION

        static_data targetCohe;
        //Inicializamos las estructuras necesarias de otros componentes
        for (int i = 0; i<targets_names_cohe.Length; i++){
            targetCohe = GameObject.Find(targets_names_cohe[i]).GetComponent<static_data>();
            targets_trans_cohe.Add(targetCohe.transform);
        }

        Transform[] targets_cohe = targets_trans_cohe.ToArray();

        maxSpeedCohe = agent.maxspeed;
        cohesion = new Cohesion(kineticsAgent,targets_cohe, maxAccelCohe,maxSpeedCohe,targetRadiusCohe,slowRadiusCohe, timeToTargetCohe );

        cohesion.weigth = 1f;


        //BLEND INITIALIZATION
        //mezclamos nuestros comportamientos para crear fllock
        /**behaviors[0] = separation;
        behaviors[1] = velMatch;
        behaviors[2] = cohesion;
        
        blendFlock =  new BlendedSteering(behaviors, maxAccelBlend, maxAngularBlend);
**/
        
    }

    // Update is called once per frame
    void Update()
    {
        SteeringOutput steering = new SteeringOutput(Vector3.zero, 0f);
        SteeringOutput steering_target;

        steering_target = velMatch.getSteering();
        steering.linear += velMatch.weigth * steering_target.linear;
        steering.angular += velMatch.weigth * steering_target.angular;
    

        steering_target = separation.getSteering();
        steering.linear += separation.weigth * steering_target.linear;
        steering.angular += separation.weigth * steering_target.angular;

    
        steering_target = cohesion.getSteering();
        steering.linear += cohesion.weigth * steering_target.linear;
        steering.angular += cohesion.weigth * steering_target.angular;
      

        if(steering.linear.magnitude > maxAccelBlend){
            steering.linear.Normalize();
            steering.linear *= maxAccelBlend;
        }

        if(steering.angular > maxAngularBlend){
            steering.angular = maxAngularBlend;
        }

        steeringAgent.UpdateSteering(steering);
    }
}
