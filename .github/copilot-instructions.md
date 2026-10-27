# GitHub Copilot Instructions for MyRoom - I want that (Continued) Mod

## Mod Overview and Purpose

MyRoom - I want that (Continued) is an update and continuation of the alicetries mod. The mod aims to enhance the room arrangement and customization experience for pawns in RimWorld. It introduces various settings and systems that allow pawns to better organize and decorate their rooms based on their preferences and needs.

## Key Features and Systems

- **Impressive Room Level Setting:** Allows players to set the level of impressiveness that pawns aim to achieve in their rooms.
- **Minimum Space Setting:** Players can set a minimum space requirement for rooms, influencing how pawns arrange their belongings.
- **Wall Alignment and Orientation:** Pawns will place items along walls and orient rotatable objects to face outward for better aesthetics.
- **Avoidance of Doorway Obstructions:** Pawns ensure that they do not place larger items where they might block doorways.
- **Bed Replacement:** Pawns will not add beds to their rooms; future updates might include replacing existing beds.
- **Blueprint Placement Restrictions:** Pawns will not attempt to place multiple blueprints in the same room simultaneously.
- **Room Decoration:** Pawns will occasionally rearrange their rooms using minified things they find beautiful, based on their personal preferences.

## Coding Patterns and Conventions

1. **Modular Design:** The mod is structured into separate C# files for different functionalities, such as furniture job logic, mental state management, and utility functions.
2. **Encapsulation:** Classes are well-encapsulated, with private fields and public methods to manage state and behavior.
3. **Reusability:** Common utility functions are placed in separate classes to promote code reuse and maintainability.
4. **Documentation:** Methods and classes are documented with XML comments to clarify their purpose and usage.

## XML Integration

The mod integrates with RimWorld's XML system to define and register new game elements such as mental states and thought processes. Key XML files include:

- **Defs:** Contains definitions for mental states, thoughts, and other game elements.
- **Patches:** XML patches to modify existing game behavior using Harmony patching.

### Example XML Def

xml
<Defs>
  <MentalStateDef>
    <defName>MentalState_StealToRoom</defName>
    <label>Stealing for Room</label>
    <description>Stealing items for room decoration</description>
    <!-- Additional properties -->
  </MentalStateDef>
</Defs>


## Harmony Patching

The mod uses Harmony patching to extend and modify existing RimWorld behavior. Key patching areas include:

- **Thoughts and Mental States:** Enhancing pawn behavior related to thoughts and mental states.
- **Room Arrangement:** Modifying room arrangement logic to accommodate the new settings and systems.

### Example Harmony Patch

csharp
using HarmonyLib;
using RimWorld;
using Verse;

namespace MyRoom
{
    [HarmonyPatch(typeof(Pawn), "GetThoughts")]
    public static class HarmonyPatch_GetThoughts
    {
        public static void Postfix(Pawn __instance, List<Thought> __result)
        {
            // Custom logic to modify thoughts
        }
    }
}


## Suggestions for Copilot

1. **Start with XML Defs:** When adding new game elements, start by defining them in XML files and use Copilot to generate the corresponding C# classes and methods.
2. **Utilize Utility Classes:** Leverage the existing utility classes like `BlueprintUtilities` for common tasks related to blueprint installation and job generation.
3. **Harmony Patching:** When modifying existing game behavior, use Copilot to generate Harmony patches and integrate them with the mod's patching system.
4. **Modularize Code:** Follow the modular design pattern by creating separate classes for different functionalities and use Copilot to ensure encapsulation and reusability.
5. **Documentation:** Use Copilot to generate XML comments for new methods and classes to maintain clear documentation.

By following these guidelines and utilizing GitHub Copilot effectively, you can enhance the mod's functionality and maintain its codebase efficiently.

## Project Solution Guidelines
- Relevant mod XML files are included as Solution Items under the solution folder named XML, these can be read and modified from within the solution.
- Use these in-solution XML files as the primary files for reference and modification.
- The `.github/copilot-instructions.md` file is included in the solution under the `.github` solution folder, so it should be read/modified from within the solution instead of using paths outside the solution. Update this file once only, as it and the parent-path solution reference point to the same file in this workspace.
- When making functional changes in this mod, ensure the documented features stay in sync with implementation; use the in-solution `.github` copy as the primary file.
- In the solution is also a project called Assembly-CSharp, containing a read-only version of the decompiled game source, for reference and debugging purposes.
- For any new documentation, update this copilot-instructions.md file rather than creating separate documentation files.


## Hard rules (must follow)
- Do NOT run commands that modify the repo (no git commit, git apply, dotnet format) unless explicitly asked.
- Prefer minimal reads: read only the smallest code region needed (around the suspicious lines).
- When mentioning SonarQube issues, automatically use the SonarQube MCP service to fetch and address issues instead of making inferred fixes without querying SonarQube first.
- When mentioning the rimworld log, automatically use the Rimworld MCP service to fetch the log.

