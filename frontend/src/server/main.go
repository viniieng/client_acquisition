package main

import (
	"encoding/json"
	"flag"
	"log"
	"net/http"
	"os"
	"path/filepath"
	"strings"
)

func main() {
	root   := flag.String("root", "/www", "static file root")
	listen := flag.String("listen", ":8080", "listen address")
	flag.Parse()

	apiBaseUrl := os.Getenv("API_BASE_URL")
	if apiBaseUrl == "" {
		apiBaseUrl = "http://localhost:5000/api/"
	}

	mux := http.NewServeMux()

	mux.HandleFunc("/api-config.json", func(w http.ResponseWriter, r *http.Request) {
		w.Header().Set("Content-Type", "application/json")
		json.NewEncoder(w).Encode(map[string]string{
			"ApiBaseUrl": apiBaseUrl,
		})
	})

	mux.HandleFunc("/", func(writer http.ResponseWriter, request *http.Request) {
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

	log.Printf("serving %s on %s (API → %s)", *root, *listen, apiBaseUrl)
	log.Fatal(http.ListenAndServe(*listen, mux))
}