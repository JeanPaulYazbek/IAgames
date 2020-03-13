using System;
using UnityEngine;

public class obstacle_data : MonoBehaviour
{
    //Los vertices del cuadrado, con ellos contruiremos los triangulos
    [SerializeField] 
    public Vector3 upLeft;
    [SerializeField] 
    public Vector3 upRight;
    [SerializeField] 
    public Vector3 downLeft;
    [SerializeField] 
    public Vector3 downRight;

    //String que representa el tipo de un obstaculo, por ejemplo tree1 si son arboles pequeños
    public string type = "Tree1";



    // Funcion que toma el centro de un rectangulo y el largo de sus lados 
    // y devuelve la esquina adecuada. Si quieres esquina inferior izquierda por ejemplo
    // de un cuadrado 2*2 le debes pasar el centro y -2 -2
    public Vector3 corner(Vector3 center, float offsetX, float offsetY){
        return new Vector3(center.x + (offsetX/2f), center.y + (offsetY/2f), 0f);
    }

    void Awake(){

        upRight = corner(transform.position, Math.Abs(transform.localScale.x), Math.Abs(transform.localScale.y));
        upLeft = corner(transform.position, -Math.Abs(transform.localScale.x), Math.Abs(transform.localScale.y));
        downLeft = corner(transform.position, -Math.Abs(transform.localScale.x), -Math.Abs(transform.localScale.y));
        downRight = corner(transform.position, Math.Abs(transform.localScale.x), -Math.Abs(transform.localScale.y));

    }


}
