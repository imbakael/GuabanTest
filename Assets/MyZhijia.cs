using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyZhijia : MonoBehaviour {

    public MyGuaban guaban;
    public bool isActive;
    public MyFront front;

    public MyZhijia leftZhijia;
    public MyZhijia rightZhijia;

    // test
    public MyZhijia activeZhijia;
    public bool useTop;

    public void SetZhijia(MyZhijia leftZhijia, MyZhijia rightZhijia) {
        this.leftZhijia = leftZhijia;
        this.rightZhijia = rightZhijia;
    }

    [Button("¸úËæ")]
    public void Follow() {
        //if (!activeZhijia.front.transform.hasChanged) {
        //    return;
        //}
        guaban.Refresh(activeZhijia, useTop);
    }
}
