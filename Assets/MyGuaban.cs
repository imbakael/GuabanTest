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

    public MyLiangan liangan;

    private Transform lianjietou;
    public Transform Lianjietou {
        get {
            if (lianjietou == null) {
                lianjietou = transform.parent;
            }
            return lianjietou;
        }
    }

    public void SetZhijia(MyZhijia leftZhijia, MyZhijia rightZhijia) {
        this.leftZhijia = leftZhijia;
        this.rightZhijia = rightZhijia;
    }

    public Transform GetCorner(CornerDirection direction) {
        return cornerPoints[(int)direction];
    }

    /// <summary>
    /// 根据邻居支架刷新自身位置和旋转
    /// </summary>
    /// <param name="activeNeighbor">自己邻居支架（邻居支架的旋转和移动已计算完毕）</param>
    /// <param name="neighborDirection">邻居是否在自己的左侧</param>
    /// 

    // 能量最低原则：用最小的位移和旋转就可以让corner尽可能重合，优先旋转因为旋转耗能低，其次才是位移
    // 唯一解原则：当长宽的中部槽、哑铃销长度为定值时，推溜某个支架时其他支架的运动只有唯一解

    // todo 动态计算用top corner还是 bottom corner
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
        int fristCount = 0;
        int secondCount = 0;

        float minDistance = float.MaxValue;
        Quaternion targetRotation = Quaternion.identity;
        Quaternion targetLianjietouRotation = Quaternion.identity;
        transform.localRotation = Quaternion.AngleAxis(-MyManager.Instance.maxGuabanAngle, Vector3.up);

        while (NormalizeAngle(transform.localEulerAngles.y) <= MyManager.Instance.maxGuabanAngle) {
            Lianjietou.localRotation = Quaternion.AngleAxis(-MyManager.Instance.maxLianjietouAngle, Vector3.up);
            while (NormalizeAngle(Lianjietou.localEulerAngles.y) <= MyManager.Instance.maxLianjietouAngle) {
                fristCount++;
                float distance = (self.position - neighbor.position).sqrMagnitude;
                if (distance < minDistance) {
                    minDistance = distance;
                    targetRotation = transform.localRotation;
                    targetLianjietouRotation = Lianjietou.localRotation;
                }
                Lianjietou.localRotation *= Quaternion.AngleAxis(1f, Vector3.up);
            }
            transform.localRotation *= Quaternion.AngleAxis(1f, Vector3.up);
        }

        transform.localRotation = targetRotation;
        Lianjietou.localRotation = targetLianjietouRotation;

        float selfAngle = NormalizeAngle(transform.localEulerAngles.y);
        float selfMinAngle = Mathf.Clamp(selfAngle - 1f, -MyManager.Instance.maxGuabanAngle, MyManager.Instance.maxGuabanAngle);
        float selfMaxAngle = Mathf.Clamp(selfAngle + 1f, -MyManager.Instance.maxGuabanAngle, MyManager.Instance.maxGuabanAngle);
        float lianjietouAngle = NormalizeAngle(Lianjietou.localEulerAngles.y);
        float lianjietouMinAngle = Mathf.Clamp(lianjietouAngle - 1f, -MyManager.Instance.maxLianjietouAngle, MyManager.Instance.maxLianjietouAngle);
        float lianjietouMaxAngle = Mathf.Clamp(lianjietouAngle + 1f, -MyManager.Instance.maxLianjietouAngle, MyManager.Instance.maxLianjietouAngle);
        transform.localRotation = Quaternion.AngleAxis(selfMinAngle, Vector3.up);

        while (NormalizeAngle(transform.localEulerAngles.y) <= selfMaxAngle) {
            Lianjietou.localRotation = Quaternion.AngleAxis(lianjietouMinAngle, Vector3.up);
            while (NormalizeAngle(Lianjietou.localEulerAngles.y) <= lianjietouMaxAngle) {
                secondCount++;
                float distance = (self.position - neighbor.position).sqrMagnitude;
                if (distance < minDistance) {
                    minDistance = distance;
                    targetRotation = transform.localRotation;
                    targetLianjietouRotation = Lianjietou.localRotation;
                }
                Lianjietou.localRotation *= Quaternion.AngleAxis(0.1f, Vector3.up);
            }
            transform.localRotation *= Quaternion.AngleAxis(0.1f, Vector3.up);
        }

        transform.localRotation = targetRotation;
        Lianjietou.localRotation = targetLianjietouRotation;

        //Debug.Log("计算后距离 = " + (self.position - neighbor.position).sqrMagnitude);
        Debug.Log($"{GetComponentInParent<MyZhijia>().name} 循环次数：{fristCount} / {secondCount}, 刮板：{NormalizeAngle(transform.localEulerAngles.y)}, 连接头：{NormalizeAngle(Lianjietou.localEulerAngles.y)}");
    }

    private float NormalizeAngle(float angle) {
        angle = angle % 360;
        if (angle > 180)
            angle -= 360;

        return angle;
    }
}
