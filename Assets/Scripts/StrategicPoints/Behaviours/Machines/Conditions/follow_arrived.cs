using UnityEngine;

//Esta conodicion ayuda a saber si se elcanzo el ultimo punto de follow
public class FollowArrived : Condition {

    FollowPathOfPoints follow;
    Transform character;
    public FollowArrived(FollowPathOfPoints Follow, Transform Character){
        follow = Follow;
        character = Character;
    }

    public override bool Test(){
         
        return (Vector3.Distance(character.position, follow.LastPoint())) < 0.5f;
    }
}