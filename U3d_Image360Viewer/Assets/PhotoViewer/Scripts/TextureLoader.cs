using System;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace PhotoViewer.Scripts
{
    public class TextureLoader
    {
        public async void LoadData(string fileName, Action<Texture2D, string> onCompleted, Action onFailed)
        {
            string filePath = Path.Combine(Application.streamingAssetsPath, fileName);
            Debug.Log(filePath);

            UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(filePath);
            await uwr.SendWebRequest();

            if (uwr.isNetworkError || uwr.isHttpError)
                onFailed?.Invoke();
            else
                onCompleted?.Invoke(DownloadHandlerTexture.GetContent(uwr), fileName);
        }
    }
}