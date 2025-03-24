using Godot;
using System;

public partial class Leaderboard : Control
{
    private VBoxContainer _scoreContainer;
    private Button _backButton;

    public override void _Ready()
    {
        _scoreContainer = GetNode<VBoxContainer>("VBoxContainer/ScoreContainer");
        _backButton = GetNode<Button>("VBoxContainer/Back");

        _backButton.Pressed += OnBackPressed;

        LoadScores();
    }

    private void LoadScores()
    {
        const string savePath = "user://scores.save";

        if (!FileAccess.FileExists(savePath))
        {
            var noScoresLabel = new Label();
            noScoresLabel.Text = "No scores yet!";
            _scoreContainer.AddChild(noScoresLabel);
            return;
        }

        var file = FileAccess.Open(savePath, FileAccess.ModeFlags.Read);
        var content = file.GetAsText();
        file.Close();

        var splitScores = content.Split(",");

        for (int i = 0; i < splitScores.Length && i < 10; i++)
        {
            var scoreLabel = new Label();
            scoreLabel.Text = $"Player {i + 1}: {splitScores[i]}";
            _scoreContainer.AddChild(scoreLabel);
        }
    }

    private void OnBackPressed()
    {
        GetTree().ChangeSceneToFile("res://Scenes/mainmenu/MainMenu.tscn");
    }
}