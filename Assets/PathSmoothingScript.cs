using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEditor;

public class PathSmoothingScript : MonoBehaviour
{
    [SerializeField] GameObject dot;
    public List<Vector2> baseList;
    List<Vector2> interpolatedList;
    [SerializeField] int samples;
    List<GameObject> drawnPath;

    [SerializeField] Material blue;

    //get a smoothed curve between three points, will pass by middle "control" point
    Vector2 QuadraticBezier(Vector2 start, Vector2 control, Vector2 end, float interpolationFactor)
    {
        Vector2 result;
        float complementaryFactor = 1.0f - interpolationFactor;
        float interpolationSquared = interpolationFactor * interpolationFactor;
        float complementarySquared = complementaryFactor * complementaryFactor;

        result.x = complementarySquared * start.x + 2 * complementaryFactor * interpolationFactor * control.x + interpolationSquared * end.x;
        result.y = complementarySquared * start.y + 2 * complementaryFactor * interpolationFactor * control.y + interpolationSquared * end.y;

        return result;
    }

    //smoothed curve between four points, guaranteed to hit every point
    Vector2 GetSplinePoint(float t, Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
    {
        Vector2 result = new Vector2();

        //store some variables
        t = t - (int)t;
        float tSquared = t * t;
        float tCubed = tSquared * t;

        //calculate degree of influence for each point
        float q1 = (-1.0f * tCubed) + (2.0f * tSquared) - t;
        float q2 = (3.0f * tCubed) - (5.0f * tSquared) + 2.0f;
        float q3 = (-3.0f * tCubed) + (4.0f * tSquared) + t;
        float q4 = tCubed - tSquared;

        //apply degrees of influence to each point accordingly and calculate x and y coordinates
        result.x = 0.5f * ((p0.x * q1) + (p1.x * q2) + (p2.x * q3) + (p3.x * q4));
        result.y = 0.5f * ((p0.y * q1) + (p1.y * q2) + (p2.y * q3) + (p3.y * q4));

        return result;
    }

    public List<Vector2> Interpolate(List<Vector2> path, int samplesPerSegment)
    {
        //path has one or zero points safeguard
        if (path.Count < 2){
            return path;
        }

        if (samplesPerSegment == 0)
        {
            samplesPerSegment = 1;
        }

        List<Vector2> resultPath = new List<Vector2>();
        resultPath.Add(path[0]);

        //path has two points safeguard
        if (path.Count == 2)
        {
            //sir that is a line
            for (int sampleIndex = 1; sampleIndex < samplesPerSegment; sampleIndex++)
            {
                float t = (float)sampleIndex / samplesPerSegment;
                Vector2 interpolatedPoint = path[0] * (1.0f - t) + path[1] * t;
                resultPath.Add(interpolatedPoint);
            }
            resultPath.Add(path[1]);
            return resultPath;
        }

        //path has three points safeguard
        if (path.Count == 3)
        {
            //bezier curve it
            for (int sampleIndex = 1; sampleIndex < samplesPerSegment; sampleIndex++)
            {
                float t = (float)sampleIndex / samplesPerSegment;
                Vector2 interpolatedPoint = QuadraticBezier(path[0], path[1], path[2], t);
                resultPath.Add(interpolatedPoint);
            }
            resultPath.Add(path[2]);
            return resultPath;
        }

        int p0, p1, p2, p3; //indices of the points we'll reference in the method

        for (float i = 0; i < (float)path.Count - 3.0f; i += 0.05f)
        {
            //set reference point indices
            p1 = (int)i + 1;
            p2 = p1 + 1;
            p3 = p2 + 1;
            p0 = p1 - 1;

            //get new point and add it to the new path
            Vector2 interpolatedPoint = GetSplinePoint(i, path[p0], path[p1], path[p2], path[p3]);
            resultPath.Add(interpolatedPoint);
        }
        resultPath.Add(path[path.Count - 1]);

        return resultPath;
    }

    public void DrawPath(List<Vector2> path)
    {
        //clear the screen of the previous path
        for (int i = drawnPath.Count - 1; i >= 0; i--)
        {
            DestroyImmediate(drawnPath[i]);
        }
        drawnPath.Clear();

        //reset interpolated path
        interpolatedList.Clear();
        interpolatedList = Interpolate(path, samples);

        //draw the initial points in blue
        foreach (Vector2 i in path)
        {
            //initialize game object at point and add it to the drawn path list
            Vector3 newPoint = new Vector3(i.x, 0.0f, i.y);
            GameObject newObject = GameObject.Instantiate(dot, newPoint, Quaternion.identity);
            newObject.gameObject.GetComponent<MeshRenderer>().material = blue;
            drawnPath.Add(newObject);
        }

        //draw the smoothed path
        foreach (Vector2 i in interpolatedList)
        {
            //initialize game object at point and add it to the drawn path list
            Vector3 newPoint = new Vector3(i.x, 0.0f, i.y);
            GameObject newObject = GameObject.Instantiate(dot, newPoint, Quaternion.identity);
            drawnPath.Add(newObject);
        }
    }
}


[CustomEditor(typeof(PathSmoothingScript))]
public class PathSmoothingEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var smoothing = (PathSmoothingScript)target;

        EditorGUILayout.LabelField("Path Smoothing");

        if (GUILayout.Button("Interpolate Path", GUILayout.Width(200)))
        {
            smoothing.Interpolate(smoothing.baseList, 7);
        }

        if (GUILayout.Button("Draw Path", GUILayout.Width(200)))
        {
            smoothing.DrawPath(smoothing.baseList);
        }
    }
}