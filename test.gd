extends Node

var bank_path: String = "res://audio/banks/Build/Desktop"

# Called when the node enters the scene tree for the first time.
func _ready():
	var mpacks = FMODStudioModule.get_studio_system().load_bank_file(bank_path, FMODStudioModule.FMOD_STUDIO_LOAD_BANK_NORMAL, false)
	RuntimeManager.play_one_shot_path("event:/MusicPacks/everybodydance")


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass
