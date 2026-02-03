using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// WebGL-compatible stub for ShowInterstitialScript.
/// IronSource ads are not available in browser, so this just loads scenes directly.
/// </summary>
public class ShowInterstitialScript : MonoBehaviour
{
    public string sceneName;
    
    void Start()
    {
        Debug.Log("WebGL: Interstitial ads disabled - IronSource not supported in browser");
    }

    public void ShowInterstitialButtonClicked(string sceneName)
    {
        this.sceneName = sceneName;
        Debug.Log("WebGL: Skipping interstitial ad, loading scene directly: " + sceneName);
        SceneManager.LoadSceneAsync(sceneName);
    }

    void OnApplicationPause(bool isPaused)
    {
        // No-op for WebGL - IronSource not available
        Debug.Log("WebGL: OnApplicationPause = " + isPaused + " (no ads)");
    }
}
