using Godot;
using System.Threading.Tasks;

public partial class TTSService : Node
{
    public static TTSService Instance;

    // At the top of TTSService
    private string BASE_URL =>
        ProjectSettings.HasSetting("tts/base_url")
            ? ProjectSettings.GetSetting("tts/base_url").AsString()
            : "http://localhost:3000";

    public override void _Ready()
    {
        Instance = this;

        DirAccess.MakeDirRecursiveAbsolute(
            "user://tts"
        );
    }

    public async Task PlayAudio(
        string language,
        string text
    )
    {
        HttpRequest request = new HttpRequest();

        AddChild(request);

        string url =
            $"{BASE_URL}/api/tts/generate";

        var body = new Godot.Collections.Dictionary
        {
            { "text", text },
            { "language", language }
        };

        string jsonBody =
            Json.Stringify(body);

        request.Request(
            url,
            new string[]
            {
                "Content-Type: application/json"
            },
            HttpClient.Method.Post,
            jsonBody
        );

        var result =
            await ToSignal(
                request,
                HttpRequest.SignalName.RequestCompleted
            );

        var responseBody =
            result[3].AsByteArray();

        string json =
            responseBody.GetStringFromUtf8();

        GD.Print($"TTS Response: {json}");

        var parsed =
            Json.ParseString(json)
            .AsGodotDictionary();

        string audioPath =
            parsed["audioPath"].AsString();

        string fileName =
            parsed["fileName"].AsString();

        string audioUrl =
            $"{BASE_URL}{audioPath}";

        string localPath =
            $"user://tts/{fileName}";

        GD.Print($"Audio URL: {audioUrl}");
        GD.Print($"Local Path: {localPath}");

        // LOCAL CACHE CHECK
        if (!FileAccess.FileExists(localPath))
        {
            GD.Print(
                $"Downloading audio: {fileName}"
            );

            localPath =
                await DownloadAudio(
                    audioUrl,
                    fileName
                );
        }
        else
        {
            GD.Print(
                $"Using cached audio: {fileName}"
            );
        }

        AudioManager.Instance.PlayWav(
            localPath
        );

        request.QueueFree();
    }

    private async Task<string> DownloadAudio(
        string url,
        string fileName
    )
    {
        HttpRequest request = new HttpRequest();

        AddChild(request);

        string savePath =
            $"user://tts/{fileName}";

        request.DownloadFile = savePath;

        request.Request(url);

        var result = await ToSignal(
            request,
            HttpRequest.SignalName.RequestCompleted
        );

        long responseCode = (long)result[1];

        if (responseCode != 200)
        {
            GD.PrintErr(
                $"Failed to download audio. HTTP {responseCode}"
            );
        }

        GD.Print($"HTTP Response Code: {responseCode}");

        request.QueueFree();

        return savePath;
    }
}