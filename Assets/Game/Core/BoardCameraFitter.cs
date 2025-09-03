namespace Game.Core
{
using UnityEngine;

[ExecuteAlways] // works in edit mode
public class BoardCameraFitter : MonoBehaviour
{
    public enum FitMode { MoveCamera, AdjustFOV }

    [Header("References")]
    public Camera cam;
    public Renderer boardRenderer;

    [Header("Settings")]
    [Range(0f, 1f)] public float padding = 0.05f; 
    public FitMode fitMode = FitMode.MoveCamera;

    private void LateUpdate()
    {
        if (cam == null || boardRenderer == null) return;

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

        float fov = cam.fieldOfView * Mathf.Deg2Rad;

        float distHeight = (boardHeight * 0.5f) / Mathf.Tan(fov * 0.5f);
        float fovWidth = 2f * Mathf.Atan(Mathf.Tan(fov * 0.5f) * aspect);
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

        // Distance to board center (don’t change camera pos)
        float dist = Vector3.Distance(cam.transform.position, bounds.center);

        // Required vertical FOV to fit height
        float fovHeight = 2f * Mathf.Atan((boardHeight * 0.5f) / dist);

        // Required horizontal FOV to fit width
        float fovWidth = 2f * Mathf.Atan((boardWidth * 0.5f) / dist) / aspect;

        // Use whichever is larger (ensures full fit)
        float requiredFOV = Mathf.Max(fovHeight, fovWidth) * Mathf.Rad2Deg;

        cam.fieldOfView = requiredFOV;

        cam.transform.LookAt(bounds.center);
    }
}
}