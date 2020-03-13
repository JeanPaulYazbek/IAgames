using UnityEngine;
using System;
public class dissapear : MonoBehaviour {

    public float segs;//cuantoss segundos esperar antes de desaparecer

    DateTime startTime;

    void Start(){
        startTime = DateTime.Now;
    }

    void Update(){
        DateTime rightNow = DateTime.Now;
        float difference = (float) (rightNow - startTime).TotalSeconds;
        if (segs < difference){
            UnityEngine.Object.Destroy(this.gameObject);
        }
    }

}