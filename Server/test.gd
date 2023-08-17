extends Node


# Called when the node enters the scene tree for the first time.
func _ready():
	var server = HttpServer.new()
	server.register_router("/", testrouter.new())
	add_child(server)
	server.start()


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass
