using UnityEngine;

//este componente ayudara a enviar sennales de sonido con intensidad relativas a que tan rapido
//va el personaje que use el componente
public class send_walk_sound : MonoBehaviour {

    //DATOS SENNAL
    Signal signal;

    //DATOS PERSONAJE
    public static_data character;
    Kinetics kinCharacter;

    //DATOS MODALIDAD
    //Modalidad para  la sennal
    Modality modality;
    public string description;// Informacion util como el sub-tipo del sentido que se envia, por ejemplo un olor puede ser tipo Poison
    public float maximumRange;// Que tan lejos puede llegar a lo mas el sonido
    public float attenuation; // Que tanto baja su intensidad a medida que se aleja
    public float inverseTransmissionSpeed;// Cantidad de tiempo (si es tiempo no velocidad) que toma recorrer una unidad de distancia

    //DATOS MANEJADOR DE SENNALES
    RegionalSenseManager manager;
    public sense_manager compManager;

    void Awake(){     
        modality = new SoundModality(maximumRange,attenuation,inverseTransmissionSpeed, description, null);
    }

    
    void Start(){
        kinCharacter = character.kineticsAgent;
    }

    void Update(){


        // Buscamos el manejador
        manager = compManager.senseManager;

        // Calculamos la intensidad del sonido que enviaremos
        float strength = kinCharacter.velocity.magnitude * 2;

        // Creamos una nueva sennal 
        signal = new Signal(strength, transform, modality);

        // Enviamos la sennal cada frame
        manager.AddSignal(signal);
    }



}