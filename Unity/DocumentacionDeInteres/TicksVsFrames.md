# 🎮 Ticks vs Frames in Game Development (Unity-Focused)

## 🧠 What Are Ticks?
- A **tick** is a **game logic update**.
- Handles:
  - Physics
  - AI
  - Movement
  - Collision
- Runs at a **fixed rate** (e.g., 60 ticks per second).
- Ensures consistent gameplay regardless of rendering speed.

> Think of ticks as the **brain** of the game.

---

## 👁 What Are Frames?
- A **frame** is a **rendering update** (visual output).
- Handles:
  - Drawing to screen
  - Animations
  - UI updates
- Tied to GPU performance; typically variable (30, 60, 144 FPS, etc.).

> Think of frames as the **eyes** of the game.

---

## 🔁 Why Separate Ticks from Frames?
- Keeps **game logic consistent** even if FPS drops.
- Allows smoother animation via **interpolation**.
- Prevents physics and gameplay bugs due to frame-dependent logic.

---

## 🎛 VSync and Its Role
- **VSync** syncs frame rendering with monitor refresh rate (e.g. 60Hz).
- Prevents **screen tearing**.
- Affects **frames only**, **not ticks**.

| Component | Description                | Affected by VSync? |
|-----------|----------------------------|---------------------|
| Ticks     | Game logic updates         | ❌ No               |
| Frames    | Visual screen updates      | ✅ Yes              |

---

## 🧩 Unity-Specific Behavior

### ✅ `FixedUpdate()`
- Unity’s equivalent of **ticks**.
- Runs at a **fixed interval** (default: 0.02s or 50 times/sec).
- Best for:
  - Physics
  - Consistent time-sensitive logic
- Not affected by frame rate.

### ✅ `Update()`
- Runs **once per frame**.
- Time step is **variable**.
- Best for:
  - Input
  - Non-physics movement
  - General gameplay logic

> Multiply by `Time.deltaTime` to make behavior **frame-rate independent**.

### ✅ `LateUpdate()`
- Runs after `Update()`.
- Good for:
  - Following camera movement
  - Post-processing updates

---

## ⏱ Time.deltaTime in `Update()`

Using `Time.deltaTime` **does not convert frames to ticks**, but:

> 🎯 It allows frame-dependent updates to behave **like** they run at a fixed rate.

### Example:
```csharp
void Update() {
    transform.position += Vector3.forward * speed * Time.deltaTime;
}
```

## ✅ Summary (Unity vs Unreal Engine)

| Concept       | Description                             | Unity Equivalent                 | Unreal Engine Equivalent                                         |
|---------------|-----------------------------------------|----------------------------------|------------------------------------------------------------------|
| **Ticks**     | Fixed-rate game logic updates           | `FixedUpdate()`                  | `Tick()` with fixed timestep (via `UWorld::SetFixedDeltaTime`)  |
| **Frames**    | Variable-rate visual updates            | `Update()` / `LateUpdate()`      | `Tick()` (default behavior, runs every frame)                   |
| **VSync**     | Syncs frames with monitor refresh rate  | Affects rendering in `Update()`  | Affects rendering pipeline (configurable in Project Settings)   |
| **deltaTime** | Time delta for smoothing updates        | `Time.deltaTime`                 | `DeltaSeconds` parameter in `Tick()`                            |

