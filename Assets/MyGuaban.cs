using System;
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

    private MyZhijia selfZhijia;
    public MyZhijia SelfZhijia {
        get {
            if (selfZhijia == null) {
                selfZhijia = GetComponentInParent<MyZhijia>();
            }
            return selfZhijia;
        }
    }

    private Transform selfFront;
    public Transform SelfFront {
        get {
            if (selfFront == null) {
                selfFront = SelfZhijia.front.transform;
            }
            return selfFront;
        }
    }

    public List<Transform> gizmoTrans = new List<Transform>();

    public Transform GetCorner(CornerDirection direction) {
        return cornerPoints[(int)direction];
    }


    void OnDrawGizmos() {
        // 在物体位置绘制图标
        if (gizmoTrans.Count == 2) {
            Gizmos.DrawIcon(gizmoTrans[0].position, "self.png", false);
            Gizmos.DrawIcon(gizmoTrans[1].position, "neighbor.png", false);
        }
        
    }

    /// <summary>
    /// 根据邻居支架刷新自身位置和旋转
    /// </summary>
    /// <param name="activeNeighbor">自己邻居支架（邻居支架的旋转和移动已计算完毕）</param>
    /// 

    // 能量最低原则：用最小的位移和旋转就可以让corner尽可能重合，优先旋转因为旋转耗能低，其次才是位移
    // 唯一解原则：当长宽的中部槽、哑铃销长度为定值时，推溜某个支架时其他支架的运动只有唯一解

    // todo 动态计算用top corner还是 bottom corner
    public void Refresh(MyZhijia activeNeighbor, bool useTop) {
        //if (Mathf.Abs(activeNeighbor.front.transform.localPosition.x - SelfZhijia.front.transform.localPosition.x) < Mathf.Epsilon) {
        //    Debug.Log("支架推移行程几乎相同");
        //    return;
        //}
        bool isNeighborLeft = SelfZhijia.leftZhijia == activeNeighbor;
        Transform neighborTop = isNeighborLeft ? activeNeighbor.guaban.GetCorner(CornerDirection.右上) : activeNeighbor.guaban.GetCorner(CornerDirection.左上);
        Transform neighborBottom = isNeighborLeft ? activeNeighbor.guaban.GetCorner(CornerDirection.右下) : activeNeighbor.guaban.GetCorner(CornerDirection.左下);

        Transform selfTop = isNeighborLeft ? GetCorner(CornerDirection.左上) : GetCorner(CornerDirection.右上);
        Transform selfBottom = isNeighborLeft ? GetCorner(CornerDirection.左下) : GetCorner(CornerDirection.右下);

        Transform neighbor = useTop ? neighborTop : neighborBottom;
        Transform self = useTop ? selfTop : selfBottom;
        if (!gizmoTrans.Contains(self)) {
            gizmoTrans.Add(self);
        }

        if (!gizmoTrans.Contains(neighbor)) {
            gizmoTrans.Add(neighbor);
        }

        int loopCount = 0;
        float minDistance = float.MaxValue;
        HandleRotate(neighborTop, neighborBottom, neighbor, self, ref loopCount, ref minDistance);

        //Vector3 originPos = SelfFront.transform.localPosition;
        //float t = 0f;
        //while ((self.position - neighbor.position).sqrMagnitude > MyManager.Instance.SqrYalingxiaoLength) {
        //    t += 0.02f;
        //    SelfFront.transform.localPosition = Vector3.Lerp(originPos, activeNeighbor.front.transform.localPosition, t);
        //    HandleRotate(neighborTop, neighborBottom, neighbor, self, ref loopCount, ref minDistance);
        //    if (t >= 1f) {
        //        break;
        //    }
        //}
        Debug.Log($" {SelfZhijia.name} loopCount : {loopCount}");

    }

    private void HandleRotate(Transform neighborTop, Transform neighborBottom, Transform neighbor, Transform self, ref int loopCount, ref float minDistance) {
        float maxGuabanAngle = MyManager.Instance.maxGuabanAngle;
        float maxLianjietouAngle = MyManager.Instance.maxLianjietouAngle;
        float maxTuiganAngle = MyManager.Instance.maxTuiganAngle;

        Quaternion targetRotation = Quaternion.identity;
        Quaternion targetLianjietouRotation = Quaternion.identity;
        Quaternion targetTuiganRotation = Quaternion.identity;

        CalcRotate(neighborTop, neighborBottom, neighbor, self,
            -maxGuabanAngle, maxGuabanAngle, -maxLianjietouAngle, maxLianjietouAngle, -maxTuiganAngle, maxTuiganAngle, 1f,
            ref loopCount, ref minDistance, ref targetRotation, ref targetLianjietouRotation, ref targetTuiganRotation);

        float delta = 0.5f;

        float currentGuabanAngle = NormalizeAngle(targetRotation.eulerAngles.y);
        float selfMinAngle = Mathf.Clamp(currentGuabanAngle - delta, -maxGuabanAngle, maxGuabanAngle);
        float selfMaxAngle = Mathf.Clamp(currentGuabanAngle + delta, -maxGuabanAngle, maxGuabanAngle);

        float currentLianjietouAngle = NormalizeAngle(targetLianjietouRotation.eulerAngles.y);
        float lianjietouMinAngle = Mathf.Clamp(currentLianjietouAngle - delta, -maxLianjietouAngle, maxLianjietouAngle);
        float lianjietouMaxAngle = Mathf.Clamp(currentLianjietouAngle + delta, -maxLianjietouAngle, maxLianjietouAngle);

        float currentTuiganAngle = NormalizeAngle(targetTuiganRotation.eulerAngles.y);
        float tuiganMinAngle = Mathf.Clamp(currentTuiganAngle - delta, -maxTuiganAngle, maxTuiganAngle);
        float tuiganMaxAngle = Mathf.Clamp(currentTuiganAngle + delta, -maxTuiganAngle, maxTuiganAngle);

        CalcRotate(neighborTop, neighborBottom, neighbor, self,
            selfMinAngle, selfMaxAngle, lianjietouMinAngle, lianjietouMaxAngle, tuiganMinAngle, tuiganMaxAngle, 0.1f,
            ref loopCount, ref minDistance, ref targetRotation, ref targetLianjietouRotation, ref targetTuiganRotation);
        //Debug.Log($"2 {SelfZhijia.name} currentGuabanAngle : {currentGuabanAngle}, min : {selfMinAngle}, max : {selfMaxAngle}, currentLianjietouAngle : {currentLianjietouAngle}, min : {lianjietouMinAngle}, max : {lianjietouMaxAngle}");

        transform.localRotation = Quaternion.Euler(0, Mathf.Clamp(NormalizeAngle(targetRotation.eulerAngles.y), -maxGuabanAngle, maxGuabanAngle), 0);
        Lianjietou.localRotation = Quaternion.Euler(0, Mathf.Clamp(NormalizeAngle(targetLianjietouRotation.eulerAngles.y), -maxLianjietouAngle, maxLianjietouAngle), 0);
        SelfFront.localRotation = Quaternion.Euler(0, Mathf.Clamp(NormalizeAngle(targetTuiganRotation.eulerAngles.y), -maxTuiganAngle, maxTuiganAngle), 0);

        //Debug.Log($"{SelfZhijia.name} 循环次数：{loopCount}, " +
        //    $"刮板：{NormalizeAngle(transform.localEulerAngles.y)}, 连接头：{NormalizeAngle(Lianjietou.localEulerAngles.y)}, 推杆：{NormalizeAngle(targetTuiganRotation.eulerAngles.y)}");
    }

    /// <summary>
    /// 基于刮板旋转角和连接头旋转角的范围，使两个刮板连接点处于最小距离，且两个刮板不能有重叠部分
    /// </summary>

    private void CalcRotate(Transform neighborTop, Transform neighborBottom, Transform neighbor, Transform self,
        float guabanMinAngle, float guabanMaxAngle, float lianjietouMinAngle, float lianjietouMaxAngle, float tuiganMinAngle, float tuiganMaxAngle, float angleDelta,
        ref int loopCount, ref float minDistance, ref Quaternion targetRotation, ref Quaternion targetLianjietouRotation, ref Quaternion targetTuiganRotation, float epsilon = 0.0001f) {

        int i = 0;
        SelfFront.localRotation = Quaternion.Euler(0, (tuiganMinAngle + tuiganMaxAngle) / 2, 0);
        while (NormalizeAngle(SelfFront.localEulerAngles.y) <= tuiganMaxAngle + epsilon) {
            int j = 0;
            Lianjietou.localRotation = Quaternion.Euler(0, (lianjietouMinAngle + lianjietouMaxAngle) / 2, 0);
            while (NormalizeAngle(Lianjietou.localEulerAngles.y) <= lianjietouMaxAngle + epsilon) {
                int k = 0;
                transform.localRotation = Quaternion.Euler(0, (guabanMinAngle + guabanMaxAngle) / 2, 0);
                while (NormalizeAngle(transform.localEulerAngles.y) <= guabanMaxAngle + epsilon) {
                    loopCount++;
                    float distance = (self.position - neighbor.position).sqrMagnitude;
                    if (distance < minDistance && !JL.IsSegmentIntersectingRectangle(neighborTop, neighborBottom, GetCorner(CornerDirection.左上), GetCorner(CornerDirection.右上), GetCorner(CornerDirection.右下), GetCorner(CornerDirection.左下))) {
                        minDistance = distance;
                        targetRotation = transform.localRotation;
                        targetLianjietouRotation = Lianjietou.localRotation;
                        targetTuiganRotation = SelfFront.localRotation;
                        if (minDistance < 0.0001f) {
                            return;
                        }
                    }
                    k++;
                    transform.localRotation = Quaternion.Euler(0, angleDelta * GetOffset(k), 0);
                }
                j++;
                Lianjietou.localRotation = Quaternion.Euler(0, angleDelta * GetOffset(j), 0);
            }
            i++;
            SelfFront.localRotation = Quaternion.Euler(0, angleDelta * GetOffset(i), 0);
        }
    }

    private int GetOffset(int index) {
        return index % 2 == 0 ? (-index / 2) : ((index + 1) / 2);
    }

    private float NormalizeAngle(float angle) {
        angle = angle % 360;
        if (angle > 180)
            angle -= 360;

        return angle;
    }


}
