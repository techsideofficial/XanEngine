using Epic.OnlineServices.AntiCheatClient;
using Godot;
using System;

public partial class eactest : Node
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

        GD.Print(AntiCheatClientInterface.BeginsessionApiLatest);
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
