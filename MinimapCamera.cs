using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    [SerializeField]
    private Vector3 cameraOffset;
    [SerializeField]
    private GameObject radarPulse;
    [SerializeField]
    private Vector3 radarPulseOffset;

    private RadarPulse rp;
    private Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        rp = radarPulse.GetComponent<RadarPulse>();
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        radarPulse.transform.position = PlayerManager.CurrentPlayer.transform.position + radarPulseOffset;
        gameObject.transform.position = PlayerManager.CurrentPlayer.transform.position + cameraOffset;
        cam.orthographicSize = rp.rangeMax * 0.65f;
    }
}
