using System.Collections;
using UnityEngine;
using System.Threading;
using TMPro;

public class FrameRateController : MonoBehaviour
{
    private int maxRate = 9999;
    private float targetFrameRate = 60f;
    float currentFrameTime;

    [SerializeField]
    private TMP_Text fpsText;

    void Awake()
    {
        QualitySettings.vSyncCount = 0;  // Disable VSync
        Application.targetFrameRate = maxRate;
        currentFrameTime = Time.realtimeSinceStartup;
        StartCoroutine(WaitForNextFrame());
    }

    private void Update()
    {
        fpsText.text = $"FPS: {(1.0f / Time.unscaledDeltaTime):0}";
    }

    private IEnumerator WaitForNextFrame()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            currentFrameTime += 1.0f / targetFrameRate;
            float t = Time.realtimeSinceStartup;
            float sleepTime = currentFrameTime - Time.realtimeSinceStartup - 0.01f;
            if (sleepTime > 0)
                Thread.Sleep((int)(sleepTime * 1000f));
            while (t < currentFrameTime)
            {
                t = Time.realtimeSinceStartup;
            }
        }
    }
}
