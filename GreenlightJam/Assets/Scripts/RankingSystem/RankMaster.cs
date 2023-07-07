using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RankMaster : MonoBehaviour
{
    public List<Rank> ranks = new List<Rank>();

    public List<LevelData> levelData = new List<LevelData>();

    public static RankMaster Instance;
    [HideInInspector] public int enemyCountInLevel;

    private float time;
    private int enemiesKilled;

    private bool updateStats;

    private LevelData currentLevel;

    private float milliseconds;
    private float seconds;
    private float minutes;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        if (SceneManager.GetActiveScene().buildIndex == 0)
            return;

        StartLevel();
    }
    public void StartLevel()
    {
        updateStats = true;
        currentLevel = levelData.First(l => l.sceneID == SceneManager.GetActiveScene().buildIndex);
    }
    private void Update()
    {
        if (!updateStats)
            return;

        time += Time.deltaTime;

        //milliseconds = time/100;
        //seconds = time;
        //minutes = time/60;

        //if (milliseconds >= 1)
        //    milliseconds = 0;

        //if (seconds >= 60)
        //    seconds = 0;

        Player.Instance.statsDisplay.timeDisplay.text = "Time: " + time.ToString("f1");
    }
    public void EndLevel()
    {
        updateStats = false;
        float enemyPercentage = ((float)enemiesKilled / enemyCountInLevel) * 100;

        int rankIndex = 0;

        if (time < currentLevel.platRankRequirement.timeRequired)
            rankIndex = 3;
        else if (time < currentLevel.goldRankRequirement.timeRequired)
            rankIndex = 2;
        else if (time < currentLevel.ironRankRequirement.timeRequired)
            rankIndex = 1;

        print(enemyPercentage);

        switch (enemyPercentage)
        {
            case >= 100:
                break;

            case >= 75:
                rankIndex--;
                break;

            case >= 50:
                rankIndex -= 2;
                break;

            case >= 25:
                rankIndex -= 3;
                break;

            case >= 0:
                rankIndex -= 4;
                break;

            default:
                break;
        }
        if (rankIndex <= 0)
            rankIndex = 0;

        if (currentLevel.achievedRank > (RankType)rankIndex)
            return;

        currentLevel.achievedRank = (RankType)rankIndex;

        time = 0;
        Player.Instance.statsDisplay.killAmount = 0;
    }
    public void ExitScene()
    {
        updateStats = false;
        time = 0;
        if(Player.Instance != null)
            Player.Instance.statsDisplay.killAmount = 0;
    }
    public void SubtractEnemies(int amount)
    {
        enemiesKilled += amount;
    }
}
public enum RankType { Unranked = 0, Iron = 1, Gold = 2, Platinum = 3 }

[System.Serializable]
public class Rank
{
    [Header("Rank Info")]
    public RankType rankType;
    public Sprite rankImg;
}