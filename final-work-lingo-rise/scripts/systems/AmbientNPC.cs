using Godot;

[GlobalClass]
public partial class AmbientNPC : Resource
{
	[Export] public DialogueData Dialogue;
	[Export] public Texture2D NpcTexture { get; set; }
	[Export] public float SpeakInterval = 8f;
}
