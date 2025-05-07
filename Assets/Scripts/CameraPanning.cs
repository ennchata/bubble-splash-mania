using UnityEngine;

public class CameraPanning : MonoBehaviour {
    public float MaxPanAngle = 5f;
    public float PanSpeed = 5f;

    private Quaternion originalRotation;

    void Start() {
        originalRotation = transform.rotation;
    }

    void Update() {
        Vector2 mousePosition = Input.mousePosition;
        Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Vector2 normalizedMouseOffset = new Vector2(
            (mousePosition.x - screenCenter.x) / screenCenter.x,
            (mousePosition.y - screenCenter.y) / screenCenter.y
        );

        normalizedMouseOffset = Vector2.ClampMagnitude(normalizedMouseOffset, 1f);

        float targetYaw = normalizedMouseOffset.x * MaxPanAngle;    // Horizontal
        float targetPitch = -normalizedMouseOffset.y * MaxPanAngle; // Vertical (inverted Y)

        Quaternion targetRotation = Quaternion.Euler(targetPitch, targetYaw, 0f);
        Quaternion finalRotation = Quaternion.Lerp(transform.rotation, originalRotation * targetRotation, Time.deltaTime * PanSpeed);

        transform.rotation = finalRotation;
    }
}
