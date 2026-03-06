using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuaBanShuSongJi : MonoBehaviour {

    public Transform[] corners;

    public Transform zhongbucao;
    public Transform tempParent;

    public Transform GetCorner(CornerDirection direction) {
        return corners[(int)direction];
    }

    public void SetChildByDirection(CornerDirection direction) {
        if (zhongbucao.parent == tempParent) {
            tempParent.DetachChildren();
        }
        Transform target = GetCorner(direction);
        tempParent.position = target.position;
        zhongbucao.parent = tempParent;
    }

    public int random;
    public void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            SetChildByDirection((CornerDirection)random);
        }
    }
}
