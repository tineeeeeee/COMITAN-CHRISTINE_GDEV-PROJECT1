using Godot;
using System;

public partial class MainMenu : Control
{
    public override void _Ready()
    {
        Button playButton = GetNode<Button>("PlayButton");
        playButton.Pressed += OnPlayPressed;

        Button leaderboardButton = GetNode<Button>("LeaderboardButton");
        leaderboardButton.Pressed += OnLeaderboardPressed;
    }

    private void OnPlayPressed()
    {
        GD.Print("Play Button Clicked! Loading Game...");
        GetTree().ChangeSceneToFile("res://Scenes/Game/Game.tscn");
    }

    private void OnLeaderboardPressed()
    {
        GD.Print("Leaderboard Button Clicked! Loading Leaderboard...");
        GetTree().ChangeSceneToFile("res://Scenes/leaderboard/Leaderboard.tscn");
    }
}