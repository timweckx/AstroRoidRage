# AstroRoidRage
Astro Roid Rage - A Simple Godot Project Tutorial

Watch the full tutorial at https://youtube.com/quill18creates:

https://www.youtube.com/watch?v=HPL82UkeNA4

## C# Version

This version has all GDScript files translated to C#. Most things are straight-forward, but there is some functionality
that is not available or is done slightly different in C# vs GDScript. I've left all the GDScripts in this version for reference.

There are probably other things that can be done differently / more in style with C#, but I stayed as close to the original
GDScript version as possible.

### Input action strings

Various methods (e.g. `Input.IsActionPressed`) take in `StringName` vs a regular string (""). C# will automatically convert your
plain string into StringName, but for frequent calls this can affect performance (mostly for large games). To avoid this, this code
uses a static KeyMap class that has all the actions pre-defined with StringName instances. (see Player.cs)

### Signals

To stay close with the original version, I kept the signal connects in the editor. However, you can also easily hook up signals in
code using simple C# event references
```
public override void _Ready()
{
    mySignal += OnMySignal();
}
```

### Preload

C# does not have a global scoped `Preload` method, use `GD.Load` or `ResourceLoader.Load` instead. In C#, these methods use `CACHE_MODE_REUSE` by default, 
so they won't reload from disk if called multiple times, but there might be slight nuances, see https://docs.godotengine.org/en/stable/tutorials/best_practices/logic_preferences.html#loading-vs-preloading for details.

### OnReady

C# does not have equivalent global function for `@onready`. You must use initialize those values in `_Ready` method.

### Method overrides

C# requires you to explicitly call the base class's method when overriding. E.g. for the Player/Asteroid -> RigidBody2DWrap inheritance,
if you don't call `base._Ready()`, your child classes will not set the viewport & sprite sizes.
