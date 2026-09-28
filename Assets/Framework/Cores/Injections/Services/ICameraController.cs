using UnityEngine;

namespace Dada.Cores;

public interface ICameraController
{
    Camera Camera { get; }
    float Zoom { get; set; }
    float Yaw { get; set; }

    void Follow(Transform target);
    void StopFollow();
    void MoveTo(Vector3 worldPosition);
    void SetBounds(Bounds bounds);
    void ClearBounds();
}
