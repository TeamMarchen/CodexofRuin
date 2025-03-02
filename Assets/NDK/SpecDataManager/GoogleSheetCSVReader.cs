using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public static class GoogleSheetCSVReader
{
    public static async Task<string> DownloadCSV(string url)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            // 요청 보내기
            var operation = webRequest.SendWebRequest();

            // 완료될 때까지 대기
            while (!operation.isDone)
            {
                await Task.Yield();
            }

            // 오류 처리
            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(webRequest.error);
                return null;
            }

            // 응답 처리
            return webRequest.downloadHandler.text;
        }
    }
}