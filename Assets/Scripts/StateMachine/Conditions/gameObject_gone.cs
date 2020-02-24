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
        return (items.Peek() == null);
    }
}