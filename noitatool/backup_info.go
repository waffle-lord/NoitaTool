package main

type BackupInfo struct {
	Name   string
	Path   string
	Player *NoitaPlayer
}

func NewBackupInfo(name string, path string) *BackupInfo {

	player, err := LoadPlayerData(path)

	if err != nil {
		println("Error while loading player data")
		println(err.Error())
	}

	return &BackupInfo{
		Name:   name,
		Path:   path,
		Player: player,
	}
}
