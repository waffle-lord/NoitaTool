package main

import (
	"context"
	"fmt"
	"os"
	"path/filepath"

	cp "github.com/otiai10/copy"
)

// App struct
type App struct {
	ctx                  context.Context
	noita_game_path      string
	notia_main_save_path string
	backup_folder_path   string
	backups              []BackupInfo
}

// NewApp creates a new App application struct
func NewApp() *App {

	game_path, err := os.UserCacheDir()

	if err != nil {
		println("Failed to get user cache dir")
	}

	ex, err := os.Executable()

	if err != nil {
		println("Failed to get current exe path")
	}

	exDir := filepath.Dir(ex)

	game_path += "Low\\Nolla_Games_Noita"
	main_save := game_path + "\\save00"
	backup_path := exDir + "\\notia_backups"

	entries, err := os.ReadDir(backup_path)

	if err != nil {
		println("Failed to get backups")
	}

	println("::dir contents::")

	foundBackups := []BackupInfo{}

	for _, e := range entries {

		if !e.IsDir() {
			continue
		}

		println(e.Name())

		foundBackups = append(foundBackups, *NewBackupInfo(e.Name(), backup_path+"\\"+e.Name()))
	}

	return &App{
		noita_game_path:      game_path,
		notia_main_save_path: main_save,
		backup_folder_path:   backup_path,
		backups:              foundBackups,
	}
}

// startup is called when the app starts. The context is saved
// so we can call the runtime methods
func (a *App) startup(ctx context.Context) {
	a.ctx = ctx
}

func (a *App) BackupSave(name string) string {
	source := a.notia_main_save_path
	destination := a.backup_folder_path + "\\" + name

	err := cp.Copy(source, destination)

	if err != nil {
		println(err.Error())
		return "Failed to backup save"
	}

	a.backups = append(a.backups, *NewBackupInfo(name, destination))

	return fmt.Sprintf("backup complete: %s", name)
}

func (a *App) RestoreSave(name string) {
	println("not implemented: ", name)
}

func (a *App) GetBackups() []BackupInfo {
	return a.backups
}
