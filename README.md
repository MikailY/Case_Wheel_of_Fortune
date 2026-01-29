# 🎡 Wheel of Fortune - Game Developer Demo

A high-performance, scalable "Wheel of Fortune" game built in Unity 2021 LTS. This project was developed as a technical demo, focusing on clean code (SOLID), reusable UI architecture, and dynamic game configurations.

---

### 🎮 Gameplay Demonstration

<p align="center"> 
  <a href="https://www.youtube.com/watch?v=ey8qD7ySacQ"> 
    <img src="https://img.youtube.com/vi/ey8qD7ySacQ/maxresdefault.jpg" alt="Watch Gameplay" width="600px">
  </a>

<em>Click above to watch the gameplay video</em> </p>

---

### 📋 Project Overview
The game is a "risk vs. reward" gambling mechanic where players spin wheels to collect rewards while avoiding the bomb.

* **Core Loop:** Spin → Collect or Lose All → Decide to Cash Out or Risk More.
* **Dynamic Zones**
  * **Standard Zones:** High risk with a bomb slice.
  * **Safe Zones (Every 5th):** Risk-free silver spin.
  * **Super Zones (Every 30th):** Risk-free golden spin with premium rewards.

---

### 🛠️ Technical Implementation (The "Conditions")
I have strictly followed the technical requirements provided in the brief:

* **Clean Code & Architecture**
  * Implemented using **SOLID** principles and **OOP** concepts.
  * **Scriptable Objects** used for wheel configurations (stages, rewards, configs).
  * **DOTween** utilized for smooth, performance-friendly UI animations.

* **UI Technical Details**
  * **Canvas:** Set to Expand mode with correct anchor/pivot settings for 20:9, 16:9, and 4:3 support.
  * **Optimized UI:** "Raycast Target" and "Maskable" disabled on non-interactive elements to save draw calls.
  * **Automation:** Button references are set automatically via OnValidate (No manual drag-and-drop in Editor).
  * **Naming Convention:** Followed the ui_image_specifier and _value suffix rules.
  * **TextMeshPro:** Used for all text elements.
    
---

### 📸 Multi-Aspect Ratio Screenshots
The UI is designed to be lean and compatible with different screen sizes:

<table style="width: 100%; border-collapse: collapse; border: none;">
  <tr>
    <td align="center" style="border: none;">
      <img width="100%" alt="Resolution 16:9" src="https://github.com/user-attachments/assets/5314396a-aea5-48d2-af79-8f7d352222ce" /><br />
      <sub><b>Resolution 16:9</b></sub>
    </td>
    <td align="center" style="border: none;">
      <img width="100%" alt="Resolution 20:9" src="https://github.com/user-attachments/assets/a2311454-60e8-462b-9b0f-563ebcaf611e" ><br/>
      <sub><b>Resolution 20:9</b></sub>
    </td>
    <td align="center" style="border: none;">
      <img width="100%" alt="Resolution 4:3" src="https://github.com/user-attachments/assets/0188e11c-a409-4644-a668-d708fe1cbd11" ><br/>
      <sub><b>Resolution 4:3</b></sub>
    </td>
  </tr>
</table>

---

### 📦 Deliverables

* **Source Code:** Available in this repository.
* **Android Build:** [Download Latest APK (Release)](https://github.com/MikailY/Case_Wheel_of_Fortune/releases)

---

### ⚙️ How to Run

1. Clone this repository.
2. Open the project in Unity 2021.3 LTS.
3. Load the SampleScene located in Assets/Scenes/.
4. Press Play or build for Android to test.







