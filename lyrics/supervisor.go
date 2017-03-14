package main

import (
	"sync"
	"os/exec"
	"fmt"
)

type WaitGroup struct {
	waitGroup sync.WaitGroup
}

func (w * WaitGroup) wait() {
	w.waitGroup.Wait()
}

func (w * WaitGroup) launch(job func()) {
	w.waitGroup.Add(1)

	go func() {
		job()
		w.waitGroup.Done()
	}()
}

type Launcher interface {
	launch(job func())
}

func launchWithWaitGroup(jobCreator func(w Launcher)){
	var wg WaitGroup
	jobCreator(&wg)
	wg.wait()
}

func runSync(cmd string) {
	fmt.Println("Run: " + cmd)
	exec.Command(cmd).Run()
	fmt.Println("Exit: " + cmd)
}

func wrapBackendCommandName(cmd string) string {
	return "backend/" + cmd + "/bin/release/" + cmd + ".exe"
}

func wrapFrontendCommandName(cmd string) string {
	return "frontend/" + cmd
}

type CommandsList map[string]uint
type CommandsWrapper func(string)string
type AsyncCommand func()

func prepareCommands(commands CommandsList, nameWrapper CommandsWrapper) []AsyncCommand  {
	var prepared []AsyncCommand
	for cmd, count := range commands {
		fullCmd := nameWrapper(cmd)
		for countCopy := count; countCopy > 0; countCopy-- {
			prepared = append(prepared, func() {
				runSync(fullCmd)
			})
		}
	}

	return prepared
}

func main() {
	const WORKERS_COUNT = 1

	frontendCommands := map[string]uint{
		"run.bat": 1,
	}

	backendCommands := map[string]uint{
		"backend": 1,
		"LyricStatistics": 1,
		"LyricStorage": 1,
		"LyricValidator": WORKERS_COUNT,
		"BadWordsReplacer": WORKERS_COUNT,
		"CapsWordsToLowerCaser": WORKERS_COUNT,
	}

	launchWithWaitGroup(func(l Launcher) {
		for _, cmd := range prepareCommands(frontendCommands, wrapFrontendCommandName) {
			l.launch(cmd);
		}

		for _, cmd := range prepareCommands(backendCommands, wrapBackendCommandName) {
			l.launch(cmd);
		}

		l.launch(func() {
			fmt.Println("1")
			exec.Command("call", "frontend/run.bat").Run()
			fmt.Println("2")
		})
	})
}