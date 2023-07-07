using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RankDisplayer : MonoBehaviour
{
    [SerializeField] private List<LevelDisplay> levelDisplays = new List<LevelDisplay>();

    private void Awake()
    {
        DisplayData();
    }
    void DisplayData()
    {
        for (int i = 0; i < levelDisplays.Count; i++)
        {
            LevelData data = RankMaster.Instance.levelData.First(l=>l.sceneID == levelDisplays[i].sceneID);

            levelDisplays[i].rankImg.sprite = RankMaster.Instance.ranks.First(r=>r.rankType == data.achievedRank).rankImg;
        }
    }
}