using UnityEngine;
using System.Collections.Generic;

public class shoot : MonoBehaviour
{

    public GameObject pokeBallPrefab;//aqui va el modelo de una pokeball
    public float ballSpeed;//magnitud de velocidad de lanzamiento

    GameObject pokeBall;//aqui guardaremos la instancia de una pokeball

    
    //Aqui guardaremos los pokemones que pueden ser atrapados
    List<Kinetics> pokemons_list = new List<Kinetics>();
    Kinetics[] pokemons_kins;

    //obstaculos que evadir
    List<Transform> obstacles_list = new List<Transform>();
    Transform[] obstacles;
    
    // Start is called before the first frame update
    void Start()
    {
        //POKEMONS
        GameObject[] pokemons = GameObject.FindGameObjectsWithTag("Pokemon");
        for (int i = 0; i<pokemons.Length; i++){
            
            pokemons_list.Add(pokemons[i].GetComponent<static_data>().kineticsAgent);
           
        }
        pokemons_kins =  pokemons_list.ToArray();

        //OBSTACLES
        GameObject[] obstaclesGo = GameObject.FindGameObjectsWithTag("Obstacle");
        for (int i = 0; i<obstaclesGo.Length; i++){
            
            obstacles_list.Add(obstaclesGo[i].GetComponent<Transform>());
           
        }

        obstacles = obstacles_list.ToArray();

    }

    // Update is called once per frame
    void Update()
    {
 
        if (Input.GetKey("x") && !pokeBall) {//si presionamos x y ya termino la ultima pokeBall lanzada
            
            //creamos un pokeball en el mismo lugar que esta el character que la lanza.
            pokeBall = Instantiate(pokeBallPrefab, transform.position, Quaternion.identity);
            //le pasamos la lista de pokemones y obstaculos
            pokeBall.GetComponent<static_shoot>().pokemons = pokemons_kins;
            pokeBall.GetComponent<static_shoot>().obstacles = obstacles;
      
        }

        
        

    }
}
