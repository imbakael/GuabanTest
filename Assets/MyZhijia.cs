using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyZhijia : MonoBehaviour {

    public MyGuaban guaban;
    public MyLiangan liangan;
    public bool isActive;

    public void TryToModify() {
        guaban.Modify();
    }

}
