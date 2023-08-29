@tool
extends GridContainer

func _on_dirstruct_pressed():
	DirAccess.make_dir_absolute("res://XanMod/")
	DirAccess.make_dir_absolute("res://XanMod/content/")
	var conf = ConfigFile.new()
	var conf2 = ConfigFile.new()
	conf.set_value("BankCache", "LoadedContent", ["SONG_1", "SONG_2", "..."])
	conf2.set_value("INFO", "INFO", "This folder should contain the files 'Master.bank, Master.strings.bank, and Xan.bank'")
	conf.save("res://XanMod/contentref.txt")
	conf2.save("res://XanMod/content/README.txt")
	
func _on_help_pressed():
	OS.shell_open("https://github.com/techsideofficial/XanEngine/blob/main/Docs/DOCS.md")
	
func _on_update_pressed():
	OS.shell_open("https://github.com/techsideofficial/XanEngine/releases")
	
func _on_template_pressed():
	var dir = DirAccess.open("res://")
	dir.new().copy("res://addons/XanEngine/staging/export_presets.cfg", "res://export_presets.cfg")
