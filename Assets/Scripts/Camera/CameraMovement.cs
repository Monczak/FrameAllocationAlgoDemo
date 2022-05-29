using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private CameraController controller;

    public bool panDown;
    public float panSpeed;
    private Vector3 panSmoothVelocity;

    private void Awake()
    {
        controller = GetComponent<CameraController>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        PanDown();
    }

    void PanDown()
    {
        controller.offset = Vector3.SmoothDamp(controller.offset, panDown ? Vector3.back * CameraUtility.GetScreenHeightInUnits() : Vector3.zero, ref panSmoothVelocity, panSpeed);
    }
}
