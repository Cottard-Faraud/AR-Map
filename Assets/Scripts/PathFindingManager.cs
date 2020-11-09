using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PathFindingManager : MonoBehaviour
{
    public static PathFindingManager Instance;

    private LineRenderer line;
    private GameObject origin;
    private NavMeshAgent agent;

    public float lineWidth = 0.1f;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        line = GetComponent<LineRenderer>();
        agent = GetComponent<NavMeshAgent>();

        ResetLineRenderer();
    }

    public void ResetLineRenderer()
    {
        line.startWidth = 0;
        line.endWidth = 0;
    }

    public void SetOrigin(GameObject g)
    {
        origin = g;
    }

    public void GeneratePath(GameObject origine, GameObject target)
    {
        StartCoroutine(GetPath(origine, target));
    }

    IEnumerator GetPath(GameObject origine, GameObject target)
    {
        line.SetPosition(0, origine.transform.position);
        line.startWidth = lineWidth;
        line.endWidth = lineWidth;

        agent.transform.position = origine.transform.position;
        agent.SetDestination(target.transform.position);

        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        agent.isStopped = true;
        DrawPath(agent.path);
    }

    public void DrawPath(NavMeshPath path)
    {
        if (path.corners.Length < 2)
            return;

        line.positionCount = path.corners.Length;

        for (var i = 1; i < path.corners.Length; i++)
        {
            line.SetPosition(i, path.corners[i]);
        }
    }
}
