using System.Collections.Generic;
using Newtonsoft.Json;

public class CharacterStatResponse
{
    [JsonProperty("date")]
    public string Date;

    [JsonProperty("character_class")]
    public string CharacterClass;

    [JsonProperty("final_stat")]
    public List<FinalStat> FinalStatList;
    
    [JsonProperty("remain_ap")]
    public int RemainAp;
}

public class FinalStat
{
    [JsonProperty("stat_name")]
    public string StatName;

    [JsonProperty("stat_value")]
    public string StatValue;
}