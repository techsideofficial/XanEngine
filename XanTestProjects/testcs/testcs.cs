using Godot;
using System;

public partial class testcs : Node
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Button testButton = GetNode<Button>("/root/Control/VBoxContainer/Button");
		testButton.Pressed += () => _on_Button_pressed();
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        ProgressBar bar = GetNode<ProgressBar>("/root/Control/VBoxContainer/ProgressBar");
        if (GetNode<CheckButton>("/root/Control/VBoxContainer/CheckButton").ButtonPressed == !true)
		{
			bar.MinValue = 0;
			bar.MaxValue = 100;
			bar.Value = GetNode<HSlider>("/root/Control/VBoxContainer/HSlider").Value;
		}
		else
		{
			bar.MaxValue = 0;
			bar.MinValue = -100;
            bar.Value = -GetNode<HSlider>("/root/Control/VBoxContainer/HSlider").Value;
        }
    }

	public void _on_Button_pressed()
	{
		GetNode<Label>("/root/Control/VBoxContainer/Label").Text = GetNode<LineEdit>("/root/Control/VBoxContainer/LineEdit").Text;
	}
}
