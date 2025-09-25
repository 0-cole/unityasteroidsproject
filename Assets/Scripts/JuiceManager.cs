using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;

public class JuiceManager : MonoBehaviour
{
    [Header("Runtime References")]
    [SerializeField] private TextMeshProUGUI scoreText;

    [Header("Rainbow Settings")]
    [SerializeField] private Color[] colors;
    [SerializeField] private float animateInterval = 0.12f;
    [SerializeField] private bool animateContinuously = true;

    private string plainText = "";
    private int baseOffset = 0;
    private Coroutine animationCoroutine;

    void Awake()
    {
        if (scoreText == null)
        {
            Debug.LogWarning("JuiceManager: scoreText not assigned.");
            return;
        }

        // We only prepare the text object here.
        // We do NOT start the animation or read the default text.
        scoreText.richText = true;
        scoreText.color = Color.white;
        scoreText.faceColor = new Color32(255, 255, 255, 255);
    }

    public void SetPlainTextAndRefresh(string text)
    {
        if (scoreText == null) return;

        plainText = text ?? "";

        if (colors == null || colors.Length == 0)
        {
            scoreText.SetText(plainText);
            return;
        }

        string colored = BuildRainbow(baseOffset);
        scoreText.SetText(colored);

        if (animateContinuously && animateInterval > 0f)
            RestartWave();
        else if (colors.Length > 0)
            baseOffset = (baseOffset + 1) % colors.Length;
    }

    private string BuildRainbow(int offset)
    {
        var sb = new StringBuilder(plainText.Length * 24);
        for (int i = 0; i < plainText.Length; i++)
        {
            if (colors.Length == 0) continue;
            var c = colors[(offset + i) % colors.Length];
            string hex = ColorUtility.ToHtmlStringRGB(c);
            sb.Append($"<color=#{hex}>{plainText[i]}</color>");
        }
        return sb.ToString();
    }

    private IEnumerator WaveRoutine()
    {
        while (true)
        {
            string colored = BuildRainbow(baseOffset);
            scoreText.SetText(colored);
            
            if (colors.Length > 0)
                baseOffset = (baseOffset + 1) % colors.Length;

            yield return new WaitForSecondsRealtime(animateInterval);
        }
    }

    private void StartWave()
    {
        if (animationCoroutine == null)
            animationCoroutine = StartCoroutine(WaveRoutine());
    }

    private void RestartWave()
    {
        StopWaveIfRunning();
        StartWave();
    }

    private void StopWaveIfRunning()
    {
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
            animationCoroutine = null;
        }
    }

    void OnDisable()
    {
        StopWaveIfRunning();
    }
}
