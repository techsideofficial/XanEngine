extends Node

func _ready():
	pass
	
func _process(delta):
	pass

func _on_set_pressed():
	var config = ConfigFile.new()
	config.set_value("XAC", "KEY", $VBoxContainer/LineEdit.text)
	config.set_value("SETTINGS", "GamePath", $VBoxContainer/gpath.text)
	config.save_encrypted_pass("res://xac", "YnWfWyFtzQ818bCXgWUoz0ZzmHW8hTYL")

