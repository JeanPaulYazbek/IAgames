using System;
using UnityEngine;
public class ProyectileFunctions {

    //funcion que dados un punto de inicio (start)
    //un punto final (end)
    //una velocidad de disparo (muzzle_v)
    //la gravedad 
    //una direccion para la velocidad por defecto en caso de que no se pueda llegar a end con muzzle_v
    //un booleano que diga si preferimos un tiro lento pero alto, o rapido corto, true si es lento
    //calcula hacia donde debemos disparar para que el proyectil caiga en end
    public Vector3 CalculateFiringSolution( Vector3 start, Vector3 end, float muzzle_v, Vector3 gravity, Vector3 defaultDirection, bool slow){

        Vector3 delta = end - start;

        float a = Vector3.Dot(gravity,gravity);
        float b = -4 * (Vector3.Dot(gravity,delta) + muzzle_v*muzzle_v);
        float c = 4 * Vector3.Dot(delta, delta);

        if (4*a*c > b*b){//en este caso no hay solucion, osea que el muzzle_v es muy pequenno para lograr llegar
            return defaultDirection;
        }

        //usualmente tendremos dos soluciones una parabola grande
        //y una corta (ojo no se sabe cual es cual)
        float time0 = (float)Math.Sqrt((-b + Math.Sqrt(b*b-4*a*c)) / (2*a));
        float time1 = (float)Math.Sqrt((-b - Math.Sqrt(b*b-4*a*c)) / (2*a));

        bool not_valid0 = (time0 < 0);
        bool not_valid1 = (time1 < 0);

        float ttt = 0f;//valor por defecto que no deberia ser usado

        if(not_valid0 && !not_valid1){//si solo t1 es valido usamos ese tiempo
            ttt = time1;
        }

        if(!not_valid0 && not_valid1){//si solo t0 es valido usamos ese tiempo
            ttt = time0;
        }

        if(not_valid0 && not_valid1){//si ninguno es valido es imposible alcanzar el target
            return defaultDirection;//mandamos una velocidad por defecto
        }

        if(!not_valid0 && !not_valid1){//si ambos son validos devolvemos el que sea mas corto
        //osea el que nos da el proyectil con la curva mas corta y rapida
            if (slow){//si queremos lento
                ttt = Math.Max(time0, time1);
            }else{//si queremos rapido
                ttt = Math.Min(time0, time1);
            }
            
        }

        //calculamos el vector direccion al que debemos lanzar el proyectil
        return (2 * delta - gravity * ttt*ttt) / (2 * muzzle_v * ttt);


    }

}