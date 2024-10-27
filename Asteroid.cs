using Godot;
using System;
using System.Net;

public partial class Asteroid : RigidBody2DWrap
{
	[Signal]
	public delegate void WasShootEventHandler();
	private const int STARTING_FORCE = 100;
	private const float STARTING_ROTATION = Mathf.Pi;

	[Export]
	private PackedScene _debrisScene;

	[Export]
	private int _debrisAmount = 2;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// C# requires explicit call to base._Ready()
		base._Ready();

		ApplyImpulse(Utility.RandomUnitVector2() * Utility.Random.RandfRange(STARTING_FORCE/2, STARTING_FORCE*2));
		AngularVelocity = Utility.Random.RandfRange(-STARTING_ROTATION, STARTING_ROTATION);
	}

	public void OnWasShoot()
	{
		if (_debrisScene != null) {
			for (var i = 0; i < _debrisAmount; i++) {
				var debris = _debrisScene.Instantiate<Asteroid>();
				debris.GlobalPosition = GlobalPosition;
				GetParent().CallDeferred(MethodName.AddChild, debris);
			}
		}
		QueueFree();
	}
}
