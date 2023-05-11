using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Edge = IPathFinder.Edge;
using Rectangle = IPathFinder.Rectangle;

public class PathFinder : IPathFinder
{
    // I haven't written a pathfinder from scratch before and so far this is the best I've been able to achieve.
    // Yes, there's obviously room for improvement, but it seems to me that it would be perfectly usable in the game.

    public IEnumerable<Vector2> GetPath(Vector2 A, Vector2 C, IEnumerable<Edge> edges)
    {
        List<Vector2> path = new List<Vector2>();

        // Add the start position to the path
        path.Add(A);

        Vector2 currentPosition = A;

        HashSet<Edge> visitedEdges = new HashSet<Edge>();

        while (true)
        {
            Edge closestEdge = FindClosestEdge(currentPosition, edges, visitedEdges);

            if (closestEdge.First.Min == Vector2.zero && closestEdge.First.Max == Vector2.zero)
            {
                break;
            }

            Vector2 nextPosition = (closestEdge.Start + closestEdge.End) / 2f;

            if (IsInsideRectangles(C, closestEdge.First, closestEdge.Second))
            {
                // Reached a position where C is inside one of the intersecting rectangles, terminate the pathfinding
                path.Add(nextPosition);
                path.Add(C);
                break;
            }

            path.Add(nextPosition);
            currentPosition = nextPosition;

            visitedEdges.Add(closestEdge);
        }

        OptimizePath(path);

        return path;
    }

    private void OptimizePath(List<Vector2> path)
    {
        if (path.Count < 3)
        {
            return;
        }

        for (int i = 1; i < path.Count - 1; i++)
        {
            Vector2 previousPoint = path[i - 1];
            Vector2 currentPoint = path[i];
            Vector2 nextPoint = path[i + 1];

            Vector2 direction1 = currentPoint - previousPoint;
            Vector2 direction2 = nextPoint - currentPoint;

            if (Mathf.Approximately(direction1.x, direction2.x) || Mathf.Approximately(direction1.y, direction2.y))
            {
                path.RemoveAt(i);
                i--;
            }
        }
    }

    private Edge FindClosestEdge(Vector2 position, IEnumerable<Edge> edges, HashSet<Edge> visitedEdges)
    {
        float minDistance = float.MaxValue;
        Edge closestEdge = new Edge();

        foreach (Edge edge in edges)
        {
            if (visitedEdges.Contains(edge))
            {
                continue;
            }

            float distanceStart = Vector2.Distance(position, edge.Start);
            float distanceEnd = Vector2.Distance(position, edge.End);

            if (distanceStart < minDistance)
            {
                minDistance = distanceStart;
                closestEdge = edge;
            }

            if (distanceEnd < minDistance)
            {
                minDistance = distanceEnd;
                closestEdge = edge;
            }
        }

        return closestEdge;
    }

    private bool IsInsideRectangles(Vector2 position, Rectangle rect1, Rectangle rect2)
    {
        return IsInsideRectangle(position, rect1) || IsInsideRectangle(position, rect2);
    }

    private bool IsInsideRectangle(Vector2 position, Rectangle rect)
    {
        return position.x >= rect.Min.x && position.x <= rect.Max.x && position.y >= rect.Min.y && position.y <= rect.Max.y;
    }
}