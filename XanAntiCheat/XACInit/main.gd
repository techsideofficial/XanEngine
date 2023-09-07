extends Node

func _ready():
	pass
	
func _process(delta):
	pass
	
func _on_cert_pressed():
	var config = ConfigFile.new()
	DirAccess.make_dir_absolute("res://Crypt/")
	config.set_value("XAC", "KEY", $VBoxContainer/LineEdit.text)
	config.save_encrypted_pass("res://Crypt/crypt", "YnWfWyFtzQ818bCXgWUoz0ZzmHW8hTYL")
	
func _on_set_pressed():
	var config = ConfigFile.new()
	config.set_value("SETTINGS", "GamePath", "<GAME PATH>")
	config.save("res://Settings.cfg")

