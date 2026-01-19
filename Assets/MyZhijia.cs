using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyZhijia : MonoBehaviour {

    public MyGuaban guaban;
    public MyLiangan liangan;
    public bool isActive;

    // test
    public MyZhijia leftZhijia;

    public void TryToModify() {
        guaban.Modify();
    }

    [Button("¸úËæ")]
    public void Follow() {
        guaban.Refresh(leftZhijia, true);
    }

}
