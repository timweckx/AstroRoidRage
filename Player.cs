using Godot;
using System;

public partial class Player : RigidBody2DWrap
{
	
	[Signal]
	public delegate void HasDiedEventHandler();

	private float _rotationSpeed = Mathf.Tau;
	private int _thrustForce = 400;

	private double _fireCooldown = .25f;
	private double _fireCooldownRemaining = 0;

	private double _invulnerabilityDuration = 2.0f;
	private double _invulnerabilityRemaining;

	private PackedScene _bulletScene = GD.Load<PackedScene>("res://bullet.tscn");

	private Sprite2D _mySprite;

	// In C#, the IsActionPressed takes a `StringName` parameter, not a regular string. To avoid unnecessary implicit casting
	// of a string to a StringName every frame, you can implement a keymap with predefined StringName instances.
	public static class KeyMap {
		public static readonly StringName RotateCW = new("rotate_cw");
		public static readonly StringName RotateCCW = new("rotate_ccw");
		public static readonly StringName ThrustForward = new("thrust_forward");
		public static readonly StringName Fire = new("fire");
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// C# requires explicit call to base._Ready()
		base._Ready();

		_invulnerabilityRemaining = _invulnerabilityDuration;
		_mySprite = GetNode<Sprite2D>("Sprite2D");
	}

	// Called every PHYSICS frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		if (_invulnerabilityRemaining > 0)
		{
			_invulnerabilityRemaining -= delta;
			if (_invulnerabilityRemaining > 0)
			{
				DoInvulnerabilityVisual();
			}
			else
			{
				StopInvulnerabilityVisual();
			}
		}

		AngularVelocity = 0;
		if (Input.IsActionPressed(KeyMap.RotateCW))
		{
			AngularVelocity = _rotationSpeed;
		}
		if (Input.IsActionPressed(KeyMap.RotateCCW))
		{
			AngularVelocity = -_rotationSpeed;
		}
		if (Input.IsActionPressed(KeyMap.ThrustForward))
		{
			ApplyForce(Vector2.Up.Rotated(Rotation) * _thrustForce);
		}		

		_fireCooldownRemaining -= delta;
		if (_fireCooldownRemaining <= 0 && Input.IsActionPressed(KeyMap.Fire))
		{
			_fireCooldownRemaining = _fireCooldown;
			DoShootBullet();
		}
	}

	private void DoInvulnerabilityVisual()
	{	
		// Modulate is a struct in C#, so we can't modify attributes directly, need to create new instance
		_mySprite.Modulate = new Color(_mySprite.Modulate, Mathf.Sin((float)_invulnerabilityRemaining * 50.0f) / 4.0f + 0.50f);
	}

	private void StopInvulnerabilityVisual()
	{
		_mySprite.Modulate = new Color(_mySprite.Modulate, 1);
	}

	private void DoShootBullet()
	{
		var bullet = _bulletScene.Instantiate<Bullet>();
		bullet.Rotation = Rotation;
		bullet.Position = GetNode<Node2D>("BulletSpawnSpot").GlobalPosition;
		GetParent().AddChild(bullet);
	}

    private void OnBodyEntered(Node body)
    {	
		if (_invulnerabilityRemaining > 0)
		{
			return;
		}

		if (body is Asteroid asteroid)
		{
			body.EmitSignal(Asteroid.SignalName.WasShoot);
		}

		EmitSignal(SignalName.HasDied);
    }
}
