using UnityEngine;
public class Collision {

    public Vector3 position;
    public Vector3 normal;

    public bool collision;//booleano que nos ayudara a saber si de verdad hubo una colision

    public Collision(Vector3 Position, Vector3 Normal, bool Colli){
        position = Position;
        normal = Normal;
        collision = Colli;
    }

}