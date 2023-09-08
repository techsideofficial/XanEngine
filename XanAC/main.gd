extends Node

var prog = null

func _ready():
	$AnimationPlayer.play("logospin")
	prog = Timer.new()
	prog.one_shot = true
	prog.wait_time = 7.0
	add_child(prog)
	prog.timeout.connect(_on_fin)
	prog.start()


func _process(delta):
	if prog.get_time_left() * 100 <= 650:
		$task.text = "VERIFYING"
	if prog.get_time_left() * 100 <= 300:
		$task.text = "PREPARING"
	if prog.get_time_left() * 100 <= 150:
		$task.text = "LAUNCHING"
	#print(prog.get_time_left() * 100)
	$ProgressBar.value = -prog.get_time_left() * 100
	
func _on_fin():
	var config = ConfigFile.new()
	config.load_encrypted_pass("res://XanAntiCheat/xac", "YnWfWyFtzQ818bCXgWUoz0ZzmHW8hTYL")
	var key = config.get_value("XAC", "KEY")
	var gamePath = config.get_value("SETTINGS", "GamePath")
	OS.execute("cmd.exe", ["/C", gamePath + " -xac-" + key])
	get_tree().quit()
