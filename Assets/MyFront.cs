using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyFront : MonoBehaviour {

    private void Awake() {
        if (transform.hasChanged) {
            transform.hasChanged = false;
        }
    }
    //void Update() {
    //    if (transform.hasChanged) {
    //        transform.hasChanged = false;
    //    }
    //}
}
