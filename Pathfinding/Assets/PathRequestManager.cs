using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PathRequestManager : MonoBehaviour {

    Queue<PathRequest> pathReqQueue = new Queue<PathRequest>();
    PathRequest currentPathReq;
    Pathfinding pathfinding;
    bool isProcessingPath;
    static PathRequestManager instance;

    private void Awake()
    {
        instance = this;
        pathfinding = GetComponent<Pathfinding>();
    }
    struct PathRequest
    {
        public Vector3 pathStart;
        public Vector3 pathEnd;
        public Action<Vector3[], bool> callback;
        public PathRequest(Vector3 _pathStart, Vector3 _pathEnd, Action<Vector3[], bool> _callback)
        {
            pathStart = _pathStart;
            pathEnd = _pathEnd;
            callback = _callback;
        }
    }
	public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback)
    {
        PathRequest newRequest = new PathRequest(pathStart, pathEnd, callback);
        instance.pathReqQueue.Enqueue(newRequest);
        instance.DoNextProcessing(); 
    }

    void DoNextProcessing() // try next path 
    {
        if (!isProcessingPath && pathReqQueue.Count > 0)
        {
            currentPathReq = pathReqQueue.Dequeue();
            isProcessingPath = true;
            pathfinding.StartPathFind(currentPathReq.pathStart, currentPathReq.pathEnd);
        }
    }

    public void finishedProcessingPath(Vector3[] path, bool success)
    {
        currentPathReq.callback(path, success);
        isProcessingPath = false;
        DoNextProcessing();
    }

}
