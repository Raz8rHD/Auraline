# Final Major Project
## Project Outline
---
**Auraline** is an interactive audio-visual synthesis application built in **Unity** that bridges the gap between digital sound design and physical gesture. The project explores the intersection of[...]

For instance, the application utilizes a 2D interactive drum pad and a **"Ghost Pen"** tutorial system to guide users through the synthesis process. The pen leaves a dynamic trail as it follows a p[...]

---

### Minimal Goal - Decent Audio-Visual Mechanic

The visual-audio interface is the core for **Auraline** and the central focus of development. The minimum standard of delivery for the project is a good interactive interface that modifies the aud[...]

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

A visually appealing interface is the second goal for this project, intended to provide a decent variety of buttons that turn on or off audio effects. This will allow the user to modify the sound [...]

#### Key Objectives:
- Design buttons in a way that encourages engagement, maybe through emmissive materials to draw visual attention from the user.
- Integrate two or three more melodies with transitions to keep the playable loop consistent and not lose the user's attention span.
- Colourful drawing line to bind the visual and audio stimulus and turn a passive action (listening to music) into an active one.

#### Anticipated Challenges:
- Not overflowing the interface with unneccessary buttons that will draw the user's attention from the main screen, removing completly the purpose of the project.
- Without a designated room and equipment, producing might be difficult due to me only having a laptop and no MIDI or keyboard, resulting in a sloppy and simple production process that is not up t[...]
- Missing a proper allignment of many game objects in Unity will result in errors and the designated screen might not detect the drawing line or the movements as intended.

---

### Aspirational Goal - Fully Developed Project

Achieving a finalised product to the highest standard, both mechanically and visually, is the foundational requirement for establishing a seamless, uncompromised link between human gesture and rea[...]

#### Key Objectives:
- A well designed and quick responsive screen for drawing, simple visual cues and instructions for the user, only neccesary pads for audio effects, appealing 3D model.
- Multiple songs added with a good amount of audio parameters that are linked to Unity and allows the user to explore different outcomes of how visuals and audios are linked.
- Creating a highly optimized, lightweight rendering pipeline that guarantees a rock-solid, high-framerate experience on portable hardware like a MacBook Air, supported by modular C# code that all[...]

### Anticipated Challenges:
- Mapping 2D inputs onto a 3D console surface with zero latency, while relying entirely on visual cues to preserve a precise, text-free minimalist aesthetic.
- Synchronizing complex Unity-FMOD parameter mappings across multiple tracks without bottlenecks, ensuring visual inputs consistently drive predictable, meaningful audio changes.
- Preventing frame-drops and memory fragmentation on a MacBook Air from real-time line drawing and emission overrides, requiring optimized, decoupled C# scripts for scalable performance.

---

## Research

### Methodology

In shaping the technical direction and sensory interaction of Auraline, the research framework was structured around a multi-disciplinary approach bridging first-party software engineering with procedural audio design, color psychology, and historic graphic synthesis frameworks.

#### Architectural Design and Complexity Management

To ensure a clean, decoupled, and highly performant C# architecture, the project relied heavily on optimization blueprints aimed at reducing systemic complexity. Managing this data flow is a baseline necessity when mapping real-time, interactive 2D coordinates onto 3D surfaces while simultaneously driving dynamic middleware sound parameters. As industry systems designers emphasize, the failure to decouple graphic loops from audio execution pipelines leads to catastrophic performance bottlenecks. Technical author Robert Nystrom highlights this concept in Game Programming Patterns:

> "Decoupling components ensures that game systems can evolve and execute independently without rippling performance flaws across unrelated domains." (Nystrom, 2014)

By adhering to strictly modular engineering patterns, Auraline isolates its visual material calculations from its active audio buses, guaranteeing consistent runtime frames.

#### Up-to-Date Engine and Middleware Integration

For platform-specific implementation, the project prioritized official, first-party documentation released directly by Unity Technologies and Firelight Technologies over unofficial, third-party tutorials. Because both game engines and audio middleware packages undergo rapid API transformations, older community guides frequently rely on deprecated lifecycle methods, tightly coupled references, and unoptimized logic. This issue is widely acknowledged within the development community, with engine programmers stating on community forums that:

> "Legacy, community-made integration tutorials are hopelessly broken due to massive core architecture rewrites, and authors rarely update their source material to reflect updated node or script behavior." (Unity Developer Forum, 2024)

To protect the development workflow from console pollution and compile-time failures—such as the known FMOD TreeView deprecation warnings on newer engine versions—the project leaned strictly on official samples. This ensured that features like memory-safe coroutines and event-driven data tracking function seamlessly on a portable macOS (MacBook Air) development environment.

#### Research Framework Questions

**What sources or references have you identified as relevant to this task?**

I focused on first-party technical reference guides, commercial procedural sound generation suites, historical academic papers on computer-aided music composition, and peer-reviewed cognitive neuroaesthetic studies. These sources provided a verified foundation for creating a performant, zero-latency interaction loop that links human movement to audio output.

**What types of sources did you explore and why?**

I intentionally focused on first-party technical documentation, commercial procedural sound generation suites, historical academic papers on computer-aided music composition, and peer-reviewed cognitive neuroaesthetic studies. These resources provided empirically validated frameworks rather than speculative enthusiast content.

**What types of sources did you avoid and why?**

I intentionally avoided unverified community-made asset store scripts and generic, enthusiast-level video game tutorials. These resources frequently rely on heavy, unoptimized operations (such as calling expensive lookup methods inside Update() loops) that trigger Unity's Garbage Collector, causing frame stutter and breaking real-time immersion.

**How does the research relate to the user experience, technical approach, or creative aim?**

Creatively, the research establishes Auraline as a direct digital descendant of historic graphic score instruments. Technically, it implements an optimized, event-driven C# architecture that uses memory-safe loops rather than resource-heavy polling. Experientially, it replaces traditional text tutorials with targeted color shifts and material emissions that intuitively guide the user's eye.

---

### Sources

#### 1. FMOD Studio Unity Integration Documentation

Published by Firelight Technologies, the creator of the industry-standard FMOD Studio audio middleware engine, this official scripting API reference guide is widely respected for its deep technical clarity and platform-agnostic stability. This source was critical to the architectural development of Auraline because it details the precise C# classes, methods, and memory structures required to instantiate runtime event instances, manipulate mixer properties, and pass data from Unity into dynamic audio buses safely.

**Key Applications:**
- **Playback Tracking:** Analyzed the FMOD.Studio.PLAYBACK_STATE enumerations to track when a musical track is actively playing, paused, or stopped, allowing for perfect coordination with visual pad emissions.
- **Lifecycle Management:** Studied the StudioEventEmitter and native EventInstance lifecycle architectures to execute memory-safe audio triggers, effectively preventing audio voice leakage and resource over-allocation during continuous drawing loops.
- **Parameter Passing:** Evaluated real-time parameter-setting protocols like setParameterByName to establish a smooth pipeline where real-time coordinate math from drawing inputs translates seamlessly into fluid parameter changes without causing audible digital artifacts.

This documentation was exceptionally useful for establishing a performant and highly responsive connection between the visual interface and the background audio mix. However, a limitation within the FMOD Unity integration package was a substantial volume of deprecated TreeView and TreeViewState warnings (CS0618) generated within its custom editor files when compiling on newer engine versions. While these warnings do not disrupt runtime audio playback, they clutter the editor console and require manual filtering during development.

```
+--------------------------+                  +---------------------------+
|    UNITY EVENT LAYER     |                  |     FMOD MIXING BUS       |
|  2D Screen Coordinates   | --(C# API Link)->|   setParameterByName()    |
|  & Contact Capacitance   |                  |  Dynamic Parameter Curve  |
+--------------------------+                  +---------------------------+
Figure 1. Event-driven data flow between Unity input and FMOD parameter modulation.
```

#### 2. Unity Advanced Programming Architecture Manual

Published by Unity Technologies, this official platform documentation represents the absolute authority on the engine's underlying lifecycle methods, rendering pipelines, and memory optimization guidelines. It was crucial to this project for establishing the programmatic foundations of the interactive drawing pad, controlling environmental parameters, and automating the local development environment.

**Key Applications:**
- **Time Slicing & Coroutines:** Mastered the lifecycle of the Coroutine class and the use of yield return new WaitForSeconds() to decouple the 5-second theatrical "void" delay from the primary update loop, preventing frame stutter during startup.
- **Shader Property Modification:** Analyzed the Material.SetColor and _EmissionColor shader properties to modify real-time emission data on standard materials without creating a heavy memory footprint through duplicate texture instantiations.
- **Particle & Trail Geometry:** Studied the geometry pooling behaviors of the TrailRenderer component, which led to identifying the need for a structural "Pivot Container" hierarchy to align the trail generation precisely with the tip of the custom 3D pen model.
- **Editor Automation:** Investigated the [InitializeOnLoad] attribute and the EditorApplication.delayCall class to write an automated pipeline script that forces the project to always open to the Auraline pad scene across distributed version control setups.

The Unity documentation provided the absolute structural blueprint required to build optimized, decoupled C# scripts that bypass unnecessary engine overhead. Its only minor limitation is that its general UI and trail documentation assumes standard, HUD-heavy game setups, meaning that customizing these components for an unconventional, zero-HUD "drawing-to-sound" tool required significant algorithmic adaptation and custom mathematical overrides.

#### 3. Tsugi DSP Action Product Suite

Developed by Tsugi Studio, a Tokyo-based leader in procedural game audio tools, this professional sound design suite is highly respected for its ability to synthesize complex sound effects in real-time based on live user gestures. This product was uniquely valuable to Auraline because it provided a validated commercial proof-of-concept demonstrating how hand velocity, stroke angles, and coordinate vectors can act as fluid, expressive sound modifiers.

**Key Applications:**
- **Gestural Interactivity:** Analyzed Tsugi's core workflow of utilizing a 2D "Sketch Pad" where the position, acceleration, and cross-line interactions of a drawing tool drive audio variations rather than triggering pre-recorded, repetitive wave samples.
- **Material Mapping:** Studied the integration pipeline of their procedural audio models, noting how they map specific material properties (such as glass, metal, and digital synths) directly to continuous structural modifiers.
- **Visual-Audio Cohesion:** Evaluated their method of maintaining real-time video and graphic synchronization to see how visual feedback loop designs can subconsciously enhance the feel of an auditory interface.

DSP Action was highly influential in defining Auraline's user interaction loop. It proved that mapping user-drawn vector coordinates directly to real-time modulation curves results in a highly expressive and intuitive tool that prevents user ear fatigue. Observing Tsugi's implementation directly inspired Auraline's shift away from generic button-pushing interfaces toward a continuous, fluid drawing workspace that prioritizes physical motion.

#### 4. ZKM Karlsruhe: From Xenakis's UPIC to Graphic Notation Today

Published by the ZKM Center for Art and Media, this academic compilation tracks the genesis and evolution of the UPIC (Unité Polyagogique Informatique du CEMAMu) system—a computational synthesis tool developed by the avant-garde composer and architect Iannis Xenakis in the late 1970s. This resource stands as the primary historical and artistic justification for Auraline, establishing a professional lineage for transforming hand-drawn geometric illustrations into audio waveforms.

**Key Applications:**
- **Vector Composition:** Analyzed the historical technique of utilizing a digitizing tablet to translate structural drawing strokes directly into specific wave frequencies, pitches, and durations.
- **Macro-Form Notation:** Studied how architectural vector lines can be reimagined as macro-level musical scores, breaking down the rigid limits of traditional Western sheet music.
- **Menu-Less Frameworks:** Explored the user workflow of the UPIC, discovering that eliminating text menus entirely allows non-musicians and artists to immediately compose audio through pure spatial expression.

Reviewing this historical system was incredibly useful for validating the creative direction of Auraline, confirming that abstract visual contours possess an inherent, powerful relationship with musical arrangement. The primary limitation of this research source is that the original UPIC relied on massive, custom, legacy mainframe hardware of the late 20th century, meaning its historical documentation offers no direct modern software answers. The core engineering challenge was translating Xenakis's grand compositional philosophy into optimized, lightweight C# logic that runs natively on a modern MacBook Air.

#### 5. Sound Design Theory: Procedural Modification Principles

This body of academic research focuses on the core principles of digital signal processing (DSP) and sound design, examining how fundamental audio waves can be dynamically altered using external telemetry data. It served as a theoretical bridge for understanding how to structure parameters within FMOD so they correlate logically with real-world user gestures.

**Key Applications:**
- **Micro-Modulation Dynamics:** Analyzed how micro-adjustments in pitch, volume, and filter cutoff frequencies simulate specific physical materials and spatial dimensions.
- **Multi-Parameter Balancing:** Studied the balance of multi-parameter modulation, exploring how changing two related variables simultaneously (e.g., matching a high visual point with increased pitch and narrower filter bandwidths) creates a cohesive sensory object.
- **System Predictability:** Investigated the concept of "audio predictability," finding that while sound variations prevent monotony, the core response to an interaction must remain stable so the user feels in control of the machine.

This theoretical research provided the foundation for designing the parameter structures within FMOD. It ensured that when a user interacts with the Auraline pads, the resulting audio manipulation feels organically connected to their movement rather than chaotic or random. While it lacks direct implementation steps for specific game engines, its overarching principles guided how the C# code scales data inputs before sending them to the mixer.

#### 6. Color Psychology and Cognitive Neuroaesthetics

This branch of cognitive neuroscience and color theory investigates how specific wavelengths of light trigger distinct emotional, physiological, and attentional responses within the human brain. This research direction was essential for developing Auraline's sequential visual "breadcrumbs," allowing the application to completely replace traditional text tutorials with targeted shifts in light and material emission.

**Key Applications:**
- **Attentional Capture:** Analyzed how highly saturated emissive colors, such as high-intensity Magenta, immediately command focal attention against pitch-black voids, making it the ideal choice for the primary "Power-Up" beacon.
- **Cognitive Load Reduction:** Studied the calming, low-alert properties of cooler spectrums (soft blues and cyans) and slow, rhythmic pulsing frequencies (0.5 Hz), which were implemented into the Reset and Next Track hint states to invite user interaction without triggering cognitive overwhelm.
- **Cross-Modal Harmony:** Investigated how the human brain processes multi-sensory harmony, discovering that visual brightness transitions must line up perfectly with audio volume rises to create a true sensation of a machine "powering up."

This research provided an empirical foundation for designing the visual layout of the application. It allowed Auraline to function as a highly responsive, self-explanatory instrument where changes in color and glow state guide the user through the interface. The only minor limitation is that color psychology results can vary based on cultural contexts; however, by focusing on universal neuroaesthetic patterns—such as light pulsing to indicate a "living" or active state—the interface achieves a highly accessible, cross-cultural intuitive flow.
