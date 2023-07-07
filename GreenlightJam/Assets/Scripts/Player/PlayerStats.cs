using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour, IPlayer
{
    private float _health = 100;
    [SerializeField] private float health { get { return _health; } set { _health = value; UpdateHealth(); } }
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private Image healthBar;
    [SerializeField] private TextMeshProUGUI healthText;

    private void Update()
    {
        if (health < 100)
        {
            health += Time.deltaTime * 2;
        }
        else
            health = 100;
    }
    public void Damage(float amount)
    {
        health -= amount;

        if(health <= 0)
        {
            Die();
        }
    }
    void UpdateHealth()
    {
        healthBar.fillAmount = health / maxHealth;
        healthText.text = health.ToString("f0") + "/" + maxHealth.ToString("f0");
    }
    public void ResetStats()
    {
        health = 100;
        healthBar.fillAmount = 1;
        healthText.text = "100/100";
    }
    void Die()
    {
        Player.Instance.ResetPlayer();
        Player.Instance.dead = true;
    }
}