using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyManager : MonoBehaviour {

    // 每个支架分为底座、前部整体（推移连杆作为主动件+中部槽被动件）

    /* 
     * 中部槽之间的规律：
     * 1.必须满足左右分别有一个corner与左右邻居中部槽的一个corner【尽可能】的重合
     * 2.运动方式只有两种：中部槽绕自身底部中心旋转、推移连杆向正前方移动(忽略推移连杆的左右摇摆)
     * 3.中部槽旋转角度不超过3°
     * 4.只有一个主动件，剩余件的中部槽中心特征点低于主动件
     * 5.满足形变的传导，即主动件运动后，最靠近主动件的中部槽A进行运动+旋转，完事后下一个B槽在基于A的位置进行运动+旋转，直至某个槽N不需要旋转
    */
    public MyZhijia[] zhijias;

    public MyZhijia testZhijia;

    private void Start() {

        for (int i = 0; i < zhijias.Length; i++) {
            MyZhijia curZhijia = zhijias[i];
            MyZhijia leftZhijia = i - 1 >= 0 ? zhijias[i - 1] : null;
            MyZhijia rightZhijia = i + 1 < zhijias.Length ? zhijias[i + 1] : null;
            curZhijia.guaban.SetCorners(leftZhijia?.guaban.cornerPoints, rightZhijia?.guaban.cornerPoints);

        }
    }

    private void Update() {

        if (Input.GetKeyDown(KeyCode.Space)) {
            // 处理邻居刮板和推移连杆
            Debug.Log("处理邻居刮板和推移连杆");
            testZhijia.TryToModify();
        }


        // 存在异常角点时
        while (ExistAbnormalCornerPoints()) {
            // 1.尝试旋转中部槽自身（保证中部槽角度在0~3°内，且每个中部槽左侧一角点与右侧一角点在同一个位置）
            // 2.若中部槽旋转仍有异常点，则推移连杆向前移动Δd距离(需保证已经到最大行程的推移连杆不动，S弯区段的推移连杆才能动)


        }
        //int index = 0;
        //while (index < zhijias.Length) {
        //    if (!zhijias[index].isActive) {
        //        zhijias[index].TryToModify();
        //    }
        //    index++;
        //}

    }

    // 判断刮板是否存在异常角点
    private bool ExistAbnormalCornerPoints() {
        return false;
    }
}