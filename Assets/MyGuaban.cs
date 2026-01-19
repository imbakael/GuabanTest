using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGuaban : MonoBehaviour {

    public Transform[] cornerPoints;

    public Transform[] leftCornerPoints;
    public Transform[] rightCornerPoints;

    public Transform center;
    public Transform bottomLink;

    public float angle;
    public float limitAngle = 3f;

    public MyLiangan liangan;
    public MyFront front; // 向前移动local move x
    public float yalingshaoLength; // 哑铃销长度

    public void SetCorners(Transform[] leftCorners, Transform[] rightCorners) {
        this.leftCornerPoints = leftCorners;
        this.rightCornerPoints = rightCorners;
    }

    public void Modify() {
        // 自身绕y轴旋转
        transform.localRotation *= Quaternion.AngleAxis(1f, Vector3.up);

        front.transform.localPosition += new Vector3(-0.1f, 0f, 0f);
    }
    
}
