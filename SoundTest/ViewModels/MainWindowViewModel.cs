using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Interactivity;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.Input;
using SoundFlow.Backends.MiniAudio;
using SoundFlow.Components;
using SoundFlow.Enums;
using SoundFlow.Providers;
using SoundTest.ViewModels;

namespace SoundTest.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public string Greeting => "Welcome to Avalonia!";

    private List<SoundFlow.Components.SoundPlayer> _players = new List<SoundFlow.Components.SoundPlayer>();
    private MiniAudioEngine _engine = new(44100, Capability.Playback, SampleFormat.F32, 1);
    public void ChipsButton_OnClick()
    {
      
        var matchingPlayer = _players.FirstOrDefault(player => player.Name == "chips");
        if (matchingPlayer == null)
        {
            AddSoundPlayer("chips.mp3", "chips");
        }
    }
    
    public void CheckButton_OnClick()
    {
        var matchingPlayer = _players.FirstOrDefault(player => player.Name == "check");
        if (matchingPlayer == null)
        {
            AddSoundPlayer("check.mp3", "check");
        }
    }
    private void AddSoundPlayer(string fileName, string name)
    {
        if (_players.Count == 0 || _players.Any(player => player.Name != name))
        {
            var file = File.OpenRead($"Assets/{fileName}");
            var player = new SoundFlow.Components.SoundPlayer(new StreamDataProvider(file));
            player.Name = name;
            player.PlaybackEnded += (sender, args) =>
            {
                CleanUpPlayer(player);
            };
            _players.Add(player);
            Mixer.Master.AddComponent(player);  
            player.Play();
        }
        
    }
    
    private void CleanUpPlayer(SoundFlow.Components.SoundPlayer player)
    { 
        Dispatcher.UIThread.InvokeAsync(() => { Mixer.Master.RemoveComponent(player); });
        _players.Remove(player);
        
    }
}