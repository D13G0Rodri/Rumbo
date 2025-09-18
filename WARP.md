# WARP.md

This file provides guidance to WARP (warp.dev) when working with code in this repository.

## Project Overview

**Rumbo** is a Unity 2D life simulation game about life decisions and stages. The game follows a character through different life stages (Baby, Child, Teen, Adult) with progression mechanics, save/load functionality, and decision-making scenarios.

## Development Commands

### Unity Development
```powershell
# Open the project in Unity Editor (Unity 6000.0.39f1)
# The Unity project is located in the current directory

# Build the project (from Unity Editor)
# File > Build Settings > Build

# Run tests in Unity
# Window > General > Test Runner
# Or use Unity Test Framework via Package Manager
```

### C# Development
```powershell
# Open in Visual Studio or JetBrains Rider
dotnet sln Rumbo.sln

# Build solution
dotnet build Rumbo.sln

# Run unit tests (via Test Explorer in IDE)
# Tests are located in Assets/Tests/EditMode/
```

### Version Control
```powershell
# The project uses standard Unity .gitignore patterns
# Key directories ignored: Library/, Logs/, obj/, Temp/, UserSettings/
```

## Architecture Overview

### Core Systems

#### Player Controller Hierarchy
- **`PlayerControllerBase`**: Abstract base class handling core mechanics (movement, health, save/load, stats)
- **Stage-specific controllers**: `PlayerControllerBaby`, `PlayerControllerChild`, `PlayerControllerTeen` extend base functionality
- **Key Stats**: Health, Intelligence, Concentration, Hunger, Bathroom needs, Timer progression

#### Save System Architecture
- **`SaveSystem`**: Static class managing JSON serialization to persistent data path
- **`PlayerData`**: Serializable data container with support for custom dictionary serialization
- **Features**: Position persistence, pushable object state, achievement tracking, stage progression
- **Save Types**: 
  - `SaveGame()`: Full progression save for stage transitions
  - `SaveCheckpoint()`: Quick save for current progress

#### Game Progression System
- **`TimerVida`**: Core life timer mechanic (60 seconds per stage)
- **`cambiarEtapas`**: Handles video transitions between life stages
- **Scene progression**: Baby → Child → Teen → Adult stages with automatic video transitions

#### UI and Audio Management
- **Audio Managers**: Separate systems for music (`AudioManagerMusic`) and SFX (`AudioManagerSFX`)
- **Menu Systems**: Main menu, pause menu, options with proper scene management
- **UI Effects**: Animated text components (`LetraTemblorosa`, `OlaTexto`, `TextoParpadeo`)

### Key Dependencies (Package Manager)
- **Cinemachine 3.1.4**: Camera system management
- **Input System 1.13.0**: Modern input handling via `InputSystem_Actions.inputactions`
- **Universal Render Pipeline 17.0.3**: 2D rendering pipeline
- **Test Framework 1.4.6**: Unit testing support

### Scene Structure
Based on code analysis, the game progresses through these scene sequences:
- Main Menu → Presentation → Birth Transition → Stage scenes (Etapa-Niño, Etapa-adolescente, Etapa-adulto) → Death/End scenes

### Development Patterns

#### Inheritance Pattern
The codebase uses a clear inheritance hierarchy where `PlayerControllerBase` provides common functionality and stage-specific controllers override virtual methods for specialized behavior.

#### Event-Driven Save/Load
Uses C# events (`OnGameDataLoaded`) to coordinate between systems when game data is loaded, ensuring proper synchronization between player state and UI elements.

#### Singleton Pattern
`GameMemory` uses singleton pattern for persistent data across scene transitions.

### Testing
- Unit tests are configured in `Assets/Tests/` with assembly definition
- Test framework configured for EditMode tests
- Example test structure in `PlayerTests.cs` demonstrates basic unit testing patterns

### VSCode Integration
- Configured for Unity development with proper file exclusions
- Solution file (`Rumbo.sln`) integration
- Debug configuration for "Attach to Unity" workflow

## Key Development Notes

### Save Data Management
The save system uses JSON serialization and custom dictionary serialization for complex data structures. When modifying `PlayerData`, ensure backward compatibility or provide migration logic.

### Stage Transitions
Video-based transitions between life stages are handled automatically. The `cambiarEtapas` script manages video playback and scene switching.

### Stats System
Player stats (hunger, bathroom, health) update continuously via `Time.deltaTime`. New stats should follow this pattern for consistent progression.

### Object Persistence
Pushable objects use `objectId` strings for save/load state persistence. Ensure unique IDs when adding new interactive objects.