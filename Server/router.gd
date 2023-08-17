extends HttpRouter
class_name testrouter

# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass

func handle_get(request, response):
	response.send(200, "Hello!")
