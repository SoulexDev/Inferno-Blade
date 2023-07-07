using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class LevelData
{
    public RankRequirement platRankRequirement;
    public RankRequirement goldRankRequirement;
    public RankRequirement ironRankRequirement;

    public RankType achievedRank;
    public int sceneID;
}

[System.Serializable]
public class RankRequirement
{
    //public RankType rankType;

    public float timeRequired;
}