using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.VersionControl;
using UnityEngine;
using XCharts.Runtime;

public class XchartTest : MonoBehaviour
{
    public LineChart chart;
    public string jsonName;

    private List<XingchengData> allData;

    public int curIndex = 0;
    private List<List<XingchengData>> modifyData = new List<List<XingchengData>>();
    private int maxNo = 169;

    private void Start() {
        string path = Application.streamingAssetsPath + $"/{jsonName}.json";
        string msg = System.IO.File.ReadAllText(path);
        allData = JsonConvert.DeserializeObject<List<XingchengData>>(msg);
        for (int i = 0; i < allData.Count; i += maxNo) {
            var temp = new List<XingchengData>();
            for (int j = i; j < i + maxNo; j++) {
                temp.Add(allData[j]);
            }
            temp = temp.OrderBy(t => t.no).ToList();
            modifyData.Add(temp);
        }
        Debug.Log($"modifyCount = {modifyData.Count}");
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (curIndex + 4 >= modifyData.Count) {
                curIndex = 0;
            }
            chart.RemoveData();
            chart.AddSerie<Line>("数据：" + curIndex.ToString());
            chart.AddSerie<Line>("数据：" + (curIndex + 1).ToString());
            chart.AddSerie<Line>("数据：" + (curIndex + 2).ToString());
            chart.AddSerie<Line>("数据：" + (curIndex + 3).ToString());
            chart.AddSerie<Line>("数据：" + (curIndex + 4).ToString());

            List<XingchengData> cur = modifyData[curIndex];
            List<XingchengData> cur1 = modifyData[curIndex + 1];
            List<XingchengData> cur2 = modifyData[curIndex + 2];
            List<XingchengData> cur3 = modifyData[curIndex + 3];
            List<XingchengData> cur4 = modifyData[curIndex + 4];

            for (int i = 0; i < maxNo; i++) {
                chart.AddXAxisData(cur[i].no.ToString());
                chart.AddData(0, cur[i].tuiLiuXC);
                chart.AddData(1, cur1[i].tuiLiuXC);
                chart.AddData(2, cur2[i].tuiLiuXC);
                chart.AddData(3, cur3[i].tuiLiuXC);
                chart.AddData(4, cur4[i].tuiLiuXC);
            }
            curIndex += 5;
        }
    }
}
