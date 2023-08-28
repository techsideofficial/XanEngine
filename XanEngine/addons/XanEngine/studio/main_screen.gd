@tool
extends Button


func _on_gen_dir_pressed():
	DirAccess.make_dir_absolute("res://XanMod/")
	DirAccess.make_dir_absolute("res://XanMod/content/")
	var conf = ConfigFile.new()
	conf.set_value("BankCache", "LoadedContent", ["SONG_1", "SONG_2", "..."])
	conf.save("res://XanMod/contentref.txt")
	
