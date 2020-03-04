using System.Collections.Generic;
using UnityEngine;
public class sense_manager : MonoBehaviour {

    public RegionalSenseManager senseManager;
    public smell_sensor[] smellSensors;
    void Awake(){

    }

    void Start(){

        List<Sensor> allSensors  = new List<Sensor>();


        // Agregamos los sensores de aromas
        for(int i = 0; i<smellSensors.Length; i++){
            allSensors.Add(smellSensors[i].sensor);
        }

        // Creamos el sense manager con los sensores dados
        senseManager = new RegionalSenseManager(allSensors);


    }
}