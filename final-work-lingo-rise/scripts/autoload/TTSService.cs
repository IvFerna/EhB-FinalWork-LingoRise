using Godot;
using System.Threading.Tasks;

public partial class TTSService : Node
{
    public static TTSService Instance;

    private const string BASE_URL =
        "http://localhost:3000";

    public override void _Ready()
    {
        Instance = this;
        DirAccess.MakeDirRecursiveAbsolute("user://tts");
    }

    public async Task PlayWord(string language, string word)
    {
        HttpRequest request = new HttpRequest();

        AddChild(request);

        string url =
            $"{BASE_URL}/api/tts/generate";

        var body = new Godot.Collections.Dictionary
        {
            { "text", word },
            { "language", language }
        };

        string jsonBody = Json.Stringify(body);

        request.Request(
            url,
            new string[] { "Content-Type: application/json" },
            HttpClient.Method.Post,
            jsonBody
        );

        var result =
            await ToSignal(
                request,
                HttpRequest.SignalName.RequestCompleted
            );

        var responseBody = result[3].AsByteArray();

        string json =
            responseBody.GetStringFromUtf8();

        GD.Print(json);

        var parsed = Json.ParseString(json).AsGodotDictionary();

        string audioPath = parsed["audioPath"].AsString();
        GD.Print($"Audio path: {audioPath}");

        string audioUrl = $"{BASE_URL}{audioPath}";
        GD.Print($"Audio URL: {audioUrl}");


        string fileName = $"{language}_{word}.wav";

        string localPath =
            $"user://tts/{fileName}";

        if (!FileAccess.FileExists(localPath))
        {
            localPath = await DownloadAudio(
                audioUrl,
                fileName
            );

            GD.Print($"Downloaded audio: {localPath}");
        }
        else
        {
            GD.Print($"Using cached audio: {localPath}");
        }

        AudioManager.Instance.PlayWav(localPath);


        AudioManager.Instance.PlayWav(localPath);

        request.QueueFree();
    }

    private async Task<string> DownloadAudio(string url, string fileName)
    {
        HttpRequest request = new HttpRequest();

        AddChild(request);

        string savePath =
            $"user://tts/{fileName}";

        request.DownloadFile = savePath;

        request.Request(url);

        await ToSignal(
            request,
            HttpRequest.SignalName.RequestCompleted
        );

        request.QueueFree();

        return savePath;
    }
}
