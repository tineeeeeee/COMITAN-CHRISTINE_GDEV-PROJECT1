using Godot;
using System;

public partial class Game : Node2D
{
    const double GEM_MARGIN = 50.0;

    private static readonly AudioStream EXPLODE_SOUND =
                GD.Load<AudioStream>("res://assets/explode.wav");

    [Export] private PackedScene _gemScene;
    [Export] private Timer _spawnTimer;
    [Export] private Label _scoreLabel;
    [Export] private AudioStreamPlayer _music;
    [Export] private AudioStreamPlayer2D _effects;
    
    private CanvasLayer _gameOverUI;
    private Button _mainMenuButton;
    private Label _finalScoreLabel; 

    private int _score = 0;

    public override void _Ready()
    {
        _spawnTimer.Timeout += SpawnGem;
        SpawnGem();

        _gameOverUI = GetNode<CanvasLayer>("CanvasLayer");
        _mainMenuButton = _gameOverUI.GetNode<Button>("Control/Button");
        _finalScoreLabel = _gameOverUI.GetNode<Label>("Control/FinalScoreLabel"); 

        _gameOverUI.Visible = false;

        _mainMenuButton.Pressed += OnMainMenuPressed;
    }

    private void SpawnGem()
    {
        Rect2 vpr = GetViewportRect();
        Gem gem = (Gem)_gemScene.Instantiate();

        AddChild(gem);

        float rX = (float)GD.RandRange(
            vpr.Position.X + GEM_MARGIN, vpr.End.X - GEM_MARGIN
        );

        gem.Position = new Vector2(rX, -100);
        gem.OnScored += OnScored;
        gem.OnGemOffScreen += GameOver;
    }

    private void OnScored()
    {
        _score += 1;
        _scoreLabel.Text = $"{_score:0000}";
        _effects.Play();
    }

    private void GameOver()
    {
        GD.Print("Game Over! Showing UI...");

        _spawnTimer.Stop();
        _music.Stop();
        _effects.Stop();

        _effects.Stream = EXPLODE_SOUND;
        _effects.Play();

        foreach (Node node in GetChildren())
        {
            if (node is Gem gem)
            { 
                gem.SetProcess(false);
                gem.SetPhysicsProcess(false);
            }
        }
    
        _finalScoreLabel.Text = $"Final Score: {_score}";

        _gameOverUI.Visible = true;

        SaveScore(_score);
    }

    private void SaveScore(int score)
{
    const string savePath = "user://scores.save";
    var scores = new Godot.Collections.Array<int>();

    if (FileAccess.FileExists(savePath))
    {
        var file = FileAccess.Open(savePath, FileAccess.ModeFlags.Read);
        var content = file.GetAsText();
        file.Close();
        var splitScores = content.Split(",");
        foreach (var s in splitScores)
        {
            if (int.TryParse(s, out int parsedScore))
                scores.Add(parsedScore);
        }
    }

    scores.Add(score);

    scores.Sort();
    scores.Reverse();
    if (scores.Count > 10)
        scores.Resize(10);

    var fileWrite = FileAccess.Open(savePath, FileAccess.ModeFlags.Write);
    fileWrite.StoreString(string.Join(",", scores));
    fileWrite.Close();
}  

private void OnMainMenuPressed()
    {
        GD.Print("Returning to Main Menu...");
        GetTree().ChangeSceneToFile("res://Scenes/mainmenu/MainMenu.tscn");
    }

}