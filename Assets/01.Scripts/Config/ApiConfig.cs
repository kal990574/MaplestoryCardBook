using UnityEngine;

[CreateAssetMenu(fileName = "ApiConfig", menuName = "Config/Api Config")]
public class ApiConfig : ScriptableObject
{
    [Header("NEXON Open API")]
    [SerializeField] private string _apiKey;
    [SerializeField] private string _baseUrl = "https://open.api.nexon.com/maplestory/v1";

    public string ApiKey => _apiKey;
    public string BaseUrl => _baseUrl;
}