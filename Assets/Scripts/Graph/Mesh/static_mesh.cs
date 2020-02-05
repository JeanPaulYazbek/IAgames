using UnityEngine;
using System;

// este componente servira para iniciar la informacion de los meshes
public class static_mesh : MonoBehaviour
{

    //Los vertices del cuadrado, con ellos contruiremos los triangulos
    [SerializeField] 
    Vector3 upLeft;
    [SerializeField] 
    Vector3 upRight;
    [SerializeField] 
    Vector3 downLeft;
    [SerializeField] 
    Vector3 downRight;

    // Funcion que toma el centro de un rectangulo y el largo de sus lados 
    // y devuelve la esquina adecuada. Si quieres esquina inferior izquierda por ejemplo
    // de un cuadrado 2*2 le debes pasar el centro y -2 -2
    Vector3 corner(Vector3 center, float offsetX, float offsetY){
        return new Vector3(center.x + (offsetX/2f), center.y + (offsetY/2f), 0f);
    }

    // Start is called before the first frame update
    void Start()
    {

        //Buscamos los datos necesarios para los calculos
        Vector3 center = transform.position;
        float offsetX = Math.Abs(transform.localScale.x);
        float offsetY = Math.Abs(transform.localScale.y);
        Debug.Log(center);

        // Conseguimos las esquinas
        upLeft = corner(center, -offsetX, offsetY);
        upRight = corner(center, offsetX, offsetY);
        downLeft = corner(center, -offsetX, -offsetY);
        downRight = corner(center, offsetX, -offsetY);

        
        // Dibujamos los triangulos
        Debug.DrawLine(upLeft, downRight, Color.black, 20, true);
        Debug.DrawLine(upLeft, upRight, Color.black, 20, true);
        Debug.DrawLine(upLeft, downLeft, Color.black, 20, true);
        Debug.DrawLine(upRight, downRight, Color.black, 20, true);
        Debug.DrawLine(downLeft, downRight, Color.black, 20, true);


        
    }

    // Update is called once per frame
    // void Update()
    // {
        
    // }
}
