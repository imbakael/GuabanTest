using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTest : MonoBehaviour {

    public Transform zhijiaParent;
    public Transform[] zs;
    public Transform[] zx;
    public Transform[] ys;
    public Transform[] yx;
    public GuaBanShuSongJi[] allGuaban;

    public Transform test;
    public int buju;
    private void Start() {
        //zs = new Transform[zhijiaCount];
        //zx = new Transform[zhijiaCount];
        //ys = new Transform[zhijiaCount];
        //yx = new Transform[zhijiaCount];
        //for (int i = 0; i < zhijiaCount; i++) {
        //    GuaBanShuSongJi gb = GameObject.Find("支架" + (i + 1)).GetComponentInChildren<GuaBanShuSongJi>();
        //    zs[i] = gb.GetCorner(CornerDirection.左上);
        //    zx[i] = gb.GetCorner(CornerDirection.左下);

        //    ys[i] = gb.GetCorner(CornerDirection.右上);
        //    yx[i] = gb.GetCorner(CornerDirection.右下);
        //}
        allGuaban = zhijiaParent.GetComponentsInChildren<GuaBanShuSongJi>();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            buju++;
            QianDuan();
        }
        if (Input.GetKeyDown(KeyCode.J)) {
            
        }
    }

    public void QianDuan() {
        allGuaban[0].SetChildByDirection(CornerDirection.右下);
        allGuaban[0].tempParent.Translate(-Vector3.right * 0.6f / 9);

        //allGuaban[1].SetChildByDirection(CornerDirection.右下);
        //allGuaban[1].tempParent.position = allGuaban[0].GetCorner(CornerDirection.左下).position;
        //allGuaban[1].tempParent.RotateAround(allGuaban[1].tempParent.position, Vector3.up, 1);

        //allGuaban[5].SetChildByDirection(CornerDirection.右上);
        //allGuaban[5].tempParent.position = allGuaban[4].GetCorner(CornerDirection.左上).position;
        //allGuaban[5].tempParent.RotateAround(allGuaban[5].tempParent.position, Vector3.up, -1);

        for (int i = 1; i <= 4; i++) {
            allGuaban[i].SetChildByDirection(CornerDirection.右下);
            allGuaban[i].tempParent.position = allGuaban[i - 1].GetCorner(CornerDirection.左下).position;
            allGuaban[i].tempParent.RotateAround(allGuaban[i].tempParent.position, Vector3.up, i == buju ? 1 : 0);
            //yx[i].position = zx[i - 1].position;
            //yx[i].RotateAround(yx[i].position, Vector3.up, 1);
        }
        for (int i = 5; i < 9; i++) {
            allGuaban[i].SetChildByDirection(CornerDirection.右上);
            allGuaban[i].tempParent.position = allGuaban[i - 1].GetCorner(CornerDirection.左上).position;
            allGuaban[i].tempParent.RotateAround(allGuaban[i].tempParent.position, Vector3.up, i == buju ? -1 : 0);
            //ys[i].position = zs[i - 1].position;
            //ys[i].RotateAround(ys[i].position, Vector3.up, -1);
        }
    }
}
