using UnityEngine;
using System;

// este componente servira para iniciar la informacion de los meshes
public class static_mesh : MonoBehaviour
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



    // Funcion que toma el centro de un rectangulo y el largo de sus lados 
    // y devuelve la esquina adecuada. Si quieres esquina inferior izquierda por ejemplo
    // de un cuadrado 2*2 le debes pasar el centro y -2 -2
    public Vector3 corner(Vector3 center, float offsetX, float offsetY){
        return new Vector3(center.x + (offsetX/2f), center.y + (offsetY/2f), 0f);
    }

    void DrawSquare(float duration){
        // Dibujamos todos los lados y la linea del medio que forma los triangulos
        Debug.DrawLine(upLeft, downRight, Color.black, duration, true);
        Debug.DrawLine(upLeft, upRight, Color.black, duration, true);
        Debug.DrawLine(upLeft, downLeft, Color.black, duration, true);
        Debug.DrawLine(upRight, downRight, Color.black, duration, true);
        Debug.DrawLine(downLeft, downRight, Color.black, duration, true);
    }

    
}
