using UnityEngine;


//este componenete permite a cualquier cosa que se mueva (por ejemplo un trainer o un pokemon)
//actualizar su posicion y guardar todos los datos encesarios para ello
public class static_data : MonoBehaviour
{


    //Estructuras estaticas del agente
    public Kinetics kineticsAgent;
    public SteeringOutput steeringAgent;



    //Valores por defecto para las kinetics
    public Vector3 defaultSpeed = new Vector3(1,0,0);
    public float defaultRotation = 30f;
    public Vector3 defaultLinear = new Vector3(10,10,0);
    public float defaultAngular = 60f;
    public float maxspeed = 50f;


    //extra

    public bool flocker = false;//esto solo se pone en true si eres un pokemon que usa el comportamiento flock
    public bool jumper = false;//esto es true si te puedes mover en z

    void Awake (){

        //Inicializamos las estructuras necesarias
        steeringAgent = new SteeringOutput(defaultLinear, defaultAngular);
        kineticsAgent = new Kinetics(defaultRotation, defaultSpeed, transform);

    }

    void Update() {
        kineticsAgent.UpdateKinetics(steeringAgent, Time.deltaTime,maxspeed, jumper);
    }



}
