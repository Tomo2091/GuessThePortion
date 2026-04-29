using System.Collections;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class NewGameSpawnFix
{
    private const string SelectDishSceneName = "Mainmenu";
    private static readonly Vector3 SpawnPosition = new Vector3(0f, 2.62f, 0.1f);
    private static readonly Quaternion SpawnRotation = Quaternion.Euler(12.534f, 90f, 0f);
    private static readonly Vector3 LeftHandLocalPosition = new Vector3(-0.18f, -0.35f, 0.2f);
    private static readonly Vector3 RightHandLocalPosition = new Vector3(0.18f, -0.35f, 0.2f);

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void ApplySpawnFixAfterSceneLoad()
    {
        var existing = Object.FindFirstObjectByType<MainMenuFixRunner>();
        if (existing != null)
            return;

        var runnerObject = new GameObject("MainMenuXrSpawnFixRunner");
        Object.DontDestroyOnLoad(runnerObject);
        runnerObject.hideFlags = HideFlags.HideAndDontSave;
        runnerObject.AddComponent<MainMenuFixRunner>();
    }

    private sealed class MainMenuFixRunner : MonoBehaviour
    {
        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            TryRunForActiveScene();
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == SelectDishSceneName)
                StartCoroutine(ApplySpawnFix());
        }

        private void TryRunForActiveScene()
        {
            if (SceneManager.GetActiveScene().name == SelectDishSceneName)
                StartCoroutine(ApplySpawnFix());
        }

        private IEnumerator ApplySpawnFix()
        {
            // Wait for XR systems and simulator initialization before forcing spawn.
            yield return null;
            yield return null;

            var xrOrigin = Object.FindFirstObjectByType<XROrigin>();
            if (xrOrigin != null)
            {
                xrOrigin.transform.SetPositionAndRotation(SpawnPosition, SpawnRotation);
                xrOrigin.MatchOriginUpCameraForward(Vector3.up, SpawnRotation * Vector3.forward);
                xrOrigin.MoveCameraToWorldLocation(SpawnPosition);
                MoveHandsCloserToBody(xrOrigin.transform);
            }
        }

        private static void MoveHandsCloserToBody(Transform root)
        {
            var leftHand = FindChildRecursive(root, "Left Hand");
            if (leftHand != null)
                leftHand.localPosition = LeftHandLocalPosition;

            var rightHand = FindChildRecursive(root, "Right Hand");
            if (rightHand != null)
                rightHand.localPosition = RightHandLocalPosition;
        }

        private static Transform FindChildRecursive(Transform parent, string targetName)
        {
            if (parent.name == targetName)
                return parent;

            for (var i = 0; i < parent.childCount; i++)
            {
                var child = parent.GetChild(i);
                var result = FindChildRecursive(child, targetName);
                if (result != null)
                    return result;
            }

            return null;
        }
    }
}
