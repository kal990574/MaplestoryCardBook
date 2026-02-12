using System;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class MapleStoryApi
{
    private readonly string _baseUrl;
    private readonly string _apiKey;

    public MapleStoryApi(ApiConfig config)
    {
        _baseUrl = config.BaseUrl;
        _apiKey = config.ApiKey;
    }

    public async UniTask<CharacterProfileResult> GetCharacterProfileAsync(string characterName)
    {
        var ocidResponse = await GetAsync<OcidResponse>(
            $"/id?character_name={UnityWebRequest.EscapeURL(characterName)}");
        var ocid = ocidResponse.Ocid;

        var (basic, stat, popularity, union) = await UniTask.WhenAll(
            TryGetAsync<CharacterBasicResponse>($"/character/basic?ocid={ocid}"),
            TryGetAsync<CharacterStatResponse>($"/character/stat?ocid={ocid}"),
            TryGetAsync<CharacterPopularityResponse>($"/character/popularity?ocid={ocid}"),
            TryGetAsync<UnionResponse>($"/user/union?ocid={ocid}")
        );

        return new CharacterProfileResult
        {
            Basic = basic,
            Stat = stat,
            Popularity = popularity,
            Union = union
        };
    }

    private async UniTask<T> GetAsync<T>(string path)
    {
        var url = _baseUrl + path;
        var request = UnityWebRequest.Get(url);
        request.SetRequestHeader("x-nxopen-api-key", _apiKey);

        await request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            var errorJson = request.downloadHandler.text;
            try
            {
                var apiError = JsonConvert.DeserializeObject<ApiErrorResponse>(errorJson);
                throw new Exception($"[{apiError.Error.Name}] {apiError.Error.Message}");
            }
            catch (JsonException)
            {
                throw new Exception($"HTTP {request.responseCode}: {request.error}");
            }
        }

        return JsonConvert.DeserializeObject<T>(request.downloadHandler.text);
    }

    private async UniTask<T> TryGetAsync<T>(string path) where T : class
    {
        try
        {
            return await GetAsync<T>(path);
        }
        catch (Exception e)
        {
            Debug.LogWarning($"API 호출 실패 ({path}): {e.Message}");
            return null;
        }
    }
}