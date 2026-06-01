package main

import (
	"flag"
	"log"
	"net/http"
	"os"
	"path/filepath"
	"strings"
)

func main() {
	root := flag.String("root", "/www", "static file root")
	listen := flag.String("listen", ":8080", "listen address")
	flag.Parse()

	handler := http.HandlerFunc(func(writer http.ResponseWriter, request *http.Request) {
		cleanPath := filepath.Clean(request.URL.Path)
		requestedPath := filepath.Join(*root, cleanPath)

		if info, err := os.Stat(requestedPath); err == nil && !info.IsDir() {
			http.ServeFile(writer, request, requestedPath)
			return
		}

		if strings.Contains(filepath.Base(request.URL.Path), ".") {
			http.NotFound(writer, request)
			return
		}

		http.ServeFile(writer, request, filepath.Join(*root, "index.html"))
	})

	log.Printf("serving %s on %s", *root, *listen)
	log.Fatal(http.ListenAndServe(*listen, handler))
}