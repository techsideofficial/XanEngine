extends Node


# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass

const CHUNK_SIZE = 1024

func hash_file(path):
	# Check that file exists.
	if not FileAccess.file_exists(path):
		return
	# Start a SHA-256 context.
	var ctx = HashingContext.new()
	ctx.start(HashingContext.HASH_SHA256)
	# Open the file to hash.
	var file = FileAccess.open(path, FileAccess.READ)
	# Update the context after reading each chunk.
	while not file.eof_reached():
		ctx.update(file.get_buffer(CHUNK_SIZE))
	# Get the computed hash.
	var res = ctx.finish()
	# Print the result as hex string and array.
	printt(res.hex_encode(), Array(res))
