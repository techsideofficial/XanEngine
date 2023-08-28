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

func _has_main_screen():
	return true


func _make_visible(visible):
	pass


func _get_plugin_name():
	return "Main Screen Plugin"


func _get_plugin_icon():
	return get_editor_interface().get_base_control().get_theme_icon("Node", "EditorIcons")
