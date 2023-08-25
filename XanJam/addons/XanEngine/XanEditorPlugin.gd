@tool
extends EditorPlugin


func _enter_tree():
	add_custom_type("XanEngine", "Node", preload("scripts/XanEngine.gd"), preload("assets/ico_xan16.png"))
	DirAccess.make_dir_absolute("user://XanEngine/")
	DirAccess.make_dir_absolute("user://XanEngine/Config")
	DirAccess.make_dir_absolute("user://XanEngine/Banks")
	XanEngine.new().log("Initialised XanEngine")


func _exit_tree():
	XanEngine.new().log("Shutting Down XanEngine")
	remove_custom_type("XanEngine")
