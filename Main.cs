using Godot;
using System;
using System.Drawing;
using System.Net;

public partial class Main : Node
{
	[Signal]
	public delegate void ScoreChangedEventHandler(int score);

	[Signal]
	public delegate void LevelChangedEventHandler(int level);

	[Signal]
	public delegate void LivesChangedEventHandler(int lives);

	[Signal]
	public delegate void GameOverEventHandler();

	private int _score;
	public int Score
	{
		get { return _score; }
		set
		{
			_score = value;
			EmitSignal(SignalName.ScoreChanged, value);
		}
	}

	private int _level;
	public int Level
	{
		get { return _level; }
		set
		{
			_level = value;
			EmitSignal(SignalName.LevelChanged, value);
		}
	}

	private int _lives;
	public int Lives
	{
		get { return _lives; }
		set
		{
			_lives = value;
			EmitSignal(SignalName.LivesChanged, value);
		}
	}

	private int _numAsteroids = 3;	

	private PackedScene _playerScene = GD.Load<PackedScene>("res://player.tscn");
	private PackedScene _asteroidBigScene = GD.Load<PackedScene>("res://asteroid_big.tscn");

	private int _asteroidSpawnRangeMin = 100;
	private int _asteroidSpawnRangeMax = 300;

	private Player _playerNode = null;
	private Vector2 _viewportSize;

	[Export]
	private Node2D _asteroidContainer;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// C# doesn't have an @onready equivalent, so just initialize here
		_viewportSize = GetViewport().GetVisibleRect().Size;

		SetupNewGame();
	}

	public void SetupNewGame()
	{
		CleanupGame();
		Lives = 3;
		Score = 0;
		Level = 0;
		SetupNewLevel(_numAsteroids);
	}

	private void CleanupGame()
	{
		if (_playerNode != null)
		{
			_playerNode.QueueFree();
			_playerNode = null;
		}

		// Destroy asteroids
		foreach (var ast in _asteroidContainer.GetChildren())
		{
			ast.QueueFree();
		}
	}

	// Called every VISUAL frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnPlayerDeath()
	{
		if (_lives <= 0)
		{
			_GameOver();
			return;
		}

		Lives -= 1;
		SpawnPlayer();
	}

	// using _ prefix to avoid conflict with public event `GameOver`
	private void _GameOver()
	{		
		if (_playerNode != null)
		{
			_playerNode.QueueFree();
			_playerNode = null;
		}

		EmitSignal(SignalName.GameOver);
	}

	private void SetupNewLevel(int numAsteroids){
		Level += 1;
		
		SpawnPlayer();

		for (var i=0; i<numAsteroids; i++)
		{
			SpawnAsteroid();
		}
	}

	private void SpawnPlayer()
	{
		// shorthand for checking for null
		_playerNode?.QueueFree();
		_playerNode = _playerScene.Instantiate<Player>();		
		_playerNode.Position = _viewportSize/2;
		_playerNode.HasDied += OnPlayerDeath;
		CallDeferred(MethodName.AddChild, _playerNode);
	}

    private void SpawnAsteroid()
	{
		var asteroid = _asteroidBigScene.Instantiate<Asteroid>();
		asteroid.Position = _viewportSize/2 + (Utility.RandomUnitVector2() * Utility.Random.RandfRange(_asteroidSpawnRangeMin, _asteroidSpawnRangeMax));
		_asteroidContainer.CallDeferred(MethodName.AddChild, asteroid);
	}

	private void AddToScore(int n)
	{
		Score += n;
	}

}
