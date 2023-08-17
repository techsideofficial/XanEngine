extends Node

var bank_path: String = "res://audio/banks/Build/Desktop"

func _ready():
	RuntimeManager.play_one_shot_path("event:/MusicPacks/og_classic")

func _process(delta):
	pass
