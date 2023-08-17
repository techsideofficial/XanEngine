extends Node

var config = ConfigFile.new()
var err = config.load_encrypted_pass("user://Config/localuser.qucfg", "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789")

# Called when the node enters the scene tree for the first time.
func _ready():
	if err != OK:
		config.set_value("PlayerData", "Username", "QuantumDev")
		config.set_value("PlayerData", "Vbucks", 10000000)
		config.set_value("PlayerData", "Relationships", [])
		config.set_value("PlayerData", "IsDev", true)
		config.set_value("Cosmetics", "OwnedPacks", ["mp_default", "mp_llamabell", "mp_everybodydance", "mp_floss", "mp_lilwhip"])
		config.save_encrypted_pass("user://Config/localuser.qucfg", "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789")


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass
