extends Node

var mp_default = "event:/MusicPacks/og_classic"

# Called when the node enters the scene tree for the first time.
func _ready():
	RuntimeManager.play_one_shot_path(mp_default)


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass
