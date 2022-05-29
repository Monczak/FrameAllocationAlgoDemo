using UnityEngine;

public class HUDMovement : MonoBehaviour
{
    private RectTransform canvasTransform;
    private RectTransform rectTransform;
    private CameraController cameraController;

    private void Awake()
    {
        canvasTransform = GetComponentInParent<RectTransform>();
        rectTransform = GetComponent<RectTransform>();
        cameraController = Camera.main.GetComponent<CameraController>();
    }

    // Update is called once per frame
    void Update()
    {
        rectTransform.anchoredPosition = new Vector2(0, -cameraController.offset.z * (canvasTransform.rect.height / CameraUtility.GetScreenHeightInUnits()));
    }
}
