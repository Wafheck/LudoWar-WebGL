using UnityEngine;

/// <summary>
/// WebGL-compatible stub for ShowBannerAds.
/// IronSource ads are not available in browser, so this is a no-op.
/// </summary>
public class ShowBannerAds : MonoBehaviour
{
    public static string uniqueUserId = "demoUserUnity";
    
    void Start()
    {
        Debug.Log("WebGL: Banner ads disabled - IronSource not supported in browser");
    }
    
    void Update()
    {
        // No-op
    }

    void OnApplicationPause(bool isPaused)
    {
        // No-op for WebGL - IronSource not available
        Debug.Log("WebGL: OnApplicationPause = " + isPaused + " (no ads)");
    }
}
