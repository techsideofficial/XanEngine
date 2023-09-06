extends Node

var prog = null

func _ready():
	$AnimationPlayer.play("logospin")
	prog = Timer.new()
	prog.one_shot = true
	prog.wait_time = 7.0
	add_child(prog)
	prog.timeout.connect(_on_fin)
	OS.execute("cmd.exe", ["/C", "hcrypt.exe --decrypt --file=xan.exe.data.hcrypt --pass=sQ1S5KkMwXQC82wGhX3Wb5QBr6OEEi9v"])
	prog.start()


func _process(delta):
	if prog.get_time_left() * 100 <= 650:
		$task.text = "VERIFYING"
	if prog.get_time_left() * 100 <= 300:
		$task.text = "PREPARING"
	if prog.get_time_left() * 100 <= 150:
		$task.text = "LAUNCHING"
	print(prog.get_time_left() * 100)
	$ProgressBar.value = -prog.get_time_left() * 100
	
func _on_fin():
	OS.execute("cmd.exe", ["/C", "XanRuntime.exe"])
	get_tree().quit()
