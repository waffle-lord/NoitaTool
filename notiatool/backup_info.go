package main

type BackupInfo struct {
	Name  string
	Path  string
	Stats *NotiaStats
}

func NewBackupInfo(name string, path string) *BackupInfo {
	return &BackupInfo{
		Name:  name,
		Path:  path,
		Stats: LoadStats(path),
	}
}
