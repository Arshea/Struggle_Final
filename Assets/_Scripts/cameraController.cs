using UnityEngine;
using System.Collections;

public class cameraController : MonoBehaviour {

    Vector2 mouseMove, smoothVector;
    public float sensitivity = 5.0F;
    public float smooth = 2.0F;
    public float clampLow = -45.0F, clampHigh = 60.0F;
    public Light map_light;
	public GameObject lantern;
    GameObject character;

	// Use this for initialization
	void Start () {
        character = this.transform.parent.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
        var deltaMouse = new Vector2(Input.GetAxisRaw("Mouse X") + Input.GetAxisRaw("Right Thumb X"), Input.GetAxisRaw("Mouse Y") + Input.GetAxisRaw("Right Thumb Y"));

        deltaMouse = Vector2.Scale(deltaMouse, new Vector2(sensitivity * smooth, sensitivity * smooth));
        smoothVector.x = Mathf.Lerp(smoothVector.x, deltaMouse.x, 1F / smooth);
        smoothVector.y = Mathf.Lerp(smoothVector.y, deltaMouse.y, 1F / smooth);
        mouseMove += smoothVector;
        // Restrict up and down camera control
        mouseMove.y = Mathf.Clamp(mouseMove.y, clampLow, clampHigh);

        transform.localRotation = Quaternion.AngleAxis(-mouseMove.y, Vector3.right);
		lantern.transform.localRotation = Quaternion.AngleAxis(-mouseMove.y, Vector3.right);
        character.transform.localRotation = Quaternion.AngleAxis(mouseMove.x, character.transform.up);

	}

    void OnPreCull () {
        if (map_light != null)
            map_light.enabled = false;
    }
     
    void OnPreRender() {
        if (map_light != null)
            map_light.enabled = false;
    }

    void OnPostRender() {
        if (map_light != null)
            map_light.enabled = true;
    }
}
