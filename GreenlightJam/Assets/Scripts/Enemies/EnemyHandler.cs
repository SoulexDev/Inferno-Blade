using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyHandler : MonoBehaviour
{
    public List<Enemy> enemies = new List<Enemy>();
    public UnityEvent enemiesDefeatedEvent;

    private int enemyCount;

    private void Awake()
    {
        enemies.ForEach(e => e.handler = this);
        enemyCount = enemies.Count;

        if(RankMaster.Instance != null)
            RankMaster.Instance.enemyCountInLevel += enemyCount;
    }
    public void EnableEnemies()
    {
        enemies.ForEach(e => e.gameObject.SetActive(true));

        MusicManager.Instance.SwitchTrack(AudioManager.Instance.combatMusic);
    }
    public void RemoveEnemy(Enemy enemy)
    {
        enemies.Remove(enemy);
        Player.Instance.statsDisplay.killAmount++;

        if(enemies.Count <= 0)
        {
            TimeController.Instance.StartSlowTime();
            enemiesDefeatedEvent?.Invoke();

            MusicManager.Instance.SwitchTrack(AudioManager.Instance.calmMusic);

            if (RankMaster.Instance != null)
                RankMaster.Instance.SubtractEnemies(enemyCount);
        }
    }
}