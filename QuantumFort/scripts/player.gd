extends Node
class_name qplayer

func get_songs():
	var bPak = FMODStudioModule.get_studio_system().get_bank("res://fmod/Build/Desktop/MusicPacks.bank")
	print(bPak.get_event_list(2))

func _ready():
	get_songs()

func _process(delta):
	pass
