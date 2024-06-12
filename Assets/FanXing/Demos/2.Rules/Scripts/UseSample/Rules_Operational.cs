using UnityEngine;
using System.Collections.Generic;
using System;
namespace FanXing.Demos.Rules
{
public class Rules_Operational : MonoBehaviour
{
    
    [Header("第一条规则：单位属性")]
    [SerializeField]
    GameObject unitPrefab;
    private int maxTries = 100; // 最大尝试次数
    [SerializeField] int minX = -4;
    [SerializeField] int maxX = 4;
    [SerializeField] int minY = -4;
    [SerializeField] int maxY = 4;

    [Header("第二条规则：空间")]
    [SerializeField]
    GameObject geoNodePrefab;
    GameObject geoNode;
    private List<Vector3> generatedPositions = new List<Vector3>();

    [Header("第三条规则：时间")]
    [SerializeField]
    GameObject timeCountPrefab;
    GameObject timeCount;
    private float totalSeconds = 0f;
    private bool isPaused = false;
    private float pauseTime = 0f;
    private int pausedTimeCount = 0;
    void Start()
    {
        Events.OnCreate_2D_Geometry += Create_2D_Geometry;
        Events.OnCreateUnit += CreateUnit;
        Events.OnTimeCount += TimeCountStart;
        Events.OnClearAllWorldObjects += ClearAllWorldObjects;
    }
    void Update()
    {
        switch (TemporaryStorage.Rules_Constituative)
        {
            case TemporaryStorage.Rules_Constituative_State.Rule_one:
                
                break;
            case TemporaryStorage.Rules_Constituative_State.Rule_two:
                Update_Create_2D_Geometry();
                break;
            case TemporaryStorage.Rules_Constituative_State.Rule_three:
                Update_TimeCount();
                break;
            default:
                break;
        }   
    }
    void Update_TimeCount()
    {
        if(!timeCount)return;
        if (Input.GetMouseButtonDown(0)) // 按下P键暂停/继续计时器
        {
            
            TogglePause();
        }

        if (!isPaused)
        {
            totalSeconds += Time.deltaTime;
        }

        DisplayTime();
    }
    void TogglePause()
    {
        if (isPaused)
        {
            // 继续计时
            pauseTime = Time.time - pauseTime;
        }
        else
        {
            // 暂停计时
            pauseTime = Time.time;
            pausedTimeCount++;
            
        }
        isPaused = !isPaused;
    }

    void TimeCountStart()
    {
        
        if(!timeCount)
        {
            pausedTimeCount = 0;
            totalSeconds = 0f;
            timeCount = Instantiate(timeCountPrefab, transform);
            timeCount.GetComponent<TimeCounter>().Init();
        }
    }
    void DisplayTime()
    {
        int hours = Mathf.FloorToInt(totalSeconds / 3600);
        int minutes = Mathf.FloorToInt((totalSeconds % 3600) / 60);
        int seconds = Mathf.FloorToInt(totalSeconds % 60);

        string timeString = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
        timeCount.GetComponent<TimeCounter>().timeData.textMeshPro.text = timeString;
        timeCount.GetComponent<TimeCounter>().timeData.textMeshPro.text += "\nPaused " + pausedTimeCount;
        // Debug.Log("Time Elapsed: " + timeString);

    }
    void ClearAllWorldObjects()
    {
        generatedPositions.Clear();
        if(transform.childCount == 0)return;
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
    void Create_2D_Geometry()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f;
        Vector3 worldPos = Camera.main.ScreenPointToRay(mousePos).origin;
        if(geoNode)Events.InvokeOnClearAllWorldObjects();
        geoNode = Instantiate(geoNodePrefab, worldPos, Quaternion.identity, transform);
    }
    void Update_Create_2D_Geometry()
    {
        if(!geoNode)return;
        // 获取鼠标在屏幕上的位置
        Vector3 mousePos = Input.mousePosition;
            
        // 将鼠标位置转换为世界坐标系
        mousePos.z = 10f; // 设置一个合适的距离,使得物体在合适的位置
        Vector3 worldPos = Camera.main.ScreenPointToRay(mousePos).origin;
            
        // 将物体的位置设置为世界坐标位置
        geoNode.transform.position = new Vector3(worldPos.x, worldPos.y, transform.position.z);
        if (Input.GetMouseButtonDown(0))
        {
            geoNode.GetComponent<GeoNode>().Init();
            geoNode = Instantiate(geoNodePrefab, worldPos, Quaternion.identity, transform);
        }
    }
    void CreateUnit()
    {
        Vector3 randomPosition = GetNonOverlappingPosition();
        if(randomPosition == Vector3.one * 10086)return;
        generatedPositions.Add(randomPosition);
        GameObject go = Instantiate(unitPrefab, randomPosition, Quaternion.identity, transform);
        go.GetComponent<UnitBehavior>().DisplayAttributes();
    }
    private Vector3 GetNonOverlappingPosition()
    {
        float randomX;
        float randomY;
        Vector3 randomPosition;
        int tries = 0;

        while (tries < maxTries)
        {
            randomX = UnityEngine.Random.Range(minX, maxX);
            randomY = UnityEngine.Random.Range(minY, maxY);
            randomPosition = new Vector3(randomX, randomY, 0f);

            if (!IsPositionOccupied(randomPosition))
            {
                return randomPosition;
            }

            tries++;
        }

        return Vector3.one * 10086; // 如果达到最大尝试次数仍找不到合适的位置,返回零向量
    }
    private bool IsPositionOccupied(Vector3 position)
    {
        foreach (Vector3 generatedPosition in generatedPositions)
        {
            if (Vector3.Distance(position, generatedPosition) < 1.0f) // 假设物体之间的最小距离为1.0f
            {
                return true;
            }
        }
        return false;
    }
}

}