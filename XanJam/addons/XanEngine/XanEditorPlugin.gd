@tool
extends EditorPlugin


func _enter_tree():
	add_custom_type("XanEngine", "Node", preload("scripts/XanEngine.gd"), preload("assets/ico_xan16.png"))


func _exit_tree():
	remove_custom_type("XanEngine")
