using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class PathSmoothingScript : MonoBehaviour
{
    [SerializeField] GameObject dot;
    public List<Vector2> baseList;
    public List<Vector2> interpolatedList;
    [SerializeField] int samples;

    [SerializeField] Material blue;
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }

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

    List<Vector2> Interpolate(List<Vector2> path, int samplesPerSegment)
    {
        if (path.Count < 2){
            return path;
        }

        if (samplesPerSegment == 0)
        {
            samplesPerSegment = 1;
        }

        List<Vector2> resultPath = new List<Vector2>();
        resultPath.Add(path[0]);

        if (path.Count == 2)
        {
            for (int sampleIndex = 1; sampleIndex < samplesPerSegment; sampleIndex++)
            {
                float t = (float)sampleIndex / samplesPerSegment;
                Vector2 interpolatedPoint = path[0] * (1.0f - t) + path[1] * t;
                resultPath.Add(interpolatedPoint);
            }
            resultPath.Add(path[1]);
            return resultPath;
        }

        for (int segmentIndex = 0; segmentIndex < path.Count - 1; segmentIndex++)
        {
            Vector2 currPoint = path[segmentIndex];
            Vector2 nextPoint = path[segmentIndex + 1];
            Vector2 pointAfterNext = (segmentIndex + 2 < path.Count) ? path[segmentIndex + 2] : path[segmentIndex + 1];

            for (int sampleIndex = 1; sampleIndex <= samplesPerSegment; sampleIndex++)
            {
                float t = (float)sampleIndex / samplesPerSegment;
                Vector2 interpolatedPoint = QuadraticBezier(currPoint, nextPoint, pointAfterNext, t);
                resultPath.Add(interpolatedPoint);
            }
        }

        return resultPath;
    }

    public void DrawPath()
    {
        interpolatedList.Clear();
        interpolatedList = Interpolate(baseList, samples);

        foreach (Vector2 i in baseList)
        {
            Vector3 newPoint = new Vector3(i.x, 0.0f, i.y);
            GameObject newObject = GameObject.Instantiate(dot, newPoint, Quaternion.identity);
            newObject.gameObject.GetComponent<MeshRenderer>().material = blue;
        }

        foreach (Vector2 i in interpolatedList)
        {
            Vector3 newPoint = new Vector3(i.x, 0.0f, i.y);
            GameObject.Instantiate(dot, newPoint, Quaternion.identity);
        }
    }
}
