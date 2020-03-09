using System.Collections.Generic;
using UnityEngine;

public class pokemon_data : MonoBehaviour
{
    SpriteRenderer mySprite;
    public Sprite runSprite;
    public Sprite flySprite;
    public Sprite defaultSprite;

    public int value = 1;
    public bool evolved = false;//esto se volvera true si evolucionamos dentro del juego
    public Sprite[] evolution_sprites;//arreglo con los sprites de nuestras evoluciones 
    public string[] evolution_methods;//areglo con el nombre del metodo de evolucion

    Dictionary<string, Sprite> howToEvolve;




    void Awake()
    {
        mySprite = this.GetComponent<SpriteRenderer>();
        howToEvolve =  new Dictionary<string, Sprite>();
        for(int i = 0; i < evolution_methods.Length; i++){
            howToEvolve.Add(evolution_methods[i], evolution_sprites[i]);
        }

    }

    public void Evolve(string evolution_method){
        Sprite evolSprite = howToEvolve[evolution_method];

        if(!(evolSprite is null)){
            mySprite.sprite = evolSprite;
        }

        value++;//si evolucionas tu valor aumenta en 1
    }

    //Cuando un pokemon va a correr usa esto para cambiar su sprite a uno 
    //de correr
    public void Run(){
        mySprite.sprite = runSprite;
    }

    //Cuando un pokemon tenga un sprite de vuelo
    public void Fly(){
        mySprite.sprite = flySprite;
    }

    public void DefaultSprite(){
        mySprite.sprite = defaultSprite;
    }



    
}
