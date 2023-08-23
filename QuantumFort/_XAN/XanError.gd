extends Node
var errorCode = ""

func unknown():
	errorCode = "Error Code 100: Unknown error."
	get_tree().change_scene("res://scenes/error.tscn")
	
func banks():
	errorCode = "Error Code 101: Failed to initialize banks."
	get_tree().change_scene("res://scenes/error.tscn")
	
func net():
	errorCode = "Error Code 102: Failed to connect to QuantumNet services."
	get_tree().change_scene("res://scenes/error.tscn")
	
func outdated():
	errorCode = "Error Code 103: Outdated client."
	get_tree().change_scene("res://scenes/error.tscn")
	
func aparse():
	errorCode = "Error Code 104: Failed to parse audio."
	get_tree().change_scene("res://scenes/error.tscn")
	
func access():
	errorCode = "Error Code 105: Access denied."
	get_tree().change_scene("res://scenes/error.tscn")
	
func _ready(): pass
func _process(delta): pass
