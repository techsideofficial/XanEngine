extends Node

var config = ConfigFile.new()
var err = config.load_encrypted_pass("user://Config/localuser.qucfg", "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789")
var mp_packs = config.get_value("Cosmetics", "OwnedPacks")

var mp_default = "event:/MusicPacks/og_classic"
var mp_llamabell = "event:/MusicPacks/llamabell"
var mp_everybodydance = "event:/MusicPacks/everybodydance"
var mp_floss = "event:/MusicPacks/floss"
var mp_lilwhip = "event:/MusicPacks/lilwhip"

# Called when the node enters the scene tree for the first time.
func _ready():
	RuntimeManager.play_one_shot_path(mp_default)


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass
