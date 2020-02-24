using UnityEngine;
using System;
using System.Collections.Generic;

public class SmoothPath {
    public obstacle_data[] obstacles;
    Utilities utilities;

    public SmoothPath(obstacle_data[] Obstacles){
        obstacles = Obstacles;
        utilities = new Utilities();
    }

    public Vector3[] Smooth(Vector3[] inputPath){

        if (inputPath.Length <= 2){//si el inputPath tiene dos puntos o menos no se puede mejorar
            return inputPath;
        }

        // El primer punto siempre va
        Stack<Vector3> outputPath = new Stack<Vector3>();
        outputPath.Push(inputPath[0]);

        //Comenzamos en 2 porque en teoria los primeros dos puntos no 
        //tienen obstaculos entre ellos 
        int inputIndex = 2;
        int n = inputPath.Length;
        bool rayClear;

        //Mientras no lleguemos al ultimo punto
        while( inputIndex < n - 1){

            rayClear = !(utilities.
            LineSegmentIntersectionObstacle(outputPath.Peek(), inputPath[inputIndex], obstacles));
            //Si hay un obstaculo entre el ultimo punto que guardamos y el siguiente de input
            //es que el punto anterior en el input es necesario
            if(!rayClear){
                outputPath.Push(inputPath[inputIndex - 1]);
            }

            inputIndex++;
        }

        outputPath.Push(inputPath[n - 1]);

        Vector3[] answer = outputPath.ToArray();
        Array.Reverse(answer);
        utilities.DrawPath(answer,40f);
        return answer;
    }
}