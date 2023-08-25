# !!DO NOT REMOVE THIS HEADER!!
# This script is liscened to you, as a developer, for use in hobby or commercial products.
# Selling or redistributing this script without permission from the owner [XAN Technologies Corp] is strictly prohibited.
# This file contains the source code for XanEngine.
# -- END OF HEADER --

extends Node
class_name XanEngine

func _ready(): pass
func _process(delta): pass

func SceneChange(path: String):
	get_tree().change_scene_to_file(path)
	
func ConfigExists(path):
	var config = ConfigFile.new()
	var err = config.load(path)
	if err == OK:
		return true
	else:
		return false
		
func ConfigSave(section, key, value, configName):
	var config = ConfigFile.new()
	config.set_value("XanEngine", "Version", 1.0)
	config.set_value("XanEngine", "CodeName", "Athena")
	config.set_value("XanEngine", "ConfigType", "Config")
	config.set_value(section, key, value)
	config.save("user://XanEngine/Config/" + configName + ".xfg")
	
func fmodExists():
	var config = ConfigFile.new()
	var err = config.load("res://addons/FMOD/plugin.cfg")
	if err == OK:
		return true
	else:
		return false

func log(message):
	print("[XAN]" + message)
	
func keyGen(length):
	var let = ["a","b","c","d","e","f","g","h","i","j","k","l","m","n","o","p","q","r","s","t","u","v","w","x","y","z"]
	var num = ["1","2","3","4","5","6","7","8","9","0"]
	var com = [let, num]
	var comRand = null
	var key = ""
	for i in range(length):
		comRand = com.pick_random()
		key = key + comRand.pick_random()
	return key
		
func ConfigLoad(configName, fileType, section, key):
	var config = ConfigFile.new()
	var err = config.load("user://XanEngine/Config/" + configName + "." + fileType)
	if err != OK:
		XanEngine.new().log("ERROR! You need to create a config before you can load it. This can be done with 'XanEngine.new().ConfigSave(section, key, value, configName)'.")
	return config.get_value(section, key)
