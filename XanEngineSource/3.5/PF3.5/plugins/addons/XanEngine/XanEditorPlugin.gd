@tool
extends EditorPlugin


const MainPanel: PackedScene = preload("res://addons/XanEngine/studio/main_panel.tscn")

var main_panel_instance


func _enter_tree():
	add_custom_type("XanEngine", "Node", preload("scripts/XanEngine.gd"), preload("assets/ico_xan16.png"))
	DirAccess.make_dir_absolute("user://XanEngine/")
	DirAccess.make_dir_absolute("user://XanEngine/Config")
	DirAccess.make_dir_absolute("user://XanEngine/Banks")
	main_panel_instance = MainPanel.instantiate()
	get_editor_interface().get_editor_main_screen().add_child(main_panel_instance)
	_make_visible(false)


func _exit_tree():
	if main_panel_instance:
		main_panel_instance.queue_free()
	remove_custom_type("XanEngine")


func _has_main_screen():
	return true


func _make_visible(visible):
	if main_panel_instance:
		main_panel_instance.visible = visible


func _get_plugin_name():
	return "XanEngine"


func _get_plugin_icon():
	return load("res://addons/XanEngine/assets/ico_xan16.png")
