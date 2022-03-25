using System.Collections.Generic;
using UnityEngine;

public class ClientPaths : MonoBehaviour
{
    [SerializeField]
    private List<Transform> _nodes = new List<Transform>();

    private static ClientPaths instance;
    public static List<Transform> Nodes {get { return instance._nodes; } }
    
    private void Awake()
    {
        instance = this;
    }

    private void OnDrawGizmos()
    {
        foreach (var node in _nodes)
        {
            Gizmos.DrawSphere(node.position, 0.5f);
        }
    }
}
