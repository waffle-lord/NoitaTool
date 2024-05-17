package main

type BackupInfo struct {
	Name  string
	Path  string
	Stats *NoitaStats
}

func NewBackupInfo(name string, path string) *BackupInfo {
	return &BackupInfo{
		Name:  name,
		Path:  path,
		Stats: LoadStats(path),
	}
}
