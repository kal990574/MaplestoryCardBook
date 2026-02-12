using Newtonsoft.Json;

public class CharacterPopularityResponse
{
    [JsonProperty("date")]
    public string Date;

    [JsonProperty("popularity")]
    public long Popularity;
}