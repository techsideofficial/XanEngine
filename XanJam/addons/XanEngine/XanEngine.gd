extends Node
class_name XanEngine

func SceneChange(path: String):
	get_tree().change_scene_to_file(path)
	


func _ready():
	pass

func _process(delta):
	pass
