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
    public Transform[] obstacles;//necesitamos los obstaculos para cuando choquemos con ellos 

    
    // Start is called before the first frame update
    void Start()
    {
        kineticsBall = new KineticsShoot(speed, new Vector3(direction.x,direction.y,direction.z), new Vector3(0f,0f,gravity), transform, pokemons, obstacles);   
    }

    // Update is called once per frame
    void Update()
    {
        destroy = kineticsBall.UpdateKinetics(Time.deltaTime);
        if(destroy[0]==1){//si pisamos el suelo
            Object.Destroy(this.gameObject); //desaparecemos completamente la pokeball
        }
        if(destroy[0]==2){//si le pegamos a un pokemon
            Transform caught_poke = pokemons[destroy[1]].transform;
            if(caught_poke.localScale.x > 0){//si no fue atrapado ya

                //PONEMOS UNA BALL QUIETA
                if(ultraBall){//si es una ultraball ponemos una ultra ball
                    Instantiate(ultraBallPrefab, transform.position, Quaternion.identity);
                }else{
                    Instantiate(staticBallPrefab, transform.position, Quaternion.identity);
                }
                caught_poke.localScale = Vector3.zero;
            }
            Object.Destroy(this.gameObject);
        }
        
    }
}
