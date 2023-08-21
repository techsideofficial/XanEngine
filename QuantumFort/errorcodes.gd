extends Node
class_name error

func unknown():
	return "Error Code 100: Unknown error."
	
func banks():
	return "Error Code 101: Failed to initialize banks."
	
func net():
	return "Error Code 102: Failed to connect to QuantumNet services."
	
func outdated():
	return "Error Code 103: Outdated client."
	
func aparse():
	return "Error Code 104: Failed to parse audio."
	
func access():
	return "Error Code 105: Access denied."
	
func _ready():
	pass

func _process(delta):
	pass
