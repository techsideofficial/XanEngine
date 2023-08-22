extends Node


# Called when the node enters the scene tree for the first time.
func _ready():
	get_node("/root/error/errorMsg").text = error.new().errorCode


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass
