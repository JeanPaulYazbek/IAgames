using UnityEngine;
public class send_sight : MonoBehaviour {

    //DATOS SENNAL
    public float strength;//Intensidad inicial desde el origen de la sennal
    Signal signal;

    //DATOS MODALIDAD
    //Modalidad para  la sennal
    SightModality modality;
    public string description;// Informacion util como el sub-tipo del sentido que se envia, por ejemplo un olor puede ser tipo Poison
    public float maximumRange;// Que tan lejos puede llegar a lo mas el olor por ejemplo
    public float attenuation; // Que tanto baja su intensidad a medida que se aleja
    public float inverseTransmissionSpeed;// Cantidad de tiempo (si es tiempo no velocidad) que toma recorrer una unidad de distancia

    //DATOS MANEJADOR DE SENNALES
    public sense_manager compManager;
    RegionalSenseManager manager;

    void Awake(){
        modality = new SightModality(maximumRange,attenuation,inverseTransmissionSpeed, description, null);
        signal = new Signal(strength, transform, modality);

    }


    void Start(){

        //obstaculos
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        obstacle_data[] obstaclesData =  new obstacle_data[obstacles.Length];

        for(int k = 0; k < obstacles.Length; k++){
            obstaclesData[k] = obstacles[k].GetComponent<obstacle_data>();
        }

        modality.obstacles = obstaclesData;

    }


    void Update(){

        manager = compManager.senseManager;
        // Enviamos la sennal cada frame
        manager.AddSignal(signal);
    }

    public void UpdateSignalDescription(string newDescription){
        signal.modality.description = newDescription;
    }


}