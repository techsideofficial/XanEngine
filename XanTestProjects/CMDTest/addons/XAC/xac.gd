extends Node
class_name XAC

func check(eacKey):
		if !str(OS.get_cmdline_args()).contains("-eac-" + eacKey):
			get_tree().quit()
