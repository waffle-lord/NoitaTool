package main

import (
	"encoding/xml"
	"errors"
	"os"
)

type DamageModelComponent struct {
	CurrentHp float32 `xml:"hp,attr"`
	MaxHp     float32 `xml:"max_hp,attr"`
}

type WalletComponent struct {
	Money      int64 `xml:"money,attr"`
	MoneySpent int64 `xml:"money_spent,attr"`
}

type NoitaPlayer struct {
	Health DamageModelComponent `xml:"DamageModelComponent"`
	Wallet WalletComponent      `xml:"WalletComponent"`
}

func LoadPlayerData(playerFilePath string) (*NoitaPlayer, error) {

	playerFileBytes, err := os.ReadFile(playerFilePath)

	if err != nil {
		return &NoitaPlayer{}, err
	}

	if len(playerFileBytes) == 0 {
		return &NoitaPlayer{}, errors.New("player data is empty")
	}

	println(len(playerFileBytes), "bytes loaded from player.xml")

	player := &NoitaPlayer{Health: DamageModelComponent{}, Wallet: WalletComponent{}}

	err = xml.Unmarshal(playerFileBytes, &player)

	if err != nil {
		return &NoitaPlayer{}, err
	}

	return player, nil
}
