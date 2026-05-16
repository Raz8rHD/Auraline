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

In shaping the technical direction and sensory interaction of **Auraline**, the research framework was structured around a multi-disciplinary approach bridging first-party software engineering with procedural audio design, color psychology, and historic graphic synthesis frameworks.

### Architectural Design and Complexity Management
To ensure a clean, decoupled, and highly performant C# architecture, the project relied heavily on optimization blueprints aimed at reducing systemic complexity. Managing this data flow is a baseline necessity when mapping real-time, interactive 2D coordinates onto 3D surfaces while simultaneously driving dynamic middleware sound parameters. As industry systems designers emphasize, the failure to decouple graphic loops from audio execution pipelines leads to catastrophic performance bottlenecks. Technical author Robert Nystrom highlights this concept in *Game Programming Patterns*:

> "Decoupling components ensures that game systems can evolve and execute independently without rippling performance flaws across unrelated domains." [(Nystrom, 2014)](https://gameprogrammingpatterns.com/component.html)

By adhering to strictly modular engineering patterns, Auraline isolates its visual material calculations from its active audio buses, guaranteeing consistent runtime frames.

### Up-to-Date Engine and Middleware Integration
For platform-specific implementation, the project prioritized official, first-party documentation released directly by **Unity Technologies** and **Firelight Technologies** over unofficial, third-party tutorials. Because both game engines and audio middleware packages undergo rapid API transformations, older community guides frequently rely on deprecated lifecycle methods, tightly coupled references, and unoptimized logic. This issue is widely acknowledged within the development community, with engine programmers stating on community forums that:

> "Legacy, community-made integration tutorials are hopelessly broken due to massive core architecture rewrites, and authors rarely update their source material to reflect updated node or script behavior." [(Unity/FMOD Developer Forum, 2024)](https://qa.fmod.com/t/lots-of-warnings-in-unity-6-2/23701)

To protect the development workflow from console pollution and compile-time failures—such as the known FMOD `TreeView` deprecation warnings on newer engine versions—the project leaned strictly on official samples. This ensured that features like memory-safe coroutines and event-driven data tracking function seamlessly on a portable **macOS (MacBook Air)** development environment.

---

### Sources

#### 1. [FMOD Studio Unity Integration Documentation](https://www.fmod.com/docs/2.03/unity/api.html)
Published by **Firelight Technologies**, the creator of the industry-standard FMOD Studio audio middleware engine, this official scripting API reference guide is widely respected for its deep technical clarity and platform-agnostic stability. This source was critical to the architectural development of Auraline because it details the precise C# classes, methods, and memory structures required to instantiate runtime event instances, manipulate mixer properties, and pass data from Unity into dynamic audio buses safely.

* **Playback Tracking:** Analyzed the `FMOD.Studio.PLAYBACK_STATE` enumerations to track when a musical track is actively playing, paused, or stopped, allowing for perfect coordination with visual pad emissions.
* **Lifecycle Management:** Studied the `StudioEventEmitter` and native `EventInstance` lifecycle architectures to execute memory-safe audio triggers, effectively preventing audio voice leakage and resource over-allocation during continuous drawing loops.
* **Parameter Passing:** Evaluated real-time parameter-setting protocols like `setParameterByName` to establish a smooth pipeline where real-time coordinate math from drawing inputs translates seamlessly into fluid parameter changes without causing audible digital artifacts.

This documentation was exceptionally useful for establishing a performant and highly responsive connection between the visual interface and the background audio mix. However, a limitation within the FMOD Unity integration package was a substantial volume of deprecated `TreeView` and `TreeViewState` warnings (`CS0618`) generated within its custom editor files when compiling on newer engine versions. While these warnings do not disrupt runtime audio playback, they clutter the editor console and require manual filtering during development.

```
+--------------------------+                  +---------------------------+
|    UNITY EVENT LAYER     |                  |     FMOD MIXING BUS       |
|  2D Screen Coordinates   | --(C# API Link)->|   setParameterByName()    |
|  & Contact Capacitance   |                  |  Dynamic Parameter Curve  |
+--------------------------+                  +---------------------------+
```
*Figure 1. Event-driven data flow between Unity input and FMOD parameter modulation.*

#### 2. [Unity Advanced Programming Architecture Manual](https://unity.com/how-to/advanced-programming-and-code-architecture)
Published by **Unity Technologies**, this official platform documentation represents the absolute authority on the engine’s underlying lifecycle methods, rendering pipelines, and memory optimization guidelines. It was crucial to this project for establishing the programmatic foundations of the interactive drawing pad, controlling environmental parameters, and automating the local development environment.

* **Time Slicing & Coroutines:** Mastered the lifecycle of the `Coroutine` class and the use of `yield return new WaitForSeconds()` to decouple the 5-second theatrical "void" delay from the primary update loop, preventing frame stutter during startup.
* **Shader Property Modification:** Analyzed the `Material.SetColor` and `_EmissionColor` shader properties to modify real-time emission data on standard materials without creating a heavy memory footprint through duplicate texture instantiations.
* **Particle & Trail Geometry:** Studied the geometry pooling behaviors of the `TrailRenderer` component, which led to identifying the need for a structural "Pivot Container" hierarchy to align the trail generation precisely with the tip of the custom 3D pen model.
* **Editor Automation:** Investigated the `[InitializeOnLoad]` attribute and the `EditorApplication.delayCall` class to write an automated pipeline script that forces the project to always open to the Auraline pad scene across distributed version control setups.

The Unity documentation provided the absolute structural blueprint required to build optimized, decoupled C# scripts that bypass unnecessary engine overhead. Its only minor limitation is that its general UI and trail documentation assumes standard, HUD-heavy game setups, meaning that customizing these components for an unconventional, zero-HUD "drawing-to-sound" tool required significant algorithmic adaptation and custom mathematical overrides.

---

#### 3. [Tsugi DSP Action Product Suite](https://tsugi-studio.com/web/en/products-dspmotion.html)
Developed by **Tsugi Studio**, a Tokyo-based leader in procedural game audio tools, this professional sound design suite is highly respected for its ability to synthesize complex sound effects in real-time based on live user gestures. This product was uniquely valuable to Auraline because it provided a validated commercial proof-of-concept demonstrating how hand velocity, stroke angles, and coordinate vectors can act as fluid, expressive sound modifiers.

* **Gestural Interactivity:** Analyzed Tsugi's core workflow of utilizing a 2D "Sketch Pad" where the position, acceleration, and cross-line interactions of a drawing tool drive audio variations rather than triggering pre-recorded, repetitive wave samples.
* **Material Mapping:** Studied the integration pipeline of their procedural audio models, noting how they map specific material properties (such as glass, metal, and digital synths) directly to continuous structural modifiers.
* **Visual-Audio Cohesion:** Evaluated their method of maintaining real-time video and graphic synchronization to see how visual feedback loop designs can subconsciously enhance the feel of an auditory interface.

DSP Action was highly influential in defining Auraline’s user interaction loop. It proved that mapping user-drawn vector coordinates directly to real-time modulation curves results in a highly expressive and intuitive tool that prevents user ear fatigue. Observing Tsugi's implementation directly inspired Auraline's shift away from generic button-pushing interfaces toward a continuous, fluid drawing workspace that prioritizes physical motion.

![Screenshot](./ResearchPictures/Screenshot%202026-05-15%20at%2023.40.51.png)

*Figure 2. Tsugi DSP Motion user interface demonstrating the gestural drawing-to-sound workflow, where mouse or tablet vector inputs map directly to procedural audio synthesis and real-time visual synchronization.*

---

#### 4. [ZKM Karlsruhe: From Xenakis's UPIC to Graphic Notation Today](https://zkm.de/en/from-xenakiss-upic-to-graphic-notation-today)
Published by the **ZKM Center for Art and Media**, this academic compilation tracks the genesis and evolution of the **UPIC (Unité Polyagogique Informatique du CEMAMu)** system—a computational synthesis tool developed by the avant-garde composer and architect Iannis Xenakis in the late 1970s. This resource stands as the primary historical and artistic justification for Auraline, establishing a professional lineage for transforming hand-drawn geometric illustrations into audio waveforms.

* **Vector Composition:** Analyzed the historical technique of utilizing a digitizing tablet to translate structural drawing strokes directly into specific wave frequencies, pitches, and durations.
* **Macro-Form Notation:** Studied how architectural vector lines can be reimagined as macro-level musical scores, breaking down the rigid limits of traditional Western sheet music.
* **Menu-Less Frameworks:** Explored the user workflow of the UPIC, discovering that eliminating text menus entirely allows non-musicians and artists to immediately compose audio through pure spatial expression.

Reviewing this historical system was incredibly useful for validating the creative direction of Auraline, confirming that abstract visual contours possess an inherent, powerful relationship with musical arrangement. The primary limitation of this research source is that the original UPIC relied on massive, custom, legacy mainframe hardware of the late 20th century, meaning its historical documentation offers no direct modern software answers. The core engineering challenge was translating Xenakis's grand compositional philosophy into optimized, lightweight C# logic that runs natively on a modern MacBook Air.

---

#### 5. [Crossmodal Correspondences and Multi-Sensory Mapping](https://link.springer.com/article/10.3758/s13414-010-0073-7)
This highly cited peer-reviewed paper by Dr. Charles Spence, head of the Crossmodal Research Laboratory at the University of Oxford (*Attention, Perception, & Psychophysics*), provides an empirical cognitive psychology framework for cross-modal mapping. The research focuses on how the human brain naturally and consistently links distinct visual attributes (such as spatial height, brightness, and size) with specific auditory dimensions (such as pitch, loudness, and timbre). This academic source was essential for justifying Auraline's design choices, proving that the relationships between visual drawing coordinates and sound outputs are backed by universal human cognitive patterns rather than being completely arbitrary.

* **Spatial-Pitch Correspondences:** Analyzed the fundamental neurological pairing between vertical space and audio frequency, confirming that humans naturally expect higher positions on a 2D interface to generate higher musical pitches.
* **Brightness-Loudness Alignment:** Studied how visual intensity and color saturation map directly to acoustic volume and filter configurations, validating the synchronization of bright material emissions with intense audio dynamics.
* **Implicit UX Design Frameworks:** Investigated how leveraging pre-existing cross-modal connections reduces cognitive load, showing that building an interactive interface around natural sensory expectations removes the need for explicit text-based instructions or a cluttered HUD.

This research was incredibly valuable for calibrating the math behind Auraline's interactive drawing board, ensuring that data sent from Unity's 2D space maps to FMOD parameters in a way that feels instantly intuitive to the user. By basing the interaction logic on established cognitive science, the system achieves a true "Zero-UI" flow where the interface feels natural from the first stroke. The primary limitation of this paper is its focus on controlled laboratory testing rather than live digital tools, meaning its insights had to be manually translated into real-time C# scaling algorithms and continuous FMOD modulation curves.

---

## Implementation

### Visual Representaion Of Auraline

---

I kicked off development by bridging the gap between tactile performance and fluid, visual sound synthesis. The goal was to establish a clear visual blueprint before diving into Unity, resulting in the Auraline AP-10 concept mock-up. I have asked Gemini to generate a 2D model of a drum pad with a screen used for drawing and modifying sounds. This was the result:

![Picture](./DevelopmentPictures/Gemini_Generated_Image_3qjcci3qjcci3qjc-2.png)

*Figure 3. Auraline AP-10 user interface demonstrating the hybrid tactile triggering and gestural drawing-to-sound workflow, where vector-based screen inputs map directly to procedural modulation envelopes and real-time visual spectrum synchronization.*

This high-fidelity mock-up serves as my definitive UI anchor, defining the spatial layout, color-coded parameter feedback, and asset requirements before turning the canvas into a 3D model in Unity.

---

### Using Pro Builder

While Unity’s primitive 3D shapes worked for a basic block-out, they quickly became a bottleneck for the **Auraline** chassis. Fabricating the recessed screen, sloped panel indents, and edge bevels by nesting and scaling dozens of separate primitive GameObjects cluttered the hierarchy and restricted our texture mapping.

Switching to **ProBuilder** directly within Unity streamlined the pipeline:

* **In-Editor Modeling:** I will be able to sculpt the custom hardware casing and screen cavities directly in the scene view, avoiding the friction of shifting to external 3D software.
* **Optimized Meshes:** ProBuilder combines complex geometry into a single, clean mesh, drastically reducing GameObject bloat.
* **Granular UV Control:** This was crucial for mapping the detailed interface textures onto the hardware precisely without stretching.

Transitioning to ProBuilder allowed me to quickly iterate on the physical form of the drum pad while keeping the project structure clean and optimized. Through images, below I will display the process of modelling the interface.

![Picture](./DevelopmentPictures/unnamed.jpg)

*Figure 4. Using ProBuilder to define the initial 3D slab for the chassis, establishing the hardware footprint and edge profile directly in the Unity scene view.*

---

![Picture](./DevelopmentPictures/buttonsadded.png)

*Figure 5. Populating the right side of the interface with a precise 4x5 grid array of physical performance pads, establishing the tactile control zone.*

---

![Picture](./DevelopmentPictures/buttonsglowing.jpg)

*Figure 6. Testing a high-intensity magenta emissive material on a single pad to calibrate baseline bloom, lighting behavior, and visual feedback constraints.*

---

![Picture](./DevelopmentPictures/chasis.jpg)

*Figure 7. Expanding the emissive material passes across the entire grid, implementing color-coded rows to visually categorize instrument groups and parameter states.*

---

![Picture](./DevelopmentPictures/screen.png)

*Figure 8. Utilizing ProBuilder's face editing to extrude and bevel the left side of the chassis inward, creating the finalized, matte hardware cavity dedicated to the drawing interface.*

---

## FMOD Implementation In Unity
