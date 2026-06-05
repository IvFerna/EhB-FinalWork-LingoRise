# EhB-FinalWork-LingoRise

## Overview

This project is a bachelor-level game development assignment exploring **implicit language learning through gameplay**. Instead of relying on explicit drills, the game aims to support natural language acquisition through player interaction, repeated exposure, and contextual discovery.

The project uses:

- **Godot 4 (C#)**
- **Node.js**
- **Node.js backend with server-side Piper TTS generation**

---

## Project Status

This repository now contains a working game prototype with core gameplay systems and backend integration.

Built so far:

- Godot 4 C# project with mobile portrait configuration (1080x1920)
- Playable bakery scenario with player movement and item interaction
- Autoload systems for lexicon progress, dialogue, audio, requests, and TTS
- Lexicon exposure and unlock tracking with persistent save/load
- Inventory system, item pickup, and request validation
- UI systems for main menu, lexicon menu, settings, and inventory
- Dialogue bubbles with Spanish text-to-speech playback
- Node.js backend service for TTS generation and audio hosting

Current focus:

- Expanding gameplay interactions and world content
- Refining the lexicon learning flow
- Polishing mobile controls and UI feedback
- Strengthening the item request / reward loop

---

## Setup

### Prerequisites

- Godot 4.x
- Node.js

### Backend Setup

```bash
cd backend
npm install
npm run dev
```

The backend will start a TTS service accessible at `http://localhost:3000`.

### Godot Setup

Open `final-work-lingo-rise/` in Godot 4.x.

---

## Platform Support

Tested on:

- Windows
- Android

Primary target:

- Android mobile

---

## Project Structure

```
backend/               → Node.js backend for TTS and static audio storage
final-work-lingo-rise/→ Godot project, scenes, scripts, resources, and exports
```

Within `final-work-lingo-rise/`:

```
assets/        → In-game art, icons, tilesets, and imported assets
addons/        → Mobile virtual joystick support
android/       → Android build configuration and outputs
resources/     → Lexicon database and game resources
scenes/        → Core scenes, UI scenes, and scenario layouts
scripts/       → C# game logic, UI controllers, systems, and save data
```

Within `final-work-lingo-rise/scripts/`:

```
autoload/      → Singleton managers initialized at startup (Lexicon, Audio, TTS, etc.)
core/          → Scene management and game state coordination
gameplay/      → Gameplay mechanics and player interactions
savedata/      → Save/load functionality and game state persistence
systems/       → Core game systems (Inventory, Requests, Dialogue, etc.)
ui/            → UI screens and interactive elements
utilities/     → Helper functions and utility classes
```

---

## Implemented Systems

- **Lexicon System**: tracks exposures by heard, seen, and interacted events, unlocks vocabulary, and persists progress
- **Local Lexicon Repository**: loads vocabulary entries from `resources/Lexicon/LexiconDatabase.tres`
- **Dialogue Manager**: displays speech bubbles, manages dialogue priority, and exposes vocabulary during NPC and ambient dialogue
- **TTS Service**: requests audio from the backend, caches generated files, and plays them in-game
- **Audio Manager**: handles WAV playback for TTS and other audio events
- **Inventory & Request Flow**: item pickup, inventory slot management, request states, and validation for requested items
- **UI Flow**: main menu toggling, lexicon menu, settings menu, and inventory UI updates

---

## Backend Integration

The game communicates with a Node.js backend service for text-to-speech generation:

```text
[Godot TTSService] --> POST /api/tts/generate --> [Express backend]
[Express backend] --> [TTS Engine (Piper)] --> [Generated audio files]
[Godot TTSService] --> download/cached audio from backend
[AudioManager] --> play cached WAV
```

The backend handles audio generation, caching, and serving to the Godot client.

The project remains modular and scalable to support iterative development and academic experimentation.

Key principles:

- **Separation of concerns**: UI, gameplay, systems, and backend are decoupled
- **Event-driven communication**: autoload managers and scene scripts interact through signals and shared services
- **Data persistence**: lexicon progress is saved locally for replayability and retention analysis
- **Mobile-first design**: mobile resolution, input mapping, and touch-friendly controls are supported

---

## Development Workflow

Version control follows a structured Git workflow:

- `main` → stable builds
- `develop` → integration branch
- `feature/*` → individual features

Commit convention:

```
feat: add new feature
fix: resolve bug
chore: setup or maintenance
refactor: improve code structure
docs: update documentation
```

---

## Academic Context

This project is part of a bachelor thesis investigating:

- Game-based learning
- Implicit language acquisition
- Player motivation through interaction and discovery

The implementation is guided by research in:

- Context-based learning
- Constructivist learning theory
- Motivation through gameplay systems

---

## Future Work

Upcoming development phases include:

- Filling out the core gameplay loop and scenario progression
- Adding richer vocabulary interactions and quest content
- Improving feedback, polish, and onboarding for language exposure
- Enhancing mobile controls, UX, and UI transitions
- Adding evaluation mechanics for language learning outcomes

---

## Notes

This README reflects the current prototype stage with playable systems and backend support. The project is still evolving, and the architecture is designed to accommodate future gameplay and learning feature growth.

---
