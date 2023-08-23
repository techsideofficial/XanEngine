@tool
extends EditorPlugin


func _enter_tree():
	add_custom_type("XanEngine", "Node", preload("XanEngine.gd"), preload("xan-icon.png"))


func _exit_tree():
	remove_custom_type("XanEngine")
