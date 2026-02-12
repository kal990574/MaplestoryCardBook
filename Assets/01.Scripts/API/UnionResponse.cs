using Newtonsoft.Json;

public class UnionResponse
{
    [JsonProperty("date")]
    public string Date;

    [JsonProperty("union_level")]
    public int UnionLevel;

    [JsonProperty("union_grade")]
    public string UnionGrade;

    [JsonProperty("union_artifact_level")]
    public int UnionArtifactLevel;
}