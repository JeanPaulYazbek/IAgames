using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dyn_look_where : MonoBehaviour
{

    public static_data agent;

    //Estructuras estaticas del agente
    public Kinetics kineticsAgent;
    public SteeringOutput steeringAgent;

    public LookWhereYouAreGoing look;
    // Start is called before the first frame update
    void Start()
    {
        kineticsAgent = agent.kineticsAgent;
        steeringAgent = agent.steeringAgent;

        look = new LookWhereYouAreGoing(kineticsAgent);
        
    }

    // Update is called once per frame
    void Update()
    {
        //miramos hacia donde vayamos
        steeringAgent.UpdateSteering(look.getSteering());
        
    }
}
