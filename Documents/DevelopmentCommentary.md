# Final Major Project
## Project Outline
---
**Auraline** is an interactive audio-visual synthesis application built in **Unity** that bridges the gap between digital sound design and physical gesture. The project explores the intersection of psychology, visual art, and audio perception, focusing on how human beings process and interact with multisensory stimuli. While stylistically and technically inspired by historical systems like the [**Xenakis UPIC**](https://en.wikipedia.org/wiki/UPIC) and the [**Oramics Machine**](https://en.wikipedia.org/wiki/Oramics), the application eschews traditional, button-heavy interfaces in favor of a clean, gesture-based "drawing-to-sound" experience driven by the **FMOD** audio engine. A core feature is the implementation of a theatrical "Power-Up" sequence and reactive material emissions that provide immediate visual feedback for real-time audio modulation.

For instance, the application utilizes a 2D interactive drum pad and a **"Ghost Pen"** tutorial system to guide users through the synthesis process. The pen leaves a dynamic trail as it follows a procedural mathematical path—specifically x=sin(time⋅speed)⋅width and y=cos(time⋅speed⋅0.5)⋅height—teaching the user the mechanics of the interface before they begin their own performance. Importantly, the system utilizes sequential visual "breadcrumbs," where elements like the **Reset** and **Next Track** buttons only fade into existence as the user requires them, prioritizing an immersive, minimal interface over traditional HUD-heavy layouts.

---

### Minimal Goal - Decent Audio-Visual Mechanic

The visual-audio interface is the core for **Auraline** and the central focus of development. The minimum standard of delivery for the project is a good interactive interface that modifies the audio based on the user's mouse motion (drawings).

#### Key Objectives:
- Compact design with simplistic visual instructions to keep optimal engagement.
- Self-produced audio by exploring different genres and sampling techinques.
- Implement a responsive interface that binds drawing and audio modifications flawlessly through drawings.

#### Anticipated Challenges:
- Designing a 3D model of an interface that is not too confusing or too minimalistic.
- Producing and arranging melodies could take a good portion of my time and slowing down development.
- Fluid responses between FMOD events and Unity through coding that detects the mouse moves and responds accordingly.

---

### Desired Goal - Polished Interface And Multiple Audios

A visually appealing interface is the second goal for this project, intended to provide a decent variety of buttons that turn on or off audio effects. This will allow the user to modify the sound as he pleases. Integrating multiple songs would give the opportunity to choose the most outstanding one while keeping gameplay at optimal levels.

#### Key Objectives:
- Design buttons in a way that encourages engagement, maybe through emmissive materials to draw visual attention from the user.
- Integrate two or three more melodies with transitions to keep the playable loop consistent and not lose the user's attention span.
- Colourful drawing line to bind the visual and audio stimulus and turn a passive action (listening to music) into an active one.

#### Anticipated Challenges:
- Not overflowing the interface with unneccessary buttons that will draw the user's attention from the main screen, removing completly the purpose of the project.
- Without a designated room and equipment, producing might be difficult due to me only having a laptop and no MIDI or keyboard, resulting in a sloppy and simple production process that is not up to the industry standards.
- Missing a proper allignment of many game objects in Unity will result in errors and the designated screen might not detect the drawing line or the movements as intended.

---

### Aspirational Goal - Fully Developed Project

Achieving a finalised product to the highest standard, both mechanically and visually, is the foundational requirement for establishing a seamless, uncompromised link between human gesture and real-time audio synthesis.

#### Key Objectives:
- A well designed and quick responsive screen for drawing, simple visual cues and instructions for the user, only neccesary pads for audio effects, appealing 3D model.
- Multiple songs added with a good amount of audio parameters that are linked to Unity and allows the user to explore different outcomes of how visuals and audios are linked.
- Creating a highly optimized, lightweight rendering pipeline that guarantees a rock-solid, high-framerate experience on portable hardware like a MacBook Air, supported by modular C# code that allows developer-friendly expansions for future audio tracks and pad layouts.

### Anticipated Challenges:
- Mapping 2D inputs onto a 3D console surface with zero latency, while relying entirely on visual cues to preserve a precise, text-free minimalist aesthetic.
- Synchronizing complex Unity-FMOD parameter mappings across multiple tracks without bottlenecks, ensuring visual inputs consistently drive predictable, meaningful audio changes.
- Preventing frame-drops and memory fragmentation on a MacBook Air from real-time line drawing and emission overrides, requiring optimized, decoupled C# scripts for scalable performance.

---

## Research

### Methodology
