using UnityEngine;

public class CameraController : MonoBehaviour
{
    private new Camera camera;
    private Vector3 target;

    public Vector3 position;
    public Vector3 offset;
    public float angle, radius;

    private float initialHeight;

    public bool dollyZoomEnabled;
    private bool dollyZoomEnabledBefore = false;

    private float FrustumHeightAtDistance(float distance)
    {
        return 2.0f * distance * Mathf.Tan(camera.fieldOfView * 0.5f * Mathf.Deg2Rad);
    }

    private float FOVForHeightAndDistance(float height, float distance)
    {
        return 2.0f * Mathf.Atan(height * 0.5f / distance) * Mathf.Rad2Deg;
    }

    private void SetTargetOnPlane()
    {
        target = GetTargetOnPlane();
    }

    public Vector3 GetTargetOnPlane()
    {
        Plane plane = new Plane(Vector3.up, Vector3.zero);
        Ray ray = camera.ViewportPointToRay(new Vector2(0.5f, 0.5f));
        if (plane.Raycast(ray, out float enter))
        {
            return ray.GetPoint(enter);
        }
        return Vector3.zero;
    }

    private void SetPosition()
    {
        Vector3 pos = new Vector3(position.x, position.y + radius * Mathf.Sin(angle * Mathf.Deg2Rad), position.z + -radius * Mathf.Cos(angle * Mathf.Deg2Rad));
        pos += offset;
        transform.SetPositionAndRotation(pos, Quaternion.Euler(angle, 0f, 0f));
    }

    private void Awake()
    {
        camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (dollyZoomEnabled && !dollyZoomEnabledBefore)
        {
            initialHeight = FrustumHeightAtDistance(Vector3.Distance(transform.position, target));
        }
        dollyZoomEnabledBefore = dollyZoomEnabled;

        SetTargetOnPlane();
        SetPosition();
        if (dollyZoomEnabled)
        {
            float currentDistance = Vector3.Distance(transform.position, target);
            camera.fieldOfView = FOVForHeightAndDistance(initialHeight, currentDistance);
        }
    }
}
