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
    public MyFront front; // 向前移动local move x
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

        front.transform.localPosition += new Vector3(-0.1f, 0f, 0f);
    }

    [Button("Refresh")]
    /// <summary>
    /// 根据邻居支架刷新自身位置和旋转
    /// </summary>
    /// <param name="activeNeighbor">自己邻居支架（邻居支架的旋转和移动已计算完毕）</param>
    /// <param name="neighborDirection">邻居是否在自己的左侧</param>
    public void Refresh(MyZhijia activeNeighbor, bool isNeighborLeft) {
        Transform neighborTop = isNeighborLeft ? activeNeighbor.guaban.GetCorner(CornerDirection.右上) : activeNeighbor.guaban.GetCorner(CornerDirection.左上);
        Transform neighborBottom = isNeighborLeft ? activeNeighbor.guaban.GetCorner(CornerDirection.右下) : activeNeighbor.guaban.GetCorner(CornerDirection.左下);

        Transform selfTop = isNeighborLeft ? GetCorner(CornerDirection.左上) : GetCorner(CornerDirection.右上);
        Transform selfBottom = isNeighborLeft ? GetCorner(CornerDirection.左下) : GetCorner(CornerDirection.右下);

        float minDistance = float.MaxValue;
        Quaternion targetRotation = Quaternion.identity;
        transform.rotation = Quaternion.AngleAxis(-3, Vector3.up);
        while (NormalizeAngle(transform.localEulerAngles.y) <= 3f) {
            float distance = (selfBottom.position - neighborBottom.position).sqrMagnitude;
            if (distance < minDistance) {
                minDistance = distance;
                targetRotation = transform.rotation;
            }
            transform.rotation *= Quaternion.AngleAxis(0.01f, Vector3.up);
        }
        transform.rotation = targetRotation;
    }

    private void Update() {
        currentAngle = NormalizeAngle(transform.localEulerAngles.y);
    }

    private float NormalizeAngle(float angle) {
        // 确保角度在0-360度范围内
        angle = angle % 360;

        // 如果角度大于180度，转换为负数等效角度
        if (angle > 180)
            angle -= 360;

        return angle;
    }
}
