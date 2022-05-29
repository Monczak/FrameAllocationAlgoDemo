using UnityEngine;
using UnityEngine.UI;

public class ActivityMarker : MonoBehaviour
{
    private Image image;
    public bool active;

    private float alpha;
    public float alphaSmoothing = 20;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Color color = image.color;
        image.color = new Color(color.r, color.g, color.b, alpha);

        alpha = Mathf.Lerp(alpha, active ? 1 : 0, alphaSmoothing * Time.deltaTime);
    }
}
