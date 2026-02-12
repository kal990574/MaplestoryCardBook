using Newtonsoft.Json;

public class ApiErrorResponse
{
    [JsonProperty("error")]
    public ApiErrorDetail Error;
}

public class ApiErrorDetail
{
    [JsonProperty("name")]
    public string Name;

    [JsonProperty("message")]
    public string Message;
}