package main

import (
	"bufio"
	"os"
	"strings"
)

type NotiaStats struct {
	Health       uint8
	Gold         uint64
	Kills        uint64
	CurrentBiome string
}

func LoadStats(backupPath string) *NotiaStats {

	file, err := os.Open(backupPath + "\\player.xml")

	if err != nil {
		println(err.Error())

		return &NotiaStats{
			Health:       0,
			Gold:         0,
			Kills:        0,
			CurrentBiome: "unknown",
		}
	}
	defer file.Close()

	scanner := bufio.NewScanner(file)

	killsFile := ""
	statsFile := ""

	for scanner.Scan() {
		trimmedLine := strings.TrimSpace(scanner.Text())

		if strings.HasPrefix(trimmedLine, "stats_filename=") && strings.HasSuffix(trimmedLine, ".xml\" >") {
			println("FOUND KILLS_FILE: ", trimmedLine)
			killsFile = strings.Split(trimmedLine, "=")[1]
			killsFile = strings.Split(killsFile, "/")[2]
			killsFile = strings.Replace(killsFile, "\" >", "", 1)
			println("KILLS_FILE NAME: ", killsFile)

			killsFile = backupPath + "\\stats\\sessions\\" + killsFile
			statsFile = strings.Replace(killsFile, "kills", "stats", 1)
			println("KILLS_FILE PATH: ", killsFile)
			println("STATS_FILE PATH: ", statsFile)

			// todo: load stats from xml
			// considering just building out structs for the data, idk...

			break
		}
	}

	return &NotiaStats{
		Health:       1,
		Gold:         1,
		Kills:        1,
		CurrentBiome: "unknown",
	}
}
