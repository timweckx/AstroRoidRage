using Godot;
using System;

public partial class Bullet : Area2D
{
	private const int SPEED = 400;
	private double _lifespan = 5.0;

	// Called every PHYSICS frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		_lifespan -= delta;
		if (_lifespan <= 0)
		{
			QueueFree();
			return;
		}

		Translate(Vector2.Up.Rotated(Rotation) * SPEED * (float)delta);
	}

    private void OnBodyEntered(Node2D body)
    {
		// We could call `.HasMethod & .Call` which is similar to GDScript `if body` + just calling the method since
		// GDScript supports duck/dynamic typing. For C#, its better to try to cast as the type we need and then calling the method
        if (body is Asteroid asteroid)
        {
            asteroid.EmitSignal(Asteroid.SignalName.WasShoot);
        	QueueFree();
        }
    }
}
