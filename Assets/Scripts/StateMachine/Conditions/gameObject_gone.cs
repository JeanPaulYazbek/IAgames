using System.Collections.Generic;
using UnityEngine;

//Condicion para saber el ultimo objeto de la pila (por ejemplo) una piedra 
//evolutiva fue obtenido
public class GameObjectGone : Condition {
    Stack<GameObject> items;

    public GameObjectGone(Stack<GameObject> Items){
        items = Items;
    }

    public override bool Test(){
        // if((items.Peek() is null)){
        //     Debug.Log("Piedra ya no esta");
        // }else{
        //     Debug.Log(items.Peek().name);

        // }
        return (items.Peek() == null);
    }
}