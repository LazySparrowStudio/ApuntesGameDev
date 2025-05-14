
# 🧠 Rookie Unity Practices to Avoid in Bigger Projects (and What to Do Instead)

Improve the maintainability, scalability, and performance of your Unity projects by steering clear of these beginner mistakes:

---

## 1. 🔓 Using `public` Fields for Everything

**❌ Why it's bad:**  
Breaks encapsulation and exposes internals unnecessarily.

**✅ Do this instead:**  
Use `[SerializeField] private` to keep variables editable in the Inspector but private to other scripts.

```csharp
[SerializeField] private float speed = 5f;
```

---

## 2. 🧮 Hardcoding Values

**❌ Why it's bad:**  
Requires code changes for minor gameplay tweaks.

**✅ Do this instead:**  
Expose values in the Inspector or use ScriptableObjects for reusable config data.

---

## 3. 🐌 Calling `Find()` or `GetComponent()` in `Update()`

**❌ Why it's bad:**  
These methods are expensive if called every frame.

**✅ Do this instead:**  
Cache references in `Awake()` or `Start()`.

```csharp
private Rigidbody rb;

void Awake() {
    rb = GetComponent<Rigidbody>();
}
```

---

## 4. 🔁 Overusing `Update()`

**❌ Why it's bad:**  
Kills performance with unnecessary logic checks.

**✅ Do this instead:**  
Use `Invoke`, coroutines, or Unity events (e.g., `OnTriggerEnter`, `OnEnable`).

---

## 5. 🔗 Tightly Coupling Scripts

**❌ Why it's bad:**  
Small changes ripple across multiple scripts.

**✅ Do this instead:**  
Use interfaces, delegates/events, or ScriptableObject-based architecture to decouple logic.

---

## 6. 🕒 Ignoring Script Execution Order

**❌ Why it's bad:**  
Causes bugs due to inconsistent initialization timing.

**✅ Do this instead:**  
Use Unity’s **Script Execution Order** settings or explicitly manage initialization steps.

---

## 7. 🏷️ Relying on Tags and Object Names

**❌ Why it's bad:**  
Error-prone due to typos or renamed objects.

**✅ Do this instead:**  
Use serialized references (drag-and-drop in Inspector) or robust referencing systems.

---

## 8. 📜 Using Magic Strings

**❌ Why it's bad:**  
No compile-time checks. Typos cause silent bugs.

**✅ Do this instead:**  
Use `const string`, enums, or `nameof()` for safe references.

---

## 9. 🧩 Not Using Prefabs Properly

**❌ Why it's bad:**  
Manual duplication leads to inconsistencies and inefficiency.

**✅ Do this instead:**  
Use prefabs and prefab variants. Use overrides to manage differences cleanly.

---

## 10. 🏗️ Everything in One Script

**❌ Why it's bad:**  
Massive scripts are hard to debug and maintain.

**✅ Do this instead:**  
Follow the **Single Responsibility Principle**. Break logic into small, focused components.

---

## 11. 🧾 Not Using Version Control

**❌ Why it's bad:**  
No history or backup. Easy to lose progress.

**✅ Do this instead:**  
Use **Git**, with a Unity `.gitignore`. Use Git LFS for large binary assets.

---

## 12. 🔍 Ignoring Physics Layers and Masks

**❌ Why it's bad:**  
Unnecessary collision checks hurt performance.

**✅ Do this instead:**  
Use Unity's **Layer Collision Matrix** to fine-tune which layers interact.

---

## 13. 📂 Poor Scene Management

**❌ Why it's bad:**  
Scenes missing from Build Settings won’t load at runtime.

**✅ Do this instead:**  
Maintain the **Build Settings** list. Use `SceneManager.LoadScene()` with valid names or indices.

---

## 14. 📦 No Separation Between Code and Data

**❌ Why it's bad:**  
Rebuilding code for every data change is inefficient.

**✅ Do this instead:**  
Use **ScriptableObjects** for data containers like player stats, item configs, etc.

---

## 15. 🧪 Not Profiling or Testing on Target Devices

**❌ Why it's bad:**  
Game runs fine in the Editor, but lags or breaks on actual devices.

**✅ Do this instead:**  
Test builds frequently. Use the **Unity Profiler**, **Frame Debugger**, and test hardware.

---

## ✨ Final Tip

Build with **scalability** in mind, even for small projects. Good habits early on save massive headaches down the road.

---
