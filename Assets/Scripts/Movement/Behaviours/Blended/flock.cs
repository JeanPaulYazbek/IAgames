using UnityEngine;
using System.Collections.Generic;
using System;//solo para poder usar Array.FindAll

public class flock : MonoBehaviour
{

    //MIS ESTRUCTURAS
    public static_data agent;
    public Kinetics kineticsAgent;
    public SteeringOutput steeringAgent;

    //SEPARATION ARGS
    //public string[] targets_names_sepa;
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

    //public string[] targets_names_cohe;
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

    Behavior[] behaviors = new Behavior[3];
    BlendedSteering blendFlock;




    // Start is called before the first frame update
    void Start()
    {
        //DATOS DE PERSONAJE
        kineticsAgent = agent.kineticsAgent;
        steeringAgent = agent.steeringAgent;


        //SEPARATION INITIALIZATION

        //buscamos a todos los pokemones 
        GameObject[] allPokemons = GameObject.FindGameObjectsWithTag("Pokemon");
        //vamos a usar solo los pokemones que participan en flock
        GameObject[] pokemons = Array.FindAll(allPokemons, c => c.GetComponent<static_data>().flocker);

        //Inicializamos las estructuras necesarias de otros componentes
        for (int i = 0; i<pokemons.Length; i++){
            if(pokemons[i].name != name ){//solo me interesan los pokemones que no son yo
                targets_kin_sepa.Add(pokemons[i].GetComponent<static_data>().kineticsAgent);
            }
        }

        Kinetics[] targets_sep =  targets_kin_sepa.ToArray();
        separation =  new Separation(kineticsAgent, targets_sep, threshold, maxAccelSep);

        separation.weigth = 3f;


        //VELOCITY MATCH INITIALIZATION

        enemyVel = GameObject.Find(enemyNameVel).GetComponent<static_data>();
        kineticsEnemyVel = enemyVel.kineticsAgent;

        velMatch = new VelocityMatch(kineticsAgent, kineticsEnemyVel, MaxAccelVel,timeTotargetVel);

        velMatch.weigth = 1f;


        //COHESION INITIALIZATION

  
        //Inicializamos las estructuras necesarias de otros componentes
        for (int i = 0; i<pokemons.Length; i++){
            
            targets_trans_cohe.Add(pokemons[i].transform);
        }

        Transform[] targets_cohe = targets_trans_cohe.ToArray();

        maxSpeedCohe = agent.maxspeed;
        cohesion = new Cohesion(kineticsAgent,targets_cohe, maxAccelCohe,maxSpeedCohe,targetRadiusCohe,slowRadiusCohe, timeToTargetCohe );

        cohesion.weigth = 1.5f;


        //BLEND INITIALIZATION
        //mezclamos nuestros comportamientos para crear fllock
        behaviors[0] = separation;
        behaviors[1] = velMatch;
        behaviors[2] = cohesion;
        
        blendFlock =  new BlendedSteering(behaviors, maxAccelBlend, maxAngularBlend);

        
    }

    // Update is called once per frame
    void Update()
    {
        
        steeringAgent.UpdateSteering(blendFlock.getSteering());
        kineticsAgent.GetNewOrietation(kineticsAgent.velocity);
    }
}
