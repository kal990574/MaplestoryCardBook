using Newtonsoft.Json;

public class OcidResponse
{
    [JsonProperty("ocid")]
    public string Ocid;
}

public class CharacterBasicResponse
{
    [JsonProperty("character_name")]
    public string CharacterName;

    [JsonProperty("world_name")]
    public string WorldName;

    [JsonProperty("character_gender")]
    public string CharacterGender;

    [JsonProperty("character_class")]
    public string CharacterClass;

    [JsonProperty("character_class_level")]
    public string CharacterClassLevel;

    [JsonProperty("character_level")]
    public int CharacterLevel;

    [JsonProperty("character_exp")]
    public long CharacterExp;

    [JsonProperty("character_exp_rate")]
    public string CharacterExpRate;

    [JsonProperty("character_guild_name")]
    public string CharacterGuildName;

    [JsonProperty("character_image")]
    public string CharacterImage;

    [JsonProperty("character_date_create")]
    public string CharacterDateCreate;

    [JsonProperty("access_flag")]
    public string AccessFlag;

    [JsonProperty("liberation_quest_clear_flag")]
    public string LiberationQuestClearFlag;
}