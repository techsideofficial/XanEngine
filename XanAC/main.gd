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
	print(prog.get_time_left() * 100)
	$ProgressBar.value = -prog.get_time_left() * 100
	
func _on_fin():
	DirAccess.make_dir_absolute("res://Cert/")
	var config = ConfigFile.new()
	var keys = config.load_encrypted_pass("res://Cert/crypt", "YnWfWyFtzQ818bCXgWUoz0ZzmHW8hTYL")
	var gamepath = config.load("res://Settings.cfg")
	#config.set_value("XAC", "KEY", "YnWfWyFtzQ818bCXgWUoz0ZzmHW8hTYL")
	#config.save_encrypted_pass("res://Cert/crypt", "YnWfWyFtzQ818bCXgWUoz0ZzmHW8hTYL")
	#config.set_value("SETTINGS", "GamePath", "")
	#config.save("res://Settings.cfg")
	OS.execute("cmd.exe", ["/C", gamepath - args])
	get_tree().quit()
