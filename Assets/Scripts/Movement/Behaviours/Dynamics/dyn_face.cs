using UnityEngine;

public class dyn_face : MonoBehaviour
{

    //Los objetos necesarios enemigo y agente
    static_data enemy;
    public static_data agent;

    //Estructuras estaticas del agente
    public Kinetics kineticsAgent;
    public SteeringOutput steeringAgent;
    public Kinetics kineticsEnemy;

    public Face face;

    // Start is called before the first frame update
    void Start()
    {
        //Inicializamos las estructuras necesarias de otros componentes
        enemy = GameObject.Find("Enemy").GetComponent<static_data>();
        kineticsEnemy = enemy.kineticsAgent;

        kineticsAgent = agent.kineticsAgent;
        steeringAgent = agent.steeringAgent;

        face = new Face(kineticsAgent, kineticsEnemy);
        
    }

    // Update is called once per frame
    void Update()
    {
        steeringAgent.UpdateSteering(face.getSteeringF());
        
    }
}
