Thanks for buying Simbryo's Ferr2D Terrain Tool for Unity3D! We hope your experience with it is the best ever, and should it ever be less than that, please drop us a note and let us know!

For documentation and tutorial videos go here! Or check out the quickstart guide and reference in the same folder as this file.
http://www.simbryocorp.com/Ferr2DTerrain

We can always be reached either by email, or on twitter!
support@simbryocorp.com
@koujaku

Also, big thanks to Kelde for providing some sweet materials~ Check out his stuff!
@KaiElde
http://artbyelde.wordpress.com/

MENUS
GameObject->Create Ferr2D Terrain->Create Physical 2D Terrain (Ctrl+T)
	Creates a pre-built terrain object in the scene, with collider options on. It's the easiest way of creating a terrain object.

GameObject->Create Ferr2D Terrain->Create Decorative 2D Terrain (Ctrl+Shift+T)
	Same as previous, but with colliders off.
	
GameObject->Create Ferr2D Terrain->Create Terrain Material OR
Project Window->Create->Create Terrain Material
	This creates a basic empty terrain material prefab. Hook up some materials to it, define some sides, and go!


PATH CONTROLS
SHIFT+Click: add path point
CTRL +Click: removes path points

KNOWN ISSUES
-If you drag your mouse outside the material editor window and let go while dragging a region, no undo data will be saved.
-Forcing an edge type on the closing segment will have no effect, however, forcing the edge type before the closing segment will affect both.
-Building your game will NOT prebuild terrain prefabs! Only playing the game will, so make sure you play a scene, or 'Assets->Prebuild Ferr2D Terrain' before building your game!

VERSION LOG

v1.0.7 2014-3
+Improved smoothed terrain support, it's very useable now =D
+Improved triangulation performance, and no more weird holes!
+Basic snapping features can be found in Edit->Preferences->Ferr->Snap Mode, but it's a little early. I do not promise it's mind-blowing.
+Path Terrain
 -Fixed weird collider generation issues, colliders should now behave far more predictably!
 -Fixed prefab issues, terrain objects now save and load their meshes properly when they are prefab objects! ('Assets->Prebuild Ferr2D Terrain' to force save)
 -Fixed an issue with 2D physics materials not getting assigned correctly.
 -Added smoothSphereCollisions option for 3D colliders.                                                    COLLIDERS->Smooth Sphere Collisions
 -Added an edge split option, to reduce texture stretching around corners.                                 VISUALS->Split Middle
 -Added option to create tangents on terrain meshes for normal mapping (warning: very slow in editor only) VISUALS->Create Tangents
 -Added some options for when randomization occurs. Per segment, vs. per quad.                             VISUALS->Randomize by World Coordinates.
+Path
 -Editing on scaled/rotated path is much improved. Not perfect, but better.


v1.0.6 2013-11
+Added 4.3 support for the new 2D Physics system
+Added 4.3 support for the new Undo system
+Added a new example scene demonstrating how to make real-time edits to the terrain
+Added a cool JSON parser, improvements still in line for this later
+Added JSON save/load to Ferr2DT_PathTerrain, Ferr2DT_TerrainMaterial, and Ferr2D_Path
+Path Terrain
 -Added option to switch between 3D colliders and 2D colliders when using Unity 4.3+
 -Added a slider to adjust how Ferr2DT stretches texture segments
 -Added AddAutoPoint method for inserting points easily to the path, see the new example scene for reference
+Path
 -Tweaked handle size to work slightly better in orthographic view, more work on this due later
 
 
v1.0.5 2013-10
+Path Terrain
 -Added edge material overrides
 -Tweaked stretch algorithm, instead of stretching between a scale of 1 and 2, it now stretches between 0.5 and 1.5
 -Changed draw order of materials for better compatability with transparent fill materials
 -Added preference menu for showing or hiding terrain mesh lines
 -Added AddPoint method for easier procedural terrain
 -Fixed a collider glitch with Inverted fill modes
 -Fixed the Split Corners toggle, should now work
 -Fixed a glitch with the smooth split field not displaying properly
+Path
 -Updated icons for better visibility against same color backgrounds
 -Added preference menu for icon scaling
+Terrain Materials
 -Fixed bug with an overzealous simple mode
+Assets
 -Updated Ferr/Unlit Textured Vertex Color shader to properly use Unity's UV settings

v1.0.4 2013-10
+Terrain Materials
 -Added context menu to the project window for creating new Terrain Materials!
 -Moved the Terrain Material editor into its own windo, various and sundry improvements there.
 -Added a 'simple' mode to the TerrainMaterial editor, should be easier to create common case materials
+Path Terrain
 -Added physics material slot, since MeshColliders are generated at runtime
 -Added new fill types, Fill Only Closed, and Fill Only Skirt
 -Added option for disabling collider meshes along specific directions
 -Collider mesh no longer includes faces that point down +Z and -Z, as they're not important to 2D collisions, and it's much faster without\
+Path
 -Path GUI now scales properly with GameObject
 -Fixed an issue with button hotspots being too large when zoomed in

v1.0 2013-10
Release! Wooh! Have fun with it!