using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
namespace FanXing.Demos.Rules
{
public class AStarPathfinding : UnitBehavior
{
    [SerializeField]
    Transform start; // 起点
    [SerializeField]
    Transform target; // 终点
    [SerializeField]
    LayerMask unwalkableMask; // 不可行走的层
    [SerializeField]
    float nodeRadius; // 节点半径
    [SerializeField]
    float gridSize; // 网格大小
    [SerializeField]
    Vector3 gridWorldSize = new(10,1,10); // 网格世界大小
    Node[,] grid; // 网格
    [SerializeField]
    float speed = 5f;
    [SerializeField]
    LineRenderer lineRendererPrefab; // 预制的 LineRenderer 对象
    LineRenderer lineRenderer; // 实例化的 LineRenderer 对象
    public List<Node> path = new(); // A*算法找到的路径
    public bool IsPathFound
    {
        get 
        { 
            return path.Count > 0; 
        }
    }
    
    private bool isPreview =false;
    Vector3 startPath;
    Vector3 endPath;
    private float timer_pathPreview = 0.0f;
    private float interval_pathPreview = 0.3f;
  
    void Start()
    {
        // Init();
        // CreateGrid_Square();
        // FindPath(start.position, target.position);
        if(attributes.unitType == ScriptableObject_UnitAttributes.UnitType.Role)
        {
            Events.OnMovePanelOpen += OpenMovePanel;

        }else if (attributes.unitType == ScriptableObject_UnitAttributes.UnitType.Enemy)
        {
            Events.OnUnitAuto += OnUnitAuto;
            // OnUnitAuto(true);
        }
        
        
        
    }
    void Update()
    {
        if (isPreview && UnityEngine.Input.GetMouseButtonDown(1)) // 在鼠标右键按下时执行取消路径移动
        {
            if(lineRenderer){Destroy(lineRenderer.gameObject);}
            isPreview = false;
            path.Clear();
            return;
        }
        if (isPreview && UnityEngine.Input.GetMouseButtonDown(0))// 在鼠标左键按下时执行路径移动
        {
            if(lineRenderer){Destroy(lineRenderer.gameObject);}
            Invoke(nameof(LateMove),0.1f);
            // StartCoroutine(FollowPath(path));
            isPreview = false;
            Events.InvokeCloseAllUIWindow();
            return;
        } 
        

        if(isPreview)
        {
            //---
            timer_pathPreview += Time.deltaTime;
            if (timer_pathPreview < interval_pathPreview)return;
            // 执行需要每隔N秒执行一次的代码
            // 创建射线
            Ray ray = Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition);
            RaycastHit hit;
            // 检测射线是否击中世界坐标下的物体
            if (Physics.Raycast(ray, out hit))
            {
                string input = hit.collider.gameObject.name;
                if (input.Contains("Ground") || input.Contains("PlacementArea"))
                {
                    // 检测到击中物体
                    start.position = transform.position;
                    target.position = hit.point;
                    start.position = new Vector3(start.position.x-0.4f, 0.5f, start.position.z -0.4f);
                    target.position = new Vector3(target.position.x-0.4f, 0.5f, target.position.z -0.4f);
                    Init();
                    FindPath(start.localPosition, target.localPosition);
                }
            }
            timer_pathPreview = 0.0f; // 重置计时器
            // --- 
        }
        
        
    }
    void OpenMovePanel(GameObject go,bool isOpen)
    {
        if(!isOpen)return;
        if(go!=gameObject)return;
        CreateGrid_Square();
        Invoke(nameof(LatePreview),0.1f);
    }
    void OnUnitAuto(bool auto)
    {
        
        if(auto)
        {
            attributes.potentialTargets = FindObjectsOfType<Playable_Unit>().ToList().Where(x => x.Attributes.unitType != attributes.unitType).Select(x => x.gameObject).ToList();
            GameObject moveTo = attributes.FindBestTarget();
            if(!moveTo)return;
            start.localPosition = Vector3.zero;
            target.position = moveTo.transform.position;
            CreateGrid_Square();
            Debug.Log("OnUnitAuto" + moveTo.name);
            // start.position = new Vector3(start.position.x+0.71f, 0.5f, start.position.z +0.4f);
            // target.position = new Vector3(target.position.x+0.71f, 0.5f, target.position.z +0.4f);
            FindPath(start.localPosition, target.localPosition);
            StartCoroutine(FollowPath(path));
        }else
        {
            // if(lineRenderer){Destroy(lineRenderer.gameObject);}
            // path.Clear();
        }
        
        
    }
    void LatePreview()
    {
        isPreview = true;
    }
    void LateMove()
    {
        StartCoroutine(FollowPath(path));
    }
    void Init()
    {
        Quaternion myRotation = Quaternion.identity;
        myRotation.eulerAngles = new Vector3(90, 0, 0);
        if(lineRenderer){
            Destroy(lineRenderer.gameObject);}
        lineRenderer = Instantiate(lineRendererPrefab, Vector3.zero, myRotation);
        lineRenderer.positionCount = 0; // 初始设置 LineRenderer 的位置数为0
    }
    void CreateGrid_Square()
    {
        // 计算网格大小和节点数量
        int gridSizeX = Mathf.RoundToInt(gridWorldSize.x / gridSize);
        int gridSizeZ = Mathf.RoundToInt(gridWorldSize.z / gridSize);
        grid = new Node[gridSizeX, gridSizeZ];

        // 创建网格
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.z / 2;
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int z = 0; z < gridSizeZ; z++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * gridSize + nodeRadius) + Vector3.forward * (z * gridSize + nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
                grid[x, z] = new Node(walkable, worldPoint, x, z);
            }
        }
    }

    void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Node startNode = NodeFromWorldPoint(startPos);
        Node targetNode = NodeFromWorldPoint(targetPos);

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost || (openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost))
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                // 找到路径
                RetracePath(startNode, targetNode);
                return;
            }

            foreach (Node neighbor in GetNeighbors(currentNode))
            {
                if (!neighbor.walkable || closedSet.Contains(neighbor))
                {
                    continue;
                }

                int newMovementCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor);
                if (newMovementCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
                {
                    neighbor.gCost = newMovementCostToNeighbor;
                    neighbor.hCost = GetDistance(neighbor, targetNode);
                    neighbor.parent = currentNode;

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);               
                    }
                }
                
                
            }
        }
       
     
    }
    void UpdateLineRenderer(Vector3 position)
    {
        if(!lineRenderer)return;
        lineRenderer.positionCount++; // 增加 LineRenderer 的位置数
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, position); // 设置 LineRenderer 的位置
    }
    

    int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstZ = Mathf.Abs(nodeA.gridZ - nodeB.gridZ);
        return 10 * (dstX + dstZ);
    }

    void RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
            // 更新 LineRenderer 的位置
            UpdateLineRenderer(currentNode.worldPosition);
        }
        path.Reverse();
        this.path = path;
        // 此处可以将路径应用到角色的移动上
        // StartCoroutine(FollowPath(path));
    }
    // 使用协程来处理角色的移动
    IEnumerator FollowPath(List<Node> path)
    {
        // foreach (Node node in path)
        // {
        //     // 在这里实现将角色移动到节点的逻辑，例如：
        //     // transform.position = node.worldPosition;
        //     // 等待一段时间，以便能够看到角色移动的效果
        //     // yield return new WaitForSeconds(0.5f);
        // }
        float distanceThreshold = 0.1f; // 触发到达目标位置的阈值

        for (int i = 0; i < path.Count; i++)
        {
            if(TemporaryStorage.GameTimeIsRunning == false)
            {
                path.Clear();
                yield break;
            }
            Node currentNode = path[i];
            Vector3 targetPosition = currentNode.worldPosition;

            while (Vector3.Distance(transform.position, targetPosition) > distanceThreshold)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
                yield return null; // 等待下一帧
            }
        }
        path.Clear();
        
    }

    List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();
        for (int x = -1; x <= 1; x++)
        {
            for (int z = -1; z <= 1; z++)
            {
                if (x == 0 && z == 0)
                {
                    continue;
                }
                int checkX = node.gridX + x;
                int checkZ = node.gridZ + z;

                if (checkX >= 0 && checkX < grid.GetLength(0) && checkZ >= 0 && checkZ < grid.GetLength(1))
                {
                    neighbors.Add(grid[checkX, checkZ]);
                }
            }
        }
        return neighbors;
    }

    Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + gridWorldSize.x/2) / gridWorldSize.x;
        float percentZ = (worldPosition.z + gridWorldSize.z/2) / gridWorldSize.z;
        percentX = Mathf.Clamp01(percentX);
        percentZ = Mathf.Clamp01(percentZ);

        int x = Mathf.RoundToInt((grid.GetLength(0) - 1) * percentX);
        int z = Mathf.RoundToInt((grid.GetLength(1) - 1) * percentZ);
        return grid[x, z];
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, gridWorldSize);
        if (grid != null)
        {
            foreach (Node n in grid)
            {
                Gizmos.color = (n.walkable) ? Color.white : Color.red;
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (gridSize - 0.1f));
            }
        }
    }

    public class Node
    {
        public bool walkable;
        public Vector3 worldPosition;
        public int gridX;
        public int gridZ;
        public int gCost;
        public int hCost;
        public Node parent;

        public Node(bool _walkable, Vector3 _worldPos, int _gridX, int _gridZ)
        {
            walkable = _walkable;
            worldPosition = _worldPos;
            gridX = _gridX;
            gridZ = _gridZ;
        }

        public int fCost
        {
            get { return gCost + hCost; }
        }
    }
}
}