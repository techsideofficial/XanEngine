extends Node

var isDebug = true

var config = ConfigFile.new()
var err = config.load_encrypted_pass("user://Config/localuser.qucfg", "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789")

# Called when the node enters the scene tree for the first time.
func _ready():
	if err != OK:
		#AthenaInfo (Config Version, Type)
		config.set_value("UserProfileMetadata", "Athena", true)
		config.set_value("UserProfileMetadata", "AthenaVer", "Quantum-01A")
		config.set_value("UserProfileMetadata", "CustomAthena", true)
		config.set_value("UserProfileMetadata", "CustomAthenaVer", "01A")
		
		#PlayerData (Userdata, AccountInfo)
		config.set_value("PlayerData", "Username", "QuantumDev")
		config.set_value("PlayerData", "EpicID", "0000-0000-0000")
		config.set_value("PlayerData", "Vbucks", 10000000)
		config.set_value("PlayerData", "AccountLevel", 999)
		config.set_value("PlayerData", "Relationships", [])
		config.set_value("PlayerData", "IsDev", true)
		
		#Cosmetics (Owned, Gifting)
		config.set_value("Cosmetics", "OwnedMPacks", ["all"])
		config.set_value("Cosmetics", "OwnedMPacks", ["all"])
		config.set_value("Cosmetics", "OwnedSkins", ["all"])
		config.set_value("Cosmetics", "OwnedTools", ["all"])
		config.set_value("Cosmetics", "OwnedGliders", ["all"])
		
		#Save
		if isDebug == true:
			config.save("user://Config/localuser.qucfg")
		else:
			config.save_encrypted_pass("user://Config/localuser.qucfg", "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789")


func _process(delta):
	pass
