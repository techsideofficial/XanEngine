extends Node


# Called when the node enters the scene tree for the first time.
func _ready():
	#XanEngine.new().ConfigSave("TestData", "TestVersion", 1.0, "XanDefaultEngine")
	var events = XanEngine.new().ConfigLoad("BankEventCache", "xcache", "BankCache", "DownloadedEvents")
	RuntimeManager.play_one_shot_path("event:/MusicPacks/" + events[2])


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass
