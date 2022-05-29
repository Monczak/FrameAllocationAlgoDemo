using UnityEngine;

class CameraUtility
{
    private static Camera _camera;

    public static Camera Camera
    {
        get
        {
            if (_camera == null)
                _camera = Camera.main;
            return _camera;
        }
    }

    public static float GetScreenHeightInUnits()
    {
        Plane plane = new Plane(Vector3.up, Vector3.zero);

        Ray topRay = Camera.ViewportPointToRay(new Vector2(0.5f, 1));
        plane.Raycast(topRay, out float enter);
        Vector3 topPoint = topRay.GetPoint(enter);

        Ray bottomRay = Camera.ViewportPointToRay(new Vector2(0.5f, 0));
        plane.Raycast(bottomRay, out enter);
        Vector3 bottomPoint = bottomRay.GetPoint(enter);

        return (topPoint - bottomPoint).magnitude;
    }
}
