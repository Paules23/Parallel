using UnityEngine;
using System.Collections.Generic;

public class OptimizedPlatformController : MonoBehaviour
{
    public GameObject redRectangle;
    public List<GameObject> platformsAt0Degrees;
    public List<GameObject> platformsAt30Degrees;
    public List<GameObject> platformsAtMinus30Degrees;

    void Update()
    {
        float zRotation = redRectangle.transform.rotation.eulerAngles.z;

        // Normalize the rotation to handle negative angles correctly
        if (zRotation > 180)
        {
            zRotation -= 360;
        }

        if (Mathf.Approximately(zRotation, 0))
        {
            ActivatePlatforms(platformsAt0Degrees);
            DeactivatePlatforms(platformsAt30Degrees);
            DeactivatePlatforms(platformsAtMinus30Degrees);
        }
        else if (Mathf.Approximately(zRotation, 30))
        {
            DeactivatePlatforms(platformsAt0Degrees);
            ActivatePlatforms(platformsAt30Degrees);
            DeactivatePlatforms(platformsAtMinus30Degrees);
        }
        else if (Mathf.Approximately(zRotation, -30))
        {
            DeactivatePlatforms(platformsAt0Degrees);
            DeactivatePlatforms(platformsAt30Degrees);
            ActivatePlatforms(platformsAtMinus30Degrees);
        }
    }

    void ActivatePlatforms(List<GameObject> platforms)
    {
        foreach (GameObject platform in platforms)
        {
            platform.SetActive(true);
        }
    }

    void DeactivatePlatforms(List<GameObject> platforms)
    {
        foreach (GameObject platform in platforms)
        {
            platform.SetActive(false);
        }
    }
}
