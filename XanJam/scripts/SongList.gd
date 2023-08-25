extends Node


# Called when the node enters the scene tree for the first time.
func _ready():
	#XanEngine.new().ConfigSave("TestData", "TestVersion", 1.0, "XanDefaultEngine")
	var events = XanEngine.new().ConfigLoad("BankEventCache", "xcache", "BankCache", "DownloadedEvents")
	FMODStudioModule.get_studio_system().unload_all()
	ProjectSettings.load_resource_pack("user://XanEngine/Banks/banks.pck")
	FMODStudioModule.get_studio_system().load_bank_file("res://Master.bank", 0, false)
	FMODStudioModule.get_studio_system().load_bank_file("res://Master.strings.bank", 0, false)
	FMODStudioModule.get_studio_system().load_bank_file("res://MusicPacks.bank", 0, false)
	print(events[1])
	RuntimeManager.play_one_shot_path("event:/MusicPacks/placeholder")


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass
