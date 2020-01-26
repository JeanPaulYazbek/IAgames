using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class static_shoot : MonoBehaviour
{

    public GameObject staticBallPrefab;

    KineticsShoot kineticsBall;//necesitamos una estructura que almacena nuestra informacion de ball
    int[] destroy;//necesitamos saber si debemos ser destruidos

    public Kinetics[] pokemons;//necesiitamos una lista de pokemons para saber si los podemos atrapar

    public Transform[] obstacles;//necesitamos los obstaculos para cuando choquemos con ellos 

    const float gravity = 9.8f;
    // Start is called before the first frame update
    void Start()
    {
        kineticsBall = new KineticsShoot(8f, new Vector3(2f,0f,-1f), new Vector3(0f,0f,gravity), transform, pokemons, obstacles);   
    }

    // Update is called once per frame
    void Update()
    {
        destroy = kineticsBall.UpdateKinetics(Time.deltaTime);
        if(destroy[0]==1){
            Object.Destroy(this.gameObject); //desaparecemos completamente la pokeball
        }
        if(destroy[0]==2){
            Transform caught_poke = pokemons[destroy[1]].transform;
            if(caught_poke.localScale.x > 0){//si no fue atrapado ya
                Instantiate(staticBallPrefab, transform.position, Quaternion.identity);
                pokemons[destroy[1]].transform.localScale = Vector3.zero;
            }
            Object.Destroy(this.gameObject);
        }
        
    }
}
