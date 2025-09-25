using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    [Header("UI, optional fallback if JuiceManager is missing")]
    [SerializeField] private TextMeshProUGUI scoreText; // optional fallback, can be left null
    [Header("Runtime")]
    [SerializeField] private JuiceManager juiceManager; // assign your JuiceManager here

    private int score = 0;

    void Start()
    {
        // Try to auto-find JuiceManager if not assigned in Inspector
        if (juiceManager == null)
        {
            juiceManager = FindFirstObjectByType<JuiceManager>();
            if (juiceManager != null)
                Debug.Log("Score: Auto-found JuiceManager.");
        }
        
        if (juiceManager == null && scoreText == null)
            Debug.LogWarning("Score: no output assigned, assign either JuiceManager or scoreText.");

        // initialize display
        IncreaseScore(0);
    }

    public int IncreaseScore(int increaseScore)
    {
        score += increaseScore;
        string text = "Score: " + score.ToString();

        // primary path, let JuiceManager own the TMP and write colored text
        if (juiceManager != null)
        {
            juiceManager.SetPlainTextAndRefresh(text);
        }
        else if (scoreText != null) // fallback if JuiceManager not set
        {
            scoreText.SetText(text);
            scoreText.ForceMeshUpdate();
        }

        Debug.Log("Score now " + score);
        return score;
    }
}
