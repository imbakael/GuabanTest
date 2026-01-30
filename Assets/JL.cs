using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


public class JL : MonoBehaviour {

    public Transform[] corners;

    public Transform wholePoint;
    public Transform headPoint;
    public Transform middlePoint;

    private void Update() {
        //if (Input.GetKeyDown(KeyCode.Space)) {
        //    for (int i = 0; i < corners.Length; i++) {
        //        Vector3 one = corners[i].position;
        //        Vector3 other = corners[(i + 1) % corners.Length].position;
        //        Debug.Log($"i {i}, distance : {Vector3.Distance(one, other)}, " +
        //            $" xzD : {Vector2.Distance(new Vector2(one.x, one.z), new Vector2(other.x, other.z))}, yD : {other.y - one.y}");
        //    }
        //}
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        for (int i = 0; i < corners.Length; i++) {
            Vector3 one = corners[i].position;
            Vector3 other = corners[(i + 1) % corners.Length].position;
            Gizmos.DrawLine(one, other);
        }
        Gizmos.color = Color.red;
        Gizmos.DrawLine(middlePoint.position, headPoint.position);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(headPoint.position, wholePoint.position);
    }

    /// <summary>
    /// 判断线段是否与矩形相交(线段在矩形内部也视为相交)
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="leftTop"></param>
    /// <param name="rightTop"></param>
    /// <param name="rightBottom"></param>
    /// <param name="leftBottom"></param>
    /// <returns></returns>
    public static bool IsSegmentIntersectingRectangle(Transform a, Transform b, Transform leftTop, Transform rightTop, Transform rightBottom, Transform leftBottom) {
        Vector2[] rectVertices = new Vector2[] { GetVector2(leftTop), GetVector2(rightTop), GetVector2(rightBottom), GetVector2(leftBottom) };
        return IsSegmentIntersectingRectangle(GetVector2(a), GetVector2(b), rectVertices);
    }

    public static Vector2 GetVector2(Transform t) {
        return new Vector2(t.position.x, t.position.z);
    }

    public static bool IsSegmentIntersectingRectangle(Vector2 segStart, Vector2 segEnd, Vector2[] rectVertices) {
        // 1. 快速检查：线段端点是否在矩形内
        if (IsPointInRectangle(segStart, rectVertices) || IsPointInRectangle(segEnd, rectVertices)) {
            return true;
        }

        // 2. 检查线段是否与矩形的四条边相交
        for (int i = 0; i < 4; i++) {
            Vector2 edgeStart = rectVertices[i];
            Vector2 edgeEnd = rectVertices[(i + 1) % 4]; // 取下一个点，循环到第一个点

            if (AreSegmentsIntersecting(segStart, segEnd, edgeStart, edgeEnd)) {
                return true;
            }
        }

        return false;
    }

    // 判断点是否在矩形内（利用叉积符号判断点与边的相对位置）
    private static bool IsPointInRectangle(Vector2 point, Vector2[] rectVertices) {
        // 依次检查点是否在矩形每条边的“内侧”（对于逆时针顶点顺序，通常是内侧）
        for (int i = 0; i < 4; i++) {
            Vector2 a = rectVertices[i];
            Vector2 b = rectVertices[(i + 1) % 4];
            Vector2 ap = point - a;
            Vector2 ab = b - a;

            // 计算向量AB与向量AP的二维叉积（z分量）
            float cross = ab.x * ap.y - ab.y * ap.x;
            // 如果点在边的右侧（假设顶点为逆时针顺序），则不在矩形内
            if (cross >= 0) {
                return false;
            }
        }
        return true;
    }

    // 判断两条线段是否相交（快速排斥实验 + 跨立实验）
    private static bool AreSegmentsIntersecting(Vector2 a, Vector2 b, Vector2 c, Vector2 d) {
        // 快速排斥实验：检查线段包围盒是否重叠
        if (Mathf.Max(a.x, b.x) < Mathf.Min(c.x, d.x) || Mathf.Min(a.x, b.x) > Mathf.Max(c.x, d.x) ||
            Mathf.Max(a.y, b.y) < Mathf.Min(c.y, d.y) || Mathf.Min(a.y, b.y) > Mathf.Max(c.y, d.y)) {
            return false;
        }

        Vector2 ab = b - a;
        Vector2 ac = c - a;
        Vector2 ad = d - a;

        Vector2 cd = d - c;
        Vector2 ca = a - c;
        Vector2 cb = b - c;

        // 检查点C、D是否在线段AB两侧，且点A、B是否在线段CD两侧
        bool intersect = (Cross(ab, ac) * Cross(ab, ad) < 0) && (Cross(cd, ca) * Cross(cd, cb) < 0);
        return intersect;
    }
    // 跨立实验：通过叉积符号判断点与线段的位置关系
    private static float Cross(Vector2 v1, Vector2 v2) => v1.x * v2.y - v1.y * v2.x;
}
