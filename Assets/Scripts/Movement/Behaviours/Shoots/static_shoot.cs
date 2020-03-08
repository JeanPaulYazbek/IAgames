using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ananlogo a static data pero esta hecha para datos de cosas que sean lanzadas(por ejemplo poke balls)
public class static_shoot : MonoBehaviour
{

    //DATOS PARA LA POKE BALL
    public GameObject staticBallPrefab;//imagen de pokeball para dejar ahi cuando atrapemos un poke
    public GameObject ultraBallPrefab;//imagen de ultraball

    KineticsShoot kineticsBall;//necesitamos una estructura que almacena nuestra informacion de ball
    const float gravity = 9.8f;
    public Vector3 direction;//este vector sera normal en la direccion en la que lanzamos la bola
    public float speed;//magnitud de velocidad de los disparos
    int[] destroy;//necesitamos saber si debemos ser destruidos

    public bool ultraBall = false;//debe ser true si estamos usando ultra balls y no poke balls

    //EXTERNOS
    public Kinetics[] pokemons;//necesiitamos una lista de pokemons para saber si los podemos atrapar
    public GameObject[] pokemonsObjs;//estos sera utiles cuando queramos deshabilitar los pokemones 
    public pokemon_data[] pokemonsData;//aqui esta cuanto vale cada pokemon
    public Transform[] obstacles;//necesitamos los obstaculos para cuando choquemos con ellos 

    public point_manager pointManager;//a el le notificaremos si un pokemon es atrapado

    
    // Start is called before the first frame update
    void Start()
    {
        kineticsBall = new KineticsShoot(speed, new Vector3(direction.x,direction.y,direction.z), new Vector3(0f,0f,gravity), transform, pokemons, obstacles);   
    }

    // Update is called once per frame
    void Update()
    {
        destroy = kineticsBall.UpdateKinetics(Time.deltaTime);
        int option = destroy[0];
        int pokemonIndex = destroy[1];
        if(option==1){//si pisamos el suelo
            Object.Destroy(this.gameObject); //desaparecemos completamente la pokeball
        }
        if(option==2){//si le pegamos a un pokemon
            Transform caught_poke = pokemons[pokemonIndex].transform;
            if(caught_poke.localScale.x > 0){//si no fue atrapado ya

                Vector3 ballPos = transform.position;
                ballPos.z = 0.1f;//esto es para que los personajes pasen por encima de la ball
                //PONEMOS UNA BALL QUIETA
                if(ultraBall){//si es una ultraball ponemos una ultra ball
                    Instantiate(ultraBallPrefab, ballPos, Quaternion.identity);
                }else{
                    Instantiate(staticBallPrefab, ballPos, Quaternion.identity);
                }
                //HACEMOS QUE EL POKEMON DESAPAREZCA
                DisablePokemon(pokemonsObjs[pokemonIndex], caught_poke);
                //ACTUALZAMOS PUNTUACION
                UpdateScore(pokemonIndex);

                
            }
            Object.Destroy(this.gameObject);
        }
        
    }

    //Funcion que deshabilita todos los componentes de un pokemon dados y po
    void DisablePokemon(GameObject pokemonObject, Transform pokemonTrans){

        //Reducimos el tamanno del pokemon a 0, asi los otros componentes sabran que desaparecio
        pokemonTrans.localScale = Vector3.zero;

        //Desactivamos todos sus compoenentes pero su transform sigue existiendo 
        //para efectos de los otros componentes sera como si estuviera quieto en donde se le atrapo
        MonoBehaviour[] comps = GameObject.Find(pokemonObject.name).GetComponents<MonoBehaviour>();
        foreach(MonoBehaviour c in comps)
        {
            c.enabled = false;
        }
       

    }

    void UpdateScore(int pokemonIndex){
        int value = pokemonsData[pokemonIndex].value;
        // Si la bola la lanzo el jugador es el index 0
        int ownerIndex = 0;//index para el manejador de quien lanzo la bola
        if(ultraBall){//si la lanzo el rival
            ownerIndex = 1;
        }
        pointManager.UpdateScore(ownerIndex, value);
    }
}
