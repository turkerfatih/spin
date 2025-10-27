namespace Game.Core
{
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteAlways]
public class BoardCameraFitter : MonoBehaviour
{
    public enum FitMode { MoveCamera, AdjustFOV }

    [Header("References")]
    public Camera cam;
    public Renderer boardRenderer;

    [Header("Settings")]
    [Range(0f, 1f)] public float padding = 0.05f;
    public FitMode fitMode = FitMode.MoveCamera;
    public bool autoUpdate = true;

#if UNITY_EDITOR
    private Vector3 lastBoardPos;
    private Vector3 lastBoardSize;
    private Vector2 lastScreenSize;
#endif

    private void LateUpdate()
    {
        if (cam == null || boardRenderer == null)
            return;

#if UNITY_EDITOR
        // Detect if anything has changed in edit mode
        var bounds = boardRenderer.bounds;
        var screenSize = new Vector2(Screen.width, Screen.height);

        if (!Application.isPlaying)
        {
            if (bounds.center == lastBoardPos &&
                bounds.size == lastBoardSize &&
                screenSize == lastScreenSize)
                return;

            lastBoardPos = bounds.center;
            lastBoardSize = bounds.size;
            lastScreenSize = screenSize;
        }
#endif

        if (!autoUpdate && Application.isPlaying)
            return;

        FitCamera();
    }

    private void FitCamera()
    {
        if (cam.orthographic)
        {
            FitOrthographic();
            return;
        }

        if (fitMode == FitMode.MoveCamera)
            FitByMovingCamera();
        else
            FitByAdjustingFOV();
    }

    private void FitByMovingCamera()
    {
        Bounds bounds = boardRenderer.bounds;
        float aspect = (float)Screen.width / Screen.height;

        float boardHeight = bounds.size.y * (1 + padding);
        float boardWidth = bounds.size.x * (1 + padding);

        float halfFovRad = cam.fieldOfView * 0.5f * Mathf.Deg2Rad;
        float distHeight = (boardHeight * 0.5f) / Mathf.Tan(halfFovRad);
        float fovWidth = 2f * Mathf.Atan(Mathf.Tan(halfFovRad) * aspect);
        float distWidth = (boardWidth * 0.5f) / Mathf.Tan(fovWidth * 0.5f);

        float dist = Mathf.Max(distHeight, distWidth);

        Vector3 boardCenter = bounds.center;
        Vector3 dir = (cam.transform.position - boardCenter).normalized;
        cam.transform.position = boardCenter + dir * dist;

        cam.transform.LookAt(boardCenter);
    }

    private void FitByAdjustingFOV()
    {
        Bounds bounds = boardRenderer.bounds;
        float aspect = (float)Screen.width / Screen.height;

        float boardHeight = bounds.size.y * (1 + padding);
        float boardWidth = bounds.size.x * (1 + padding);

        float dist = Vector3.Distance(cam.transform.position, bounds.center);

        float fovHeight = 2f * Mathf.Atan((boardHeight * 0.5f) / dist);
        float fovWidth = 2f * Mathf.Atan((boardWidth * 0.5f) / dist) / aspect;

        float requiredFOV = Mathf.Max(fovHeight, fovWidth) * Mathf.Rad2Deg;

        cam.fieldOfView = requiredFOV;
        cam.transform.LookAt(bounds.center);
    }

    private void FitOrthographic()
    {
        Bounds bounds = boardRenderer.bounds;
        float aspect = (float)Screen.width / Screen.height;

        float boardHeight = bounds.size.y * (1 + padding);
        float boardWidth = bounds.size.x * (1 + padding);

        cam.orthographicSize = Mathf.Max(boardHeight * 0.5f, boardWidth * 0.5f/aspect );
        cam.transform.LookAt(bounds.center);
    }
}

}
