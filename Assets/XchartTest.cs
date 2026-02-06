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
                temp = temp.OrderBy(t => t.no).ToList();
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
            curInnerIndex = modifyData[curOuterIndex].Count - 1 - 4;
            Handle();
        }
    }

    private void Handle() {
        if (curInnerIndex + 4 >= modifyData[curOuterIndex].Count) {
            curInnerIndex = 0;
            curOuterIndex = (curOuterIndex + 1) % modifyData.Count;

        }
        chart.RemoveData();
        chart.AddSerie<Line>("数据：" + curInnerIndex.ToString());
        chart.AddSerie<Line>("数据：" + (curInnerIndex + 1).ToString());
        chart.AddSerie<Line>("数据：" + (curInnerIndex + 2).ToString());
        chart.AddSerie<Line>("数据：" + (curInnerIndex + 3).ToString());
        chart.AddSerie<Line>("数据：" + (curInnerIndex + 4).ToString());

        List<XingchengData> cur = modifyData[curOuterIndex][curInnerIndex];
        List<XingchengData> cur1 = modifyData[curOuterIndex][curInnerIndex + 1];
        List<XingchengData> cur2 = modifyData[curOuterIndex][curInnerIndex + 2];
        List<XingchengData> cur3 = modifyData[curOuterIndex][curInnerIndex + 3];
        List<XingchengData> cur4 = modifyData[curOuterIndex][curInnerIndex + 4];

        for (int i = 0; i < maxNo; i++) {
            chart.AddXAxisData(cur[i].no.ToString());
            chart.AddData(0, cur[i].tuiLiuXC);
            chart.AddData(1, cur1[i].tuiLiuXC);
            chart.AddData(2, cur2[i].tuiLiuXC);
            chart.AddData(3, cur3[i].tuiLiuXC);
            chart.AddData(4, cur4[i].tuiLiuXC);
        }
        curInnerIndex += 5;
    }
}
