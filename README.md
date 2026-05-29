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

## Quick Start

### Backend

```bash
git clone <repo-url>  # if not already cloned
cd backend
npm install
npm run dev
```

### Godot

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
autoload/      → Global singleton systems and managers
resources/     → Lexicon database and game resources
scenes/        → Core scenes, UI scenes, and scenario layouts
scripts/       → C# game logic, UI controllers, systems, and save data
addons/        → mobile virtual joystick support
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
- **Backend Integration**: Express route at `/api/tts/generate`, Piper speech synthesis, and static file serving for cached audio

### Backend Flow

```text
[Godot TTSService] --> POST /api/tts/generate --> [Express backend]
[Express backend] --> [Piper TTS generator] --> [storage/tts/*.wav]
[Express backend] --> response { audioPath, fileName }
[Godot TTSService] --> download/cached audio from [storage/tts]
[AudioManager] --> play cached WAV
```

---

## External Dependencies

Piper binaries and language models are excluded from the repository and must be installed separately. This is only required for local Piper TTS and not for any online Piper setup.

---

## Architectural Approach

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
