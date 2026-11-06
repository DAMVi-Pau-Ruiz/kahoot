using System;
using System.Collections.Generic;
using System.Xml.Serialization;

[Serializable]
[XmlRoot("ranking")]
public class RankingData
{
    [XmlArray("players")]
    [XmlArrayItem("player")]
    public List<RankingPlayerData> players =
        new List<RankingPlayerData>();
}

[Serializable]
public class RankingPlayerData
{
    public string username;
    public int score;
}