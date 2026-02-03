using UnityEngine;

/// <summary>
/// WebGL-compatible stub for ShowRewardedVideoScript.
/// IronSource ads are not available in browser, so this gives free coins instead.
/// </summary>
public class ShowRewardedVideoScript : MonoBehaviour
{
    private AuthenticationManager manager;
    
    void Start()
    {
        Debug.Log("WebGL: Rewarded video ads disabled - IronSource not supported in browser");
        manager = AuthenticationManager.Instance;
    }

    public void ShowRewardedVideoButtonClicked()
    {
        Debug.Log("WebGL: Giving free coins - rewarded video not available in browser");
        
        if (manager != null)
        {
            manager.ShowAlert("Free Coins!", "You received 200 coins!", "CointHit");
            manager.AddBonus(200);
            manager.ShowCoinCollectionAnim();
        }
        else
        {
            Debug.LogWarning("WebGL: AuthenticationManager not available for coin reward");
        }
    }

    void OnApplicationPause(bool isPaused)
    {
        // No-op for WebGL - IronSource not available
        Debug.Log("WebGL: OnApplicationPause = " + isPaused + " (no ads)");
    }
}
