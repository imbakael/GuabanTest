using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyZhijia : MonoBehaviour {

    public MyGuaban guaban;
    public MyLiangan liangan;
    public bool isActive;
    public MyFront front;

    // test
    public MyZhijia activeZhijia;
    public bool useTop;

    [Button("¸úËæ")]
    public void Follow() {
        //if (!activeZhijia.front.transform.hasChanged) {
        //    return;
        //}
        guaban.Refresh(activeZhijia, false, useTop);
    }
}
