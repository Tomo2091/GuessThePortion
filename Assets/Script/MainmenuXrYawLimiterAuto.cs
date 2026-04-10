using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class MainmenuXrYawLimiterAuto
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Bootstrap()
    {
        var existing = Object.FindFirstObjectByType<Runner>();
        if (existing != null)
            return;

        var runnerObject = new GameObject("MainmenuXrYawLimiterAutoRunner");
        Object.DontDestroyOnLoad(runnerObject);
        runnerObject.hideFlags = HideFlags.HideAndDontSave;
        runnerObject.AddComponent<Runner>();
    }

    private sealed class Runner : MonoBehaviour
    {
        private const string MainmenuSceneName = "Mainmenu";
        private const float MaxYaw = 90f; // Half-turn total range: 180 degrees.

        private XROrigin xrOrigin;
        private float baseYaw;
        private bool baseYawInitialized;

        private void LateUpdate()
        {
            if (SceneManager.GetActiveScene().name != MainmenuSceneName)
            {
                baseYawInitialized = false;
                return;
            }

            if (xrOrigin == null)
                xrOrigin = Object.FindFirstObjectByType<XROrigin>();
            if (xrOrigin == null)
                return;

            if (!baseYawInitialized)
            {
                baseYaw = xrOrigin.transform.eulerAngles.y;
                baseYawInitialized = true;
            }

            var currentDelta = Mathf.DeltaAngle(baseYaw, xrOrigin.transform.eulerAngles.y);
            var clampedDelta = Mathf.Clamp(currentDelta, -MaxYaw, MaxYaw);
            xrOrigin.transform.rotation = Quaternion.Euler(0f, baseYaw + clampedDelta, 0f);
        }
    }
}
