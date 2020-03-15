using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//ananlogo a static data pero esta hecha para datos de cosas que sean lanzadas(por ejemplo poke balls)
public class static_ink_shoot : MonoBehaviour
{

    //DATOS PARA LA POKE BALL
    public GameObject inkSplashPrefab;//imagen de pokeball para dejar ahi cuando atrapemos un poke

    KineticsShoot kineticsBall;//necesitamos una estructura que almacena nuestra informacion de ball
    const float gravity = 9.8f;
    public Vector3 direction;//este vector sera normal en la direccion en la que lanzamos la bola
    public float speed;//magnitud de velocidad de los disparos
    int[] destroy;//necesitamos saber si debemos ser destruidos


    //EXTERNOS
    public Kinetics[] trainers;//necesiitamos una lista de trainers para saber si loss golpeamos
    public GameObject inkScreen;//imagenes de tinta en la camara cuando el trainer sea golpeado
    //public GameObject[] trainersObjs;//estos sera utiles cuando queramos deshabilitar los t
    public Transform[] obstacles;//necesitamos los obstaculos para cuando choquemos con ellos 

    
    // Start is called before the first frame update
    void Start()
    {
        kineticsBall = new KineticsShoot(speed, new Vector3(direction.x,direction.y,direction.z), new Vector3(0f,0f,gravity), transform, trainers, obstacles);   
    }

    // Update is called once per frame
    void Update()
    {
        destroy = kineticsBall.UpdateKinetics(Time.deltaTime);
        int option = destroy[0];
        int trainerIndex = destroy[1];
        if(option==1){//si pisamos el suelo
            
            if(destroy[1]==-1){//si chocamos con el suelo dejamos tinta
                Vector3 ballPos = transform.position;//posicion donde ira la salpicadura
                ballPos.z = 0.1f;//esto es para que los personajes pasen por encima de la tinta
                Instantiate(inkSplashPrefab, ballPos, Quaternion.identity); //dejamos una salpicadura de tinta
            }
            
            Object.Destroy(this.gameObject); //desaparecemos completamente la pokeball
        }
        if(option==2){//si le pegamos a un trainer
            
            if(trainerIndex == 0){//si golpeamos al jugador
                //ponemos tinta en la pantalla 30 segs
                inkScreen.SetActive(true);
                
            }

            Object.Destroy(this.gameObject);
        }
        
    }




}
