using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    public TextMeshProUGUI scoreText;
    private int currentScore = 0;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        UpdateScoreUI();
    }
    
    public void AddScore(int points)
    {
        currentScore += points;
        UpdateScoreUI();
    }
    
    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Очки: " + currentScore;
        }
    }
    
    public int GetCurrentScore()
    {
        return currentScore;
    }
}