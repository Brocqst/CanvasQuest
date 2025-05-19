using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowBoat : MonoBehaviour
{
    public RectTransform uiElement; // Assign your UI element here
    public float smoothSpeed = 0.125f;
    public Vector3 offset; // Adjust as needed

    void LateUpdate()
    {
        // Convert UI position to world position (for Screen Space - Overlay)
        Vector3 targetPosition = Camera.main.ScreenToWorldPoint(new Vector3(uiElement.position.x, uiElement.position.y, Camera.main.nearClipPlane));

        // Apply offset if needed
        targetPosition += offset;

        // Smoothly interpolate camera position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
