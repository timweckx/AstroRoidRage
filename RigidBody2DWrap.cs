using Godot;
using System;

public partial class RigidBody2DWrap : RigidBody2D
{
	internal Vector2 viewportSize, spriteSize;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		viewportSize = GetViewport().GetVisibleRect().Size;
		spriteSize = GetNode<Sprite2D>("Sprite2D").GetRect().Size;
	}

    public override void _IntegrateForces(PhysicsDirectBodyState2D state)
    {
		// Transform is a struct, so in C# we can't directly change its attributes, need to create a new instance
		var origin = state.Transform.Origin;

		if (origin.X + spriteSize.X/2 < 0)
		{
			state.Transform = new(state.Transform.Rotation, new Vector2(viewportSize.X + spriteSize.X/2, origin.Y));
		}
		if (origin.X - spriteSize.X/2 > viewportSize.X)
		{
			state.Transform = new(state.Transform.Rotation, new Vector2(-spriteSize.X/2, origin.Y));
		}
		if (origin.Y + spriteSize.Y/2 < 0)
		{
			state.Transform = new(state.Transform.Rotation, new Vector2(origin.X, viewportSize.Y + spriteSize.Y/2));
		}
		if (origin.Y - spriteSize.Y/2 > viewportSize.Y)
		{
			state.Transform = new(state.Transform.Rotation, new Vector2(origin.X, -spriteSize.Y/2));
		}
    }
}
