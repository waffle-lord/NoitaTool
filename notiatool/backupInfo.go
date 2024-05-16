package main

type BackupInfo struct {
	Name         string
	Path         string
	health       uint8
	gold         uint64
	kills        uint64
	currentBiome string
}

func NewBackupInfo(name string, path string) *BackupInfo {
	return &BackupInfo{
		Name: name,
		Path: path,
	}
}
