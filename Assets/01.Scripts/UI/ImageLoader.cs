using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public static class ImageLoader
{
    public static async UniTask<Texture2D> LoadTextureAsync(string url)
    {
        var request = UnityWebRequestTexture.GetTexture(url);
        await request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogWarning($"이미지 로드 실패: {request.error}");
            return null;
        }

        var texture = DownloadHandlerTexture.GetContent(request);
        texture.filterMode = FilterMode.Bilinear;
        return texture;
    }
}