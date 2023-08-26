extends Node

var selectSong = null

# Called when the node enters the scene tree for the first time.
func _ready():
	var songs = get_node("/root/SongBrowser/ItemList")
	get_node("/root/SongBrowser/ItemList").item_activated.connect(itemClicked)
	#XanEngine.new().ConfigSave("TestData", "TestVersion", 1.0, "XanDefaultEngine")
	XanEngine.new().reloadBanks("user://XanEngine/Paks/DefaultBanks.xax")
	var events = XanEngine.new().ConfigLoad("res://XanMod/contentref.txt", "BankCache", "LoadedContent")
	#RuntimeManager.play_one_shot_path("event:/MusicPacks/" + events[3])
	print(events)
	
	for item in events:	
		get_node("/root/SongBrowser/ItemList").add_item(item, null, true)


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta): pass

func itemClicked(index):
	FMODStudioModule.get_studio_system().get_bus("bus:/").stop_all_events(FMODStudioModule.FMOD_STUDIO_STOP_IMMEDIATE)
	var songs = get_node("/root/SongBrowser/ItemList")
	var songName = songs.get_item_text(index)
	var currentSong = RuntimeManager.create_instance_path("event:/" + songName)
	currentSong.start()
