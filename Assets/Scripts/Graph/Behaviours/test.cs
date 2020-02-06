using System.Collections.Generic;
using UnityEngine;
using System;

public class test : MonoBehaviour
{

    GameObject[] meshes;

    Utilities utilities;
    List<Node> nodes = new List<Node>();

    // Start is called before the first frame update
    void Start()
    {
        meshes = GameObject.FindGameObjectsWithTag("MyMesh");
        static_mesh target;
        int id = 0;
        for(int i = 0; i< meshes.Length; i++){
            target = meshes[i].GetComponent<static_mesh>();
            // Generamos los dos triangulos a partir de un rectangulo
            nodes.Add(new Node(id, target.upLeft, target.downLeft, target.downRight));
            id++;
            nodes.Add(new Node(id, target.upLeft, target.downRight, target.upRight));
            id++;

        }

        //Ahora vamos a hacer una matriz que representara las coordenadas X y Y
        //para ellos necesitaremos saber cuanto offset tienen los numeros
        //para solo trabajar momentaneamente con coordenadas positivas

        float min = float.PositiveInfinity;//si calculamos la coordenada mas pequenna podremos saber cuantos es el offser
        float max = float.NegativeInfinity;//si calculamos la coordenada mas grande junto al offser sabremos el largo de la matriz
        Vector3 vertexB;
        
        foreach(var node in nodes){
            node.DrawTriangle(40);

            //solo necesitamos un vertice para hacer las comparaciones
            //ya que sabemos que nuestros rectangulos de mayor largo
            //tienen maximo 15 en un largo entonces al final basta con
            //rodar el minimo que consigamos digamos 30 unidades
            vertexB = node.vertexB;

            if(min >  vertexB.x){
                min =  vertexB.x;
            }

            if(min >  vertexB.y){
                min =  vertexB.y;
            }

            if(max <  vertexB.x){
                max =  vertexB.x;
            }

            if(max <  vertexB.y){
                max =  vertexB.y;
            }

        }

        int offset = (int)Math.Abs(min);

        //solo para ser precavidos agreguemosle 30 unidades
        offset += 30;

        int n = offset + (int)max;
        List<Node>[,] plane = new List<Node>[n,n];

        // for(int i = 0; i<200; i++){
        //     for(int j = 0; j<200; j++){
        //         max++;
        //         Debug.Log(j);
        //     }
        // }

        Debug.Log(offset);
        Debug.Log(n);
        
    }


}
