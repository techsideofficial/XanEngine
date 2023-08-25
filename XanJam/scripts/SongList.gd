extends Node


# Called when the node enters the scene tree for the first time.
func _ready():
	#XanEngine.new().ConfigSave("TestData", "TestVersion", 1.0, "XanDefaultEngine")
	var events = XanEngine.new().ConfigLoad("BankEventCache", "xcache", "BankCache", "DownloadedEvents")
	
	ProjectSettings.load_resource_pack("user://XanEngine/Banks/banks.pck")
	var loaded = FMODStudioModule.get_studio_system().load_bank_file("res://MusicPacks.bank", 0, false)
	print(events[1])
	print(loaded)
	RuntimeManager.play_one_shot_path("event:/MusicPacks/everybodydance")


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass
