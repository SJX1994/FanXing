using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using PathCreation.Examples;
using System.Linq;
namespace FanXing.FightDemo
{
    public class FightLayer_Paths : MonoBehaviour
    {
        public List<Vector3> pathPoints = new List<Vector3>();
        [SerializeField]
        List<CylinderMeshCreator> pathCreators = new List<CylinderMeshCreator>();
        //[SerializeField] GameObject spherePrefab;
        public void GetPathVertexs()
        {
            FindObjectsWithScriptRecursively();
            foreach (var pathCreator in pathCreators)
            {
                pathPoints.AddRange(pathCreator.meshHolder.GetComponent<MeshFilter>().mesh.vertices);
            }
            TemporaryStorage.PathPoints = pathPoints;
            // foreach (var point in pathPoints)
            // {
            //     GameObject sphere = GameObject.Instantiate(spherePrefab, point, Quaternion.identity);
            //     sphere.transform.localScale = new Vector3(1, 1, 1);
            // }
            
        }
        void FindObjectsWithScriptRecursively()
        {
            pathCreators = FindObjectsOfType<CylinderMeshCreator>().ToList();
        }
        
    }
}
