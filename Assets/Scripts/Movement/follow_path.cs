
using UnityEngine;

public class FollowPath : Seek {

    Path path;//clase que tiene la funcion del camino
    float currentParam;//posicion actual en el camino de character
    float pathOffset;//que tan lejos estara el target imaginario
    public FollowPath(Kinetics Character, float MaxAccel, float PathOffset) : base(Character, Character, MaxAccel){

        pathOffset = PathOffset;
        path = new Path();
    }
    
    //funcion que persigue un target ficticio 
    public SteeringOutput getSteeringFP(){

        currentParam = path.GetParam(character.transform.position.x);

        float targetParam = currentParam + pathOffset;

        Vector3 targetPosition = path.GetPosition(targetParam);

        return getSteering2(targetPosition,1);
    }
 }