using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CornerDirection {
    左上,
    右上,
    右下,
    左下
}

public class MyGuaban : MonoBehaviour {

    public Transform[] cornerPoints;

    public MyZhijia leftZhijia;
    public MyZhijia rightZhijia;

    public Transform center;
    public Transform bottomLink;

    public float currentAngle;
    public float limitAngle = 3f;

    public MyLiangan liangan;
    public float yalingshaoLength; // 哑铃销长度

    public void SetZhijia(MyZhijia leftZhijia, MyZhijia rightZhijia) {
        this.leftZhijia = leftZhijia;
        this.rightZhijia = rightZhijia;
    }

    public Transform GetCorner(CornerDirection direction) {
        return cornerPoints[(int)direction];
    }

    public void Modify() {
        // 自身绕y轴旋转
        transform.localRotation *= Quaternion.AngleAxis(1f, Vector3.up);

    }

    /// <summary>
    /// 根据邻居支架刷新自身位置和旋转
    /// </summary>
    /// <param name="activeNeighbor">自己邻居支架（邻居支架的旋转和移动已计算完毕）</param>
    /// <param name="neighborDirection">邻居是否在自己的左侧</param>
    /// 

    // 能量最低原则：用最小的位移和旋转就可以让corner尽可能重合，优先旋转因为旋转耗能低，其次才是位移
    // 唯一解原则：当长宽的中部槽、哑铃销长度为定值时，推溜某个支架时其他支架的运动只有唯一解
    public void Refresh(MyZhijia activeNeighbor, bool isNeighborLeft, bool useTop) {
        if (Mathf.Abs(activeNeighbor.front.transform.position.x - transform.parent.position.x) < Mathf.Epsilon) {
            Debug.Log("支架推移行程几乎相同");
            return;
        }
        Transform neighborTop = isNeighborLeft ? activeNeighbor.guaban.GetCorner(CornerDirection.右上) : activeNeighbor.guaban.GetCorner(CornerDirection.左上);
        Transform neighborBottom = isNeighborLeft ? activeNeighbor.guaban.GetCorner(CornerDirection.右下) : activeNeighbor.guaban.GetCorner(CornerDirection.左下);

        Transform selfTop = isNeighborLeft ? GetCorner(CornerDirection.左上) : GetCorner(CornerDirection.右上);
        Transform selfBottom = isNeighborLeft ? GetCorner(CornerDirection.左下) : GetCorner(CornerDirection.右下);

        Transform neighbor = useTop ? neighborTop : neighborBottom;
        Transform self = useTop ? selfTop : selfBottom;

        //Debug.Log("初始距离 = " + (self.position - neighbor.position).sqrMagnitude);

        float minDistance = float.MaxValue;
        Quaternion targetRotation = Quaternion.identity;
        transform.localRotation = Quaternion.AngleAxis(-MyManager.Instance.maxAngle, Vector3.up);
        while (NormalizeAngle(transform.localEulerAngles.y) <= MyManager.Instance.maxAngle) {
            float distance = (self.position - neighbor.position).sqrMagnitude;
            // distance <= MyManager.Instance.SqrYalingxiaoLength
            if (distance < minDistance) {
                minDistance = distance;
                targetRotation = transform.localRotation;
            }
            transform.localRotation *= Quaternion.AngleAxis(0.01f, Vector3.up);
        }
        transform.localRotation = targetRotation;
        //Debug.Log("计算后距离 = " + (self.position - neighbor.position).sqrMagnitude);
    }

    private void Update() {
        currentAngle = NormalizeAngle(transform.localEulerAngles.y);
    }

    private float NormalizeAngle(float angle) {
        angle = angle % 360;
        if (angle > 180)
            angle -= 360;

        return angle;
    }
}
