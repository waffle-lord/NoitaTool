package main

import (
	"encoding/xml"
	"errors"
	"os"
	"path/filepath"
	"strings"
)

type KillStats struct {
	Kills int64 `xml:"kills,attr"`
}

type PlayerStats struct {
	KillStats KillStats `xml:"Stats"`
}

type GameStatsComponent struct {
	StatsFile string `xml:"stats_filename,attr"`
}

type DamageModelComponent struct {
	CurrentHp float32 `xml:"hp,attr"`
	MaxHp     float32 `xml:"max_hp,attr"`
}

type WalletComponent struct {
	Money      int64 `xml:"money,attr"`
	MoneySpent int64 `xml:"money_spent,attr"`
}

type NoitaPlayer struct {
	Health    DamageModelComponent `xml:"DamageModelComponent"`
	Wallet    WalletComponent      `xml:"WalletComponent"`
	StatsInfo GameStatsComponent   `xml:"GameStatsComponent"`
	Stats     PlayerStats
}

func LoadPlayerData(backupPath string) (*NoitaPlayer, error) {

	playerFileBytes, err := os.ReadFile(filepath.Join(backupPath, "player.xml"))

	if err != nil {
		return &NoitaPlayer{}, err
	}

	if len(playerFileBytes) == 0 {
		return &NoitaPlayer{}, errors.New("player data is empty")
	}

	println(len(playerFileBytes), "bytes loaded from player.xml")

	player := &NoitaPlayer{Health: DamageModelComponent{}, Wallet: WalletComponent{}, StatsInfo: GameStatsComponent{}, Stats: PlayerStats{}}

	err = xml.Unmarshal(playerFileBytes, &player)

	if err != nil {
		return &NoitaPlayer{}, err
	}

	err = xml.Unmarshal(playerFileBytes, &player.StatsInfo)

	if err != nil {
		return &NoitaPlayer{}, err
	}

	player.StatsInfo.StatsFile = strings.Replace(player.StatsInfo.StatsFile, "??STA/sessions/", "", 1)

	println("STATS_FILE_NAME: ", player.StatsInfo.StatsFile)

	killsFile := filepath.Join(backupPath, "stats", "sessions", player.StatsInfo.StatsFile)

	println("KILLS_FILE: ", killsFile)

	if _, err := os.Stat(killsFile); errors.Is(err, os.ErrNotExist) {
		return &NoitaPlayer{}, err
	}

	killStatsBytes, err := os.ReadFile(killsFile)

	if err != nil {
		return &NoitaPlayer{}, err
	}

	if len(killStatsBytes) == 0 {
		return &NoitaPlayer{}, errors.New("failed to load player kill stats file")
	}

	println(len(killStatsBytes), "bytes loaded from", player.StatsInfo.StatsFile)

	err = xml.Unmarshal(killStatsBytes, &player.Stats.KillStats)

	if err != nil {
		return &NoitaPlayer{}, err
	}

	return player, nil
}
