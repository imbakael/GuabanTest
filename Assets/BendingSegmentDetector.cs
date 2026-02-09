using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class BendingSegmentDetector {

    private readonly float _maxTravelL; // 推溜最大行程 L
    private readonly int _segmentCountS; // 弯曲段包含支架数 S
    private readonly float _tolerance; // 判定阈值（预定差值）

    public BendingSegmentDetector(float maxTravel, int segmentFrameCount, float slopeTolerance) {
        _maxTravelL = maxTravel;
        _segmentCountS = segmentFrameCount;
        _tolerance = slopeTolerance;
    }

    public List<int> DetectBendingSegments(List<XingchengData> xc) {
        var result = new List<int>();

        float theoreticalSlope = _maxTravelL / _segmentCountS; // 公式: L/S
        for (int startIdx = 0; startIdx <= xc.Count - _segmentCountS; startIdx++) {
            List<XingchengData> child = xc.Skip(startIdx).Take(_segmentCountS).ToList();
            float minValue = float.MaxValue;
            float maxValue = float.MinValue;
            int minNo = int.MaxValue;
            int maxNo = int.MinValue;
            for (int i = 0; i < child.Count; i++) {
                XingchengData xcd = child[i];
                if (xcd.tuiLiuXC == 0) {
                    continue;
                }
                if (xcd.tuiLiuXC < minValue) {
                    minValue = xcd.tuiLiuXC;
                    minNo = xcd.no;
                }
                if (xcd.tuiLiuXC > maxValue) {
                    maxValue = xcd.tuiLiuXC;
                    maxNo = xcd.no;
                }
            }
            int frameDiff = Math.Abs(maxNo - minNo) + 1;
            float actualSlope = (maxValue - minValue) / frameDiff;

            float slopeDifference = Math.Abs(Math.Abs(actualSlope) - theoreticalSlope);

            if (slopeDifference <= _tolerance) {
                result.Add(child[0].no);
                Debug.Log($"{child[0].no}    minNo:{minNo}, xc:{minValue}, maxNo:{maxNo}, xc:{maxValue}, actualSlope:{actualSlope}");
            }
        }

        
        return result;
    }
}
