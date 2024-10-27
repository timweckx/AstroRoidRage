using Godot;
using System;

public partial class MainUi : Control
{
	[Export]
	private Label _scoreLabel;
	[Export]
	private Label _levelLabel;
	[Export]
	private Label _livesLabel;
	[Export]
	private Panel _gameOverPanel;

	private void OnMainScoreChanged(int score) {
		_scoreLabel.Text = $"Score: {score}";
	}

	private void OnMainLivesChanged(int lives) {
		_livesLabel.Text = $"Lives: {lives}";
	}

	private void OnMainLevelChanged(int level) {
		_levelLabel.Text = $"Level: {level}";
	}

	private void OnMainGameOver() {
		_gameOverPanel.Show();
	}

	private void OnRestartButtonPressed() {
		GetParent<Main>().SetupNewGame();
		_gameOverPanel.Hide();
	}

	private void OnExitButtonPressed() {
		GetTree().Quit();
	}
}
