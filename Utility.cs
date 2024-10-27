using Godot;
using System;

public partial class Utility : Resource
{
	// C# doesn't expose some of the random functions as globals like GDScript, so this utility
	// generates a Random instance we can use instead.
	public static RandomNumberGenerator Random = new();

	public static Vector2 RandomUnitVector2(){
        return new Vector2(
            Random.RandfRange(-1.0f, 1.0f),
            Random.RandfRange(-1.0f, 1.0f)
        ).Normalized();
    }
}
