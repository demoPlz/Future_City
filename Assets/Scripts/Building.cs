using UnityEngine;
using System.Collections.Generic;

public class Building : MonoBehaviour
{
    [System.Serializable]
    public struct FloorWindowLayoutData
    {
        public List<float> frontLayout, backLayout, leftLayout, rightLayout;
    }

    [System.Serializable]
    public struct BuildingWindowLayoutData
    {
        public int slot;
        public List<FloorWindowLayoutData> floorLayouts;
    }

    public float buildingLength, buildingWidth;

    public Floor[] floors;
    public GameObject[] floorObjs;
    public GameObject[] plans;
    public int planIdx;
    public int floorNum;

    List<BuildingWindowLayoutData> savedLayoutDatas;

    void Start()
    {
        floorNum = 4;
        SaveBuildingLayout(0);
    }

    public void SaveBuildingLayout(int slot)
    {
        if (savedLayoutDatas == null) savedLayoutDatas = new List<BuildingWindowLayoutData>();

        for (int i = 0; i < savedLayoutDatas.Count; i++)
        {
            BuildingWindowLayoutData layoutData = savedLayoutDatas[i];
            if (layoutData.slot == slot)
            {
                layoutData.floorLayouts = new List<FloorWindowLayoutData>();
                foreach (Floor f in floors)
                {
                    FloorWindowLayoutData floorLayout = new FloorWindowLayoutData();
                    floorLayout.frontLayout = new List<float>();
                    foreach (Window w in f.layout.frontWindows)
                    {
                        floorLayout.frontLayout.Add(w.gameObject.transform.localPosition.z / buildingLength + .5f);
                    }
                    floorLayout.backLayout = new List<float>();
                    foreach (Window w in f.layout.backWindows)
                    {
                        floorLayout.backLayout.Add(w.gameObject.transform.localPosition.z / buildingLength + .5f);
                    }
                    floorLayout.leftLayout = new List<float>();
                    foreach (Window w in f.layout.leftWindows)
                    {
                        floorLayout.leftLayout.Add(w.gameObject.transform.localPosition.x / buildingWidth + .5f);
                    }
                    floorLayout.rightLayout = new List<float>();
                    foreach (Window w in f.layout.rightWindows)
                    {
                        floorLayout.rightLayout.Add(w.gameObject.transform.localPosition.x / buildingWidth + .5f);
                    }
                }

                return;
            }
        }

        BuildingWindowLayoutData newLayout = new BuildingWindowLayoutData();
        newLayout.slot = slot;
        newLayout.floorLayouts = new List<FloorWindowLayoutData>();
        foreach (Floor f in floors)
        {
            FloorWindowLayoutData floorLayout = new FloorWindowLayoutData();
            floorLayout.frontLayout = new List<float>();
            foreach (Window w in f.layout.frontWindows)
            {
                floorLayout.frontLayout.Add(w.gameObject.transform.localPosition.z / buildingLength + .5f);
            }
            floorLayout.backLayout = new List<float>();
            foreach (Window w in f.layout.backWindows)
            {
                floorLayout.backLayout.Add(w.gameObject.transform.localPosition.z / buildingLength + .5f);
            }
            floorLayout.leftLayout = new List<float>();
            foreach (Window w in f.layout.leftWindows)
            {
                floorLayout.leftLayout.Add(w.gameObject.transform.localPosition.x / buildingWidth + .5f);
            }
            floorLayout.rightLayout = new List<float>();
            foreach (Window w in f.layout.rightWindows)
            {
                floorLayout.rightLayout.Add(w.gameObject.transform.localPosition.x / buildingWidth + .5f);
            }

            newLayout.floorLayouts.Add(floorLayout);
        }

        savedLayoutDatas.Add(newLayout);
    }

    public void LoadBuildingLayout(int slot)
    {
        for (int i = 0; i < savedLayoutDatas.Count; i++)
        {
            BuildingWindowLayoutData layoutData = savedLayoutDatas[i];
            if (layoutData.slot == slot)
            {
                Debug.Log(layoutData.floorLayouts.Count);
                for (int j = 0; j < floors.Length; j++)
                {
                    SetWindowsPattern(j, layoutData.floorLayouts[j].frontLayout.ToArray());
                }

                return;
            }
        }
    }

    public void SwitchUpPlan()
    {
        plans[planIdx].SetActive(false);
        planIdx++;
        if (planIdx == plans.Length) planIdx = 0;
        plans[planIdx].SetActive(true);
    }

    public void SwitchDownPlan()
    {
        plans[planIdx].SetActive(false);
        planIdx--;
        if (planIdx == -1) planIdx = plans.Length - 1;
        plans[planIdx].SetActive(true);
    }

    public void SetPlanOne(){
        plans[planIdx].SetActive(false);
        planIdx = 0;
        plans[planIdx].SetActive(true);
    }

    public void SetPlanTwo(){
        plans[planIdx].SetActive(false);
        planIdx = 1;
        plans[planIdx].SetActive(true);
    }

    public void SetPlanThree(){
        plans[planIdx].SetActive(false);
        planIdx = 2;
        plans[planIdx].SetActive(true);
    }

    public void SetPlanFour(){
        plans[planIdx].SetActive(false);
        planIdx = 3;
        plans[planIdx].SetActive(true);
    }

    public void SetPlanFive(){
        plans[planIdx].SetActive(false);
        planIdx = 4;
        plans[planIdx].SetActive(true);
    }
    public void SwitchPlan(int idx)
    {
        plans[planIdx].SetActive(false);
        planIdx = idx;
        plans[planIdx].SetActive(true);
    }

    public void AddFloor()
    {
        floorNum++;

        for (int i = 0; i < Mathf.Min(4, floorNum); i++)
        {
            floorObjs[i].SetActive(true);
        }    
    }

    public void RemoveFloor()
    {
        floorNum--;
        floorNum = Mathf.Max(floorNum, 0);
        if (floorNum < 4)
        {
            for (int i = floorNum; i < 4; i++)
            {
                floorObjs[i].SetActive(false);
            }
        }
    }

    public void SetWindowsPattern(int floor, float[] patterns)
    {
        Floor f = floors[floor];
        foreach (Window w in f.layout.frontWindows)
            Destroy(w.gameObject);

        f.layout.frontWindows = new List<Window>();
        foreach (float p in patterns)
        {
            f.AddWindow(0, new Vector3(-35.51f, 5f, (p - 0.5f) * buildingLength));
        }
    }

    public void CopyWindowsPatterns(int floorToCopy, int floorToModify)
    {
        Floor f = floors[floorToCopy];

        List<float> patterns = new List<float>();

        foreach (Window w in f.layout.frontWindows)
        {
            patterns.Add(w.gameObject.transform.localPosition.z / buildingLength + 0.5f);
        }

        SetWindowsPattern(floorToModify, patterns.ToArray());
    }
}
