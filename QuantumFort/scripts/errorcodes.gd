extends Node
class_name error
var errorCode = ""

func unknown():
	get_tree().change_scene("res://scenes/error.tscn")
	errorCode = "Error Code 100: Unknown error."
	
func banks():
	SceneTree.new().get
	errorCode = "Error Code 101: Failed to initialize banks."
	
func net():
	get_tree().change_scene("res://scenes/error.tscn")
	errorCode = "Error Code 102: Failed to connect to QuantumNet services."
	
func outdated():
	get_tree().change_scene("res://scenes/error.tscn")
	errorCode = "Error Code 103: Outdated client."
	
func aparse():
	get_tree().change_scene("res://scenes/error.tscn")
	errorCode = "Error Code 104: Failed to parse audio."
	
func access():
	get_tree().change_scene("res://scenes/error.tscn")
	errorCode = "Error Code 105: Access denied."
	
func _ready():
	pass

func _process(delta):
	pass
