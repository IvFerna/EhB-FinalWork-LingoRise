using Godot;
using System.Threading.Tasks;

public partial class TTSService : Node
{
    public static TTSService Instance;

    private HttpRequest _activeRequest;
    private AudioStreamPlayer _activePlayer;

    // Incremented every time playback is interrupted/restarted
    private int _playbackToken = 0;

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
        // Interrupt any existing playback/request
        Stop();

        _playbackToken++;
        int myToken = _playbackToken;

        HttpRequest request = new HttpRequest();

        _activeRequest = request;

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

        Error err = request.Request(
            url,
            new string[]
            {
                "Content-Type: application/json"
            },
            HttpClient.Method.Post,
            jsonBody
        );

        if (err != Error.Ok)
        {
            GD.PrintErr(
                $"Failed to start TTS request: {err}"
            );

            CleanupRequest(request);

            return;
        }

        var result =
            await ToSignal(
                request,
                HttpRequest.SignalName.RequestCompleted
            );

        // Request invalidated/interrupted
        if (
            myToken != _playbackToken ||
            !IsInstanceValid(request)
        )
        {
            CleanupRequest(request);
            return;
        }

        long responseCode = (long)result[1];

        if (responseCode != 200)
        {
            GD.PrintErr(
                $"TTS generation failed. HTTP {responseCode}"
            );

            CleanupRequest(request);

            return;
        }

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

        // Download if not cached
        if (!FileAccess.FileExists(localPath))
        {
            GD.Print(
                $"Downloading audio: {fileName}"
            );

            localPath =
                await DownloadAudio(
                    audioUrl,
                    fileName,
                    myToken
                );

            // Interrupted during download
            if (
                myToken != _playbackToken
            )
            {
                return;
            }
        }
        else
        {
            GD.Print(
                $"Using cached audio: {fileName}"
            );
        }

        if (
            string.IsNullOrEmpty(localPath) ||
            !FileAccess.FileExists(localPath)
        )
        {
            GD.PrintErr(
                $"TTS file missing: {localPath}"
            );

            return;
        }

        CleanupRequest(request);

        _activePlayer =
            AudioManager.Instance.PlayWav(
                localPath
            );

        if (_activePlayer == null)
        {
            GD.PrintErr(
                "Failed to create audio player"
            );

            return;
        }

        await ToSignal(
            _activePlayer,
            AudioStreamPlayer.SignalName.Finished
        );

        // Interrupted while audio was playing
        if (
            myToken != _playbackToken
        )
        {
            return;
        }

        CleanupPlayer();
    }

    private async Task<string> DownloadAudio(
        string url,
        string fileName,
        int token
    )
    {
        HttpRequest request = new HttpRequest();

        _activeRequest = request;

        AddChild(request);

        string savePath =
            $"user://tts/{fileName}";

        request.DownloadFile = savePath;

        Error err = request.Request(url);

        if (err != Error.Ok)
        {
            GD.PrintErr(
                $"Failed to start download: {err}"
            );

            CleanupRequest(request);

            return null;
        }

        var result = await ToSignal(
            request,
            HttpRequest.SignalName.RequestCompleted
        );

        if (
            token != _playbackToken ||
            !IsInstanceValid(request)
        )
        {
            CleanupRequest(request);
            return null;
        }

        long responseCode = (long)result[1];

        if (responseCode != 200)
        {
            GD.PrintErr(
                $"Failed to download audio. HTTP {responseCode}"
            );

            CleanupRequest(request);

            return null;
        }

        GD.Print(
            $"Downloaded audio: {fileName}"
        );

        CleanupRequest(request);

        return savePath;
    }

    public void Stop()
    {
        // Invalidate ALL existing async operations
        _playbackToken++;

        if (_activeRequest != null)
        {
            if (IsInstanceValid(_activeRequest))
            {
                _activeRequest.CancelRequest();
            }

            _activeRequest = null;
        }

        if (_activePlayer != null)
        {
            if (IsInstanceValid(_activePlayer))
            {
                _activePlayer.Stop();
                _activePlayer.QueueFree();
            }

            _activePlayer = null;
        }
    }

    private void CleanupRequest(
        HttpRequest request
    )
    {
        if (
            request != null &&
            IsInstanceValid(request)
        )
        {
            request.QueueFree();
        }

        if (_activeRequest == request)
        {
            _activeRequest = null;
        }
    }

    private void CleanupPlayer()
    {
        if (_activePlayer != null)
        {
            if (IsInstanceValid(_activePlayer))
            {
                _activePlayer.QueueFree();
            }

            _activePlayer = null;
        }
    }
}