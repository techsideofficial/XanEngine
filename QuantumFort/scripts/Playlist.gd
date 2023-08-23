extends Node
class_name qplayer

func get_songs():
	var config = ConfigFile.new()
	var err = config.load("user://fmod.qcfg")

	if err != OK:
		print(XanPaths.new().XanErrorPath)
		get_node(XanPaths.new().XanErrorPath).banks()
	
	var songs = config.get_value("BankEvents", "LocalEvents")

func _ready():
	pass

func _process(delta):
	pass
