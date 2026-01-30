using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 连接杆+中部槽
public class MyFront : MonoBehaviour {

    public Transform point;

    public float originDistance;

    public float Angle => NormalizeAngle(transform.localEulerAngles.y);

    public bool isRotatePoint; // false表示绕自身旋转，true表示绕point旋转

    public float tuiyixingcheng;
    private Vector3 originDirection;

    private void Start() {
        if (transform.hasChanged) {
            transform.hasChanged = false;
        }
        originDistance = Vector3.Distance(point.position, transform.position);
        originDirection = transform.position - point.position;
        tuiyixingcheng = 0f;
    }

    [Button("旋转")]
    public void MoveOnlyWithAngle(float angle) {
        Quaternion rotation = Quaternion.Euler(0, angle, 0);
        transform.localRotation = rotation;
        if (!isRotatePoint) {
            return;
        }
        Vector3 nDirection = rotation * originDirection;
        transform.position = point.position + nDirection.normalized * (originDistance + tuiyixingcheng);
    }

    private float NormalizeAngle(float angle) {
        angle = angle % 360;
        if (angle > 180)
            angle -= 360;

        return angle;
    }
}
