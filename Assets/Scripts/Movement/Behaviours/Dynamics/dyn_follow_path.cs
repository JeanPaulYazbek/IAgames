using UnityEngine;

//componente que te hace seguir un path en mi caso el seno es el path
public class dyn_follow_path : MonoBehaviour
{
    public static_data agent;

    //Estructuras estaticas del agente
    public Kinetics kineticsAgent;
    public SteeringOutput steeringAgent;

    public float maxSeekAccel = 10f;
    public float pathOffset = 1f; // que tan lejos va estar el target imaginario

    public FollowPath follow;


    // Start is called before the first frame update
    void Start()
    {
        kineticsAgent = agent.kineticsAgent;
        steeringAgent = agent.steeringAgent;

        follow = new FollowPath(kineticsAgent, maxSeekAccel, pathOffset);
        
    }

    // Update is called once per frame
    void Update()
    {
        //seguimos el camino
        steeringAgent.UpdateSteering(follow.getSteering());
        
    }
}
