package main

import (
	"context"
	"errors"
	"fmt"
	"os"
	"path/filepath"
	"strings"

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

	foundBackups := loadBackups(backup_path)

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

func loadBackups(backup_path string) []BackupInfo {
	entries, err := os.ReadDir(backup_path)
	foundBackups := []BackupInfo{}

	if err != nil {
		println("Failed to get backups")
		return foundBackups
	}

	println("::dir contents::")

	for _, e := range entries {

		if !e.IsDir() {
			continue
		}

		println(e.Name())

		foundBackups = append(foundBackups, *NewBackupInfo(e.Name(), backup_path+"\\"+e.Name()))
	}

	return foundBackups
}

func (a *App) BackupSave(name string) string {
	name = strings.TrimSpace(name)

	if len(name) == 0 {
		return "no name provided"
	}

	if len(name) > 30 {
		return "30 char limit for name"
	}

	source := a.notia_main_save_path
	destination := a.backup_folder_path + "\\" + name

	err := cp.Copy(source, destination)

	if err != nil {
		println(err.Error())
		return "Failed to backup save"
	}

	a.backups = loadBackups(a.backup_folder_path)

	return fmt.Sprintf("backup complete: %s", name)
}

func (a *App) RestoreSave(name string) {
	println("not implemented: ", name)
}

func (a *App) DeleteSave(name string) bool {

	for i := 0; i < len(a.backups); i++ {
		b := a.backups[i]

		if b.Name == name {
			if _, err := os.Stat(b.Path); errors.Is(err, os.ErrNotExist) {
				println(fmt.Sprintf("could not find path to delete: %s", b.Path))
				return false
			}

			err := os.RemoveAll(b.Path)

			if err != nil {
				println(err)
				return false
			}

			a.backups = append(a.backups[:i], a.backups[i+1:]...)

			return true
		}
	}

	return false
}

func (a *App) GetBackups() []BackupInfo {
	return a.backups
}
