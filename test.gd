extends Node

var bank_path: String = "res://audio/banks/Build/Desktop"

func _ready():
	RuntimeManager.play_one_shot_path("event:/MusicPacks/everybodydance")
	RuntimeManager

func _process(delta):
	pass
