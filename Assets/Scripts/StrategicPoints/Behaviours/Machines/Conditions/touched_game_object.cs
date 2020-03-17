using UnityEngine;
using System.Collections.Generic;

// Esta condicion ayudara a saber si estamos demasiado cerca
// de algun gameobject en una lista lada
// Tambien quitara de la lista aquel que se de cuenta que fue destruido
public class TouchedGameObjects : Condition {

    List<GameObject> gameObjects;//los objetos
    Transform character;//personaje que toque los objetos

    public string touchedObj;//nombre del objeto tocado


    public TouchedGameObjects(List<GameObject> GameObjects, Transform Character, string defaultObj){
        gameObjects = GameObjects;
        character = Character;
        touchedObj = defaultObj;
    }

    public override bool Test(){

        //Hacemos una copia del arreglo temporal ya que tendremos que modificar eloriginal
        List<GameObject> gameObjects2 = new List<GameObject>(); 

        foreach(var go in gameObjects){
            gameObjects2.Add(go);
        }

        //revisamos cada objeto
        foreach(var go in gameObjects2){

            // si alguno fue destruido lo sacamos de la lista
            if( go == null){
                gameObjects.Remove(go);
                continue;
            }

            // Si estamos demasiado cerca es que lo tocamos
            if( Vector3.Distance(go.transform.position, character.position) < 2 ){
                touchedObj = string.Copy(go.name);//marcamos al que tocamos
                Object.Destroy(go);//destruimos lo que tocamos
                return true;
            }

            // Si la lista se queda vacia haremos de cuenta que tocamos algo
            if(gameObjects.Count == 0){
                return true;
            }


        }

        return false;
    }

    

}