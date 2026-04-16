# EhB-FinalWork-LingoRise

## Overview

This project is a bachelor-level game development assignment focused on exploring **implicit language learning through gameplay**. Instead of relying on traditional methods such as drills or explicit instruction, the game aims to create a **context-driven learning experience** where players acquire language naturally through interaction, repetition, and discovery.

The project is being developed using:

* **Godot 4 (C#)**
* **Node.js (planned backend)**
* **MongoDB (planned data storage)**

---

## Current Project Status

This repository currently contains the **initial project setup and architecture foundation**. At this stage, the focus is on establishing a clean, scalable structure that supports:

* Iterative gameplay development
* Modular system design
* Maintainable and testable code
* Alignment with academic requirements

Implemented so far:

* Base folder structure
* Core scene (`Main.tscn`)
* Initial UI scene (`MainMenu.tscn`)
* Global `GameManager` (autoload)
* Mobile portrait configuration (1080x1920)

No gameplay systems or mechanics have been implemented yet.

---

## Project Structure

```
assets/        → Game assets (art, audio, etc.)
autoload/      → Global singleton systems (e.g., GameManager)
resources/     → Data files and configurations

scenes/
├── core/      → Core game flow scenes (entry point, loaders)
├── ui/        → UI scenes (menus, HUD)
└── gameplay/  → Gameplay scenes (to be implemented)

scripts/
├── core/      → Core application logic (game flow, management)
├── systems/   → Reusable systems (input, audio, save, etc.)
├── ui/        → UI-related logic
├── gameplay/  → Gameplay mechanics and behaviors
└── utilities/ → Helper functions and extensions
```

---

## Architectural Approach

The project follows a **modular and scalable architecture** to support frequent iteration and evolving gameplay requirements.

Key principles:

* **Separation of concerns** (UI, gameplay, systems, core)
* **Event-driven communication** (to reduce tight coupling)
* **System-based design** (reusable and independent modules)
* **Scene decoupling** (logic not directly embedded in scenes)

This ensures that gameplay features can be modified or replaced without affecting the overall system stability.

---

## Development Workflow

Version control follows a structured Git workflow:

* `main` → stable builds
* `develop` → integration branch
* `feature/*` → individual features

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

* Game-based learning
* Implicit language acquisition
* Player motivation through interaction and discovery

The implementation is guided by prior research, including:

* Context-based learning approaches
* Constructivist learning theory
* Motivation through gameplay systems

The architecture is intentionally designed to support experimentation and iteration, which are essential for validating these concepts.

---

## Future Work

Upcoming development phases include:

* Scene management system
* Core gameplay loop implementation
* Interaction and feedback systems
* Progress tracking and evaluation mechanics
* UI/UX refinement for mobile experience

---

## Notes

This README represents an **early-stage snapshot** of the project.
Both the gameplay and structure may evolve significantly as development progresses.

The architecture is designed to accommodate these changes without requiring major refactoring.

---
