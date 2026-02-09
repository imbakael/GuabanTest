using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.VersionControl;
using UnityEngine;
using XCharts.Runtime;

public class XchartTest : MonoBehaviour
{
    public LineChart chart;
    public string[] jsonNames;
    public string jsonName;

    public int curOuterIndex = 0;
    public int curInnerIndex = 0;
    private List<List<List<XingchengData>>> modifyData = new List<List<List<XingchengData>>>();
    private int maxNo = 169;

    public float maxTravelL = 900f; // 假设最大行程900mm
    public int segmentFrameCountS = 10; // 假设弯曲段跨10个支架
    public float tolerance = 100f; // 斜率容差阈值，需根据实际情况调整

    private void Start() {
        for (int k = 0; k < jsonNames.Length; k++) {
            string jsonName = jsonNames[k];
            string path = Application.streamingAssetsPath + $"/{jsonName}.json";
            string msg = System.IO.File.ReadAllText(path);
            var allData = JsonConvert.DeserializeObject<List<XingchengData>>(msg);
            modifyData.Add(new List<List<XingchengData>>());
            for (int i = 0; i < allData.Count; i += maxNo) {
                var temp = new List<XingchengData>();
                for (int j = i; j < i + maxNo; j++) {
                    temp.Add(allData[j]);
                }
                //temp = temp.OrderBy(t => t.no).ToList();
                modifyData[k].Add(temp);
            }
            Debug.Log($"k = {k}, modifyCount = {modifyData[k].Count}");
        }
        
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            Handle();
        }
        if (Input.GetKeyDown(KeyCode.J)) {
            int index = Array.IndexOf(jsonNames, jsonName);
            if (index != -1) {
                curInnerIndex = 0;
                curOuterIndex = index;
                Handle();
            }
        }
        if (Input.GetKeyDown(KeyCode.K)) {
            curInnerIndex = modifyData[curOuterIndex].Count - 1 - 9;
            Handle();
        }
        if (Input.GetKeyDown(KeyCode.L)) {
            List<XingchengData> cur = modifyData[0][0];

            chart.RemoveData();
            chart.AddSerie<Line>("数据：" + curInnerIndex.ToString());

            for (int i = 0; i < maxNo; i++) {
                chart.AddXAxisData(cur[i].no.ToString());
                chart.AddData(0, cur[i].tuiLiuXC);
            }

            var detector = new BendingSegmentDetector(maxTravelL, segmentFrameCountS, tolerance);
            List<int> segments = detector.DetectBendingSegments(cur);
            Debug.Log($"segments.count ：{segments.Count}");
            if (segments.Count > 0) {
                foreach (int startNo in segments) {
                    Debug.Log($"弯曲段支架号： {startNo} ");
                }
            } else {
                Debug.Log("未检测到明显的弯曲段。");
            }
        }
    }

    private void Handle() {
        if (curInnerIndex + 9 >= modifyData[curOuterIndex].Count) {
            curInnerIndex = 0;
            curOuterIndex = (curOuterIndex + 1) % modifyData.Count;

        }
        chart.RemoveData();
        chart.AddSerie<Line>("数据：" + curInnerIndex.ToString());
        chart.AddSerie<Line>("数据：" + (curInnerIndex + 1).ToString());
        chart.AddSerie<Line>("数据：" + (curInnerIndex + 2).ToString());
        chart.AddSerie<Line>("数据：" + (curInnerIndex + 3).ToString());
        chart.AddSerie<Line>("数据：" + (curInnerIndex + 4).ToString());
        chart.AddSerie<Line>("数据：" + (curInnerIndex + 5).ToString());
        chart.AddSerie<Line>("数据：" + (curInnerIndex + 6).ToString());
        chart.AddSerie<Line>("数据：" + (curInnerIndex + 7).ToString());
        chart.AddSerie<Line>("数据：" + (curInnerIndex + 8).ToString());
        chart.AddSerie<Line>("数据：" + (curInnerIndex + 9).ToString());

        List<XingchengData> cur = modifyData[curOuterIndex][curInnerIndex];
        List<XingchengData> cur1 = modifyData[curOuterIndex][curInnerIndex + 1];
        List<XingchengData> cur2 = modifyData[curOuterIndex][curInnerIndex + 2];
        List<XingchengData> cur3 = modifyData[curOuterIndex][curInnerIndex + 3];
        List<XingchengData> cur4 = modifyData[curOuterIndex][curInnerIndex + 4];
        List<XingchengData> cur5 = modifyData[curOuterIndex][curInnerIndex + 5];
        List<XingchengData> cur6 = modifyData[curOuterIndex][curInnerIndex + 6];
        List<XingchengData> cur7 = modifyData[curOuterIndex][curInnerIndex + 7];
        List<XingchengData> cur8 = modifyData[curOuterIndex][curInnerIndex + 8];
        List<XingchengData> cur9 = modifyData[curOuterIndex][curInnerIndex + 9];

        for (int i = 0; i < maxNo; i++) {
            chart.AddXAxisData(cur[i].no.ToString());
            chart.AddData(0, cur[i].tuiLiuXC);
            chart.AddData(1, cur1[i].tuiLiuXC);
            chart.AddData(2, cur2[i].tuiLiuXC);
            chart.AddData(3, cur3[i].tuiLiuXC);
            chart.AddData(4, cur4[i].tuiLiuXC);
            chart.AddData(5, cur5[i].tuiLiuXC);
            chart.AddData(6, cur6[i].tuiLiuXC);
            chart.AddData(7, cur7[i].tuiLiuXC);
            chart.AddData(8, cur8[i].tuiLiuXC);
            chart.AddData(9, cur9[i].tuiLiuXC);
        }

        curInnerIndex += 10;
    }

    
}
