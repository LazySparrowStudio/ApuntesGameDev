# 🎮 Unreal Engine: Parent Class Overview

When creating a new **Blueprint** or **C++ class** in Unreal Engine, you must choose a **parent class**. This determines what functionality your class will inherit. Here's a breakdown of the most commonly used parent classes in Unreal Engine.

---

## 🧱 Gameplay-Oriented Classes

### 1. **Actor**
- **Description**: Base class for all objects that can be placed in a level.
- **Features**:
  - Has transform (location, rotation, scale)
  - Can contain components (e.g., meshes, lights, etc.)
- **Use Cases**:
  - Static props
  - Interactive objects
  - Environmental items

---

### 2. **Pawn**
- **Description**: An `Actor` that can be controlled by a `Controller` (Player or AI).
- **Features**:
  - Possessable
  - Doesn’t have movement logic by default
- **Use Cases**:
  - Vehicles
  - Drones
  - Custom controllable entities

---

### 3. **Character**
- **Description**: A `Pawn` with built-in movement capabilities.
- **Features**:
  - Character Movement Component
  - Skeletal Mesh support
  - Animation Blueprint support
- **Use Cases**:
  - Player characters
  - AI enemies
  - Humanoid NPCs

---

### 4. **PlayerController**
- **Description**: Handles player input and interaction with the world.
- **Features**:
  - Controls Pawns/Characters
  - Manages UI interaction and camera control
- **Use Cases**:
  - Input logic separation
  - Camera control systems
  - Managing player-specific behavior

---

### 5. **GameModeBase / GameMode**
- **Description**: Controls the flow and rules of the game.
- **Features**:
  - Only runs on the server
  - Manages spawning and game rules
- **Use Cases**:
  - Match setup and teardown
  - Win/loss conditions
  - Default classes for players

---

### 6. **GameStateBase / GameState**
- **Description**: Tracks and replicates the game’s current state.
- **Features**:
  - Exists on both client and server
  - Good for multiplayer synchronization
- **Use Cases**:
  - Score tracking
  - Game timers
  - Global multiplayer state

---

### 7. **HUD**
- **Description**: Legacy 2D UI drawing class.
- **Features**:
  - Uses Canvas to draw directly on screen
- **Use Cases**:
  - Simple overlays (crosshairs, debug info)
  - Legacy projects (replaced by UMG)

---

### 8. **PlayerState**
- **Description**: Holds player-specific, persistent data.
- **Features**:
  - Survives player deaths
  - Replicates in multiplayer
- **Use Cases**:
  - Tracking score
  - Player stats (name, kills, etc.)

---

## ⚙️ Component and System Classes

### 9. **ActorComponent**
- **Description**: A reusable, logic-only component.
- **Features**:
  - No visual representation
  - Easily attachable to any actor
- **Use Cases**:
  - Health systems
  - Inventory management
  - Audio and trigger logic

---

### 10. **SceneComponent**
- **Description**: A component with a transform.
- **Features**:
  - Used as a base for other visual components
- **Use Cases**:
  - Custom positioning
  - Socket attachment

---

### 11. **StaticMeshActor**
- **Description**: Prebuilt Actor with a Static Mesh Component.
- **Use Cases**:
  - Simple placeable props
  - Lightweight environment elements

---

## 🖥 UI and Utility Classes

### 12. **WidgetBlueprint (`UserWidget`)**
- **Description**: UMG-based UI element.
- **Features**:
  - Designed in the UMG editor
  - Used for HUDs, menus, etc.
- **Use Cases**:
  - Health bars
  - Pause menus
  - Inventory interfaces

---

### 13. **AnimInstance**
- **Description**: Controls animation logic for Skeletal Meshes.
- **Features**:
  - State Machines
  - Blendspaces
- **Use Cases**:
  - Character animation
  - Weapon handling animations

---

### 14. **BlueprintFunctionLibrary**
- **Description**: A container for static, globally accessible functions.
- **Features**:
  - No instances created
  - Functions callable from anywhere
- **Use Cases**:
  - Math utilities
  - Reusable gameplay functions

---

## 🧠 Choosing the Right Parent Class

| Goal / Use Case                         | Recommended Parent Class       |
|----------------------------------------|-------------------------------|
| Placeable object with components       | `Actor`                      |
| Player/A.I.-controlled entity          | `Pawn` or `Character`        |
| Movement (walking, jumping, etc.)      | `Character`                  |
| User Interface                         | `UserWidget` (Widget Blueprint) |
| Game logic and flow                    | `GameModeBase`               |
| Multiplayer game state tracking        | `GameState`                  |
| Persistent player stats                | `PlayerState`                |
| Modular, reusable game logic           | `ActorComponent`             |
| Visual-only object                     | `StaticMeshActor`            |

---

## 📌 Notes

- Always pick the **smallest base class** that gives you the functionality you need. Don’t start with `Character` if you just need a prop.
- Components like `SceneComponent`, `StaticMeshComponent`, and `CameraComponent` are building blocks for more complex Actors.

---

