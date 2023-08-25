extends Node


# Called when the node enters the scene tree for the first time.
func _ready():
	#XanEngine.new().ConfigSave("TestData", "TestVersion", 1.0, "XanDefaultEngine")
	var events = XanEngine.new().ConfigLoad("BankEventCache", "xcache", "BankCache", "DownloadedEvents")
	print(events)
	print("event:/MusicPacks/" + events[1])
	#RuntimeManager.play_one_shot_path(bankFile + events[1])


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass
