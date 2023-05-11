using System.Collections.Generic;
using UnityEngine;
using Edge = IPathFinder.Edge;
using Rectangle = IPathFinder.Rectangle;

public class TestPathFinderController : MonoBehaviour
{
    [SerializeField] private Transform _startPoint;
    [SerializeField] private Transform _endPoint;
    [SerializeField] private LineRenderer _lr;

    private PathFinder pathFinder;
    private List<Edge> edges = new List<Edge>();

    private void Start()
    {
        pathFinder = new PathFinder();

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector2 start = new Vector2(_startPoint.position.x, _startPoint.position.y);
            Vector2 end = new Vector2(_endPoint.position.x, _endPoint.position.y);
            edges = CollectEdges();
            DrawTheWay(pathFinder.GetPath(start, new Vector2(end.x, end.y), edges));
        }
    }

    public List<Edge> CollectEdges()
    {
        var edges = new List<Edge>();

        // Collect all 2D sprite renderers in the scene
        SpriteRenderer[] sprites = FindObjectsOfType<SpriteRenderer>();

        // Iterate over the sprite renderers to collect their edges
        for (int i = 0; i < sprites.Length; i++)
        {
            SpriteRenderer spriteA = sprites[i];
            Rectangle rectA = CreateRectangle(spriteA.bounds);

            for (int j = i + 1; j < sprites.Length; j++)
            {
                SpriteRenderer spriteB = sprites[j];
                Rectangle rectB = CreateRectangle(spriteB.bounds);

                if (IntersectsOrTouches(rectA, rectB))
                {
                    // Add the edge to the list
                    Edge edge = CreateEdge(rectA, rectB);
                    edges.Add(edge);
                }
            }
        }
        return edges;
    }

    private bool IntersectsOrTouches(Rectangle rectA, Rectangle rectB)
    {
        bool intersectsX = rectA.Min.x <= rectB.Max.x && rectA.Max.x >= rectB.Min.x;
        bool intersectsY = rectA.Min.y <= rectB.Max.y && rectA.Max.y >= rectB.Min.y;

        return intersectsX && intersectsY;
    }

    private Edge CreateEdge(Rectangle rectA, Rectangle rectB)
    {
        float startX = Mathf.Max(rectA.Min.x, rectB.Min.x);
        float startY = Mathf.Max(rectA.Min.y, rectB.Min.y);
        float endX = Mathf.Min(rectA.Max.x, rectB.Max.x);
        float endY = Mathf.Min(rectA.Max.y, rectB.Max.y);

        return new Edge(rectA, rectB, new Vector3(startX, startY, 0f), new Vector3(endX, endY, 0f));
    }

    private Rectangle CreateRectangle(Bounds bounds)
    {
        Vector2 min = new Vector2(bounds.min.x, bounds.min.y);
        Vector2 max = new Vector2(bounds.max.x, bounds.max.y);
        return new Rectangle(min, max);
    }

    private void DrawTheWay(IEnumerable<Vector2> wayPoints)
    {
        var data = wayPoints as List<Vector2>;
        if (data == null)
            return;

        _lr.positionCount = data.Count;

        for (int i = 0; i < data.Count; i++)
        {
            _lr.SetPosition(i, new Vector3(data[i].x, data[i].y, -1));
        }
        
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color32(40, 40, 40, 120);
        foreach (var item in edges)
        {
            Gizmos.DrawCube(item.Start, new Vector3(0.2f, 0.2f, 0.2f));
            Gizmos.DrawCube(item.End, new Vector3(0.2f, 0.2f, 0.2f));
        }
    }
}
