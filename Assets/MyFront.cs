using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyFront : MonoBehaviour {

    public Transform point;

    public float originDistance;

    public float Angle => NormalizeAngle(transform.localEulerAngles.y);

    public bool isRotatePoint; // false表示绕自身旋转，true表示绕point旋转

    private Vector3 originPosition;
    private float curRelativeDistance;

    private void Start() {
        if (transform.hasChanged) {
            transform.hasChanged = false;
        }
        originDistance = Vector3.Distance(point.position, transform.position);
        originPosition = transform.position;
        curRelativeDistance = 0f;
    }

    public void MoveOnlyWithAngle(float angle) {
        if (!isRotatePoint) {
            transform.localRotation = Quaternion.Euler(0, angle, 0);
            return;
        }
        transform.position = originPosition;
        transform.localRotation = Quaternion.identity;
        Vector3 direction = transform.position - point.position;
        Vector3 newdirection = Quaternion.AngleAxis(angle, Vector3.up) * direction;
        transform.position = point.position + newdirection.normalized * (originDistance + curRelativeDistance);
        LookAt();
    }

    [Button("只移动")]
    public void MoveOnlyWithDeltaDistance(float deltaDistance) {
        curRelativeDistance += deltaDistance;
        Vector3 direction = transform.position - point.position;
        Vector3 newdirection = Quaternion.AngleAxis(0, Vector3.up) * direction;
        transform.position = point.position + newdirection.normalized * (originDistance + curRelativeDistance);
        LookAt();
    }

    [Button("移动和旋转")]
    public void Move(float degree = 0, float moveRelativeToOriginDistance = 0) {
        curRelativeDistance = moveRelativeToOriginDistance;
        transform.position = originPosition;
        transform.localRotation = Quaternion.identity;
        if (degree == 0 && moveRelativeToOriginDistance == 0) {
            return;
        }
        Vector3 direction = transform.position - point.position;
        Vector3 newdirection = Quaternion.AngleAxis(degree, Vector3.up) * direction;
        transform.position = point.position + newdirection.normalized * (originDistance + moveRelativeToOriginDistance);
        LookAt();
    }

    private void LookAt() {
        Vector3 from = transform.right;
        Vector3 to = point.position - transform.position;
        Vector3 normalAxis = transform.up;
        Vector3 modifyTo = to - Vector3.Project(to, normalAxis);
        float angle = Vector3.SignedAngle(from, modifyTo, normalAxis);
        if (Mathf.Abs(angle) > Mathf.Epsilon) {

            transform.localRotation *= Quaternion.AngleAxis(angle, Vector3.up);
        }
    }

    private float NormalizeAngle(float angle) {
        angle = angle % 360;
        if (angle > 180)
            angle -= 360;

        return angle;
    }
}
