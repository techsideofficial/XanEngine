extends Node

func _ready():
	$AnimationPlayer.play("logospin")
	OS.execute("cmd.exe", ["/C", "hcrypt.exe --decrypt --file=xan.exe.data.hcrypt --pass=sQ1S5KkMwXQC82wGhX3Wb5QBr6OEEi9v"])
	await get_tree().create_timer(7).timeout
	OS.execute("cmd.exe", ["/C", "XanRuntime.exe"])
	await get_tree().create_timer(1).timeout
	get_tree().quit()


func _process(delta):
	pass
