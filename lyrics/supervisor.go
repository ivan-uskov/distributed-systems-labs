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

func doWithWaitGroup(jobCreator func(w Launcher)){
	var wg WaitGroup
	jobCreator(&wg)
	wg.wait()
}

func runSync(cmd string) {
	fmt.Println("Run: " + cmd)
	exec.Command(cmd).Run()
}

func wrapCommandName(cmd string) string {
	return "backend/" + cmd + "/bin/release/" + cmd + ".exe"
}

func times(n uint) []struct{} {
	return make([]struct{}, n)
}

func prepareCommands(commands map[string]uint) []func()  {
	var prepared []func()
	for cmd, count := range commands {
		fullCmd := wrapCommandName(cmd)
		for range times(count) {
			prepared = append(prepared, func() {
				runSync(fullCmd)
			})
		}
	}

	return prepared
}

func main() {
	const WORKERS_COUNT = 1
	commands := map[string]uint{
		"backend": 1,
		"BadWordsReplacer": WORKERS_COUNT,
		"CapsWordsToLowerCaser": WORKERS_COUNT,
		"LyricSaver": WORKERS_COUNT,
	}

	doWithWaitGroup(func(l Launcher) {
		for _, cmd := range prepareCommands(commands) {
			l.launch(cmd);
		}
	})
}