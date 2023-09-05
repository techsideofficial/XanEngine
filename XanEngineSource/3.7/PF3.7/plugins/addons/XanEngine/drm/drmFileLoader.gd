extends Node
class_name XanDRM

func ConfigExists(path):
	var config = ConfigFile.new()
	var err = config.load(path)
	if err == OK:
		return true
	else:
		return false

func _ready(): pass

func _process(delta): pass
