// UWP Sodoku Game
// Write by Trac Quang Hoa, 03/2019

using SudokuLib.Game;
using SudokuUWP.Logics;
using SudokuUWP.Models;
using SudokuUWP.Pages;
using SudokuUWP.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SudokuUWP
{
    public class MainPageViewModel : BindableObject
    {
        private GameLogic gameLogic;

        private GameState gameState = GameState.NotStart;

        private DispatcherTimer timer = new DispatcherTimer();


        public int X => gameLogic.X;

        public int Y => gameLogic.Y;

        public int Size => gameLogic.Size;

        public DispGameLevel Level => gameLogic.Level;

        public List<BoxModel> Boxes => gameLogic.Boxes;

        private BoxModel selectedItem;

        public BoxModel SelectedItem
        {
            get => selectedItem;
            set
            {
                selectedItem = value;
                NotifyPropertyChanged(nameof(IsHintEnabled));
            }
        }

        public bool IsHintEnabled => IsPlaying && !(SelectedItem?.IsFixed ?? true);

        private TimeSpan usedTime = TimeSpan.Zero;

        public string UsedTime
        {
            get
            {
                if (usedTime.Hours == 0)
                {
                    return $"{usedTime.Minutes:00}:{usedTime.Seconds:00}";
                }

                return $"{usedTime.Hours}:{usedTime.Minutes:00}:{usedTime.Seconds:00}";
            }
        }

        public Visibility PlayingVisibility => gameState == GameState.Pause ? Visibility.Visible : Visibility.Collapsed;

        public Visibility PauseVisibility => gameState == GameState.Playing ? Visibility.Visible : Visibility.Collapsed;

        public bool IsPauseOrPlaying => gameState == GameState.Playing || gameState == GameState.Pause;

        public bool IsPlaying => gameState == GameState.Playing;


        public void ValidateWhenChangeAt(BoxModel box)
        {
            gameLogic.ValidateWhenChangeAt(box);
            Boxes.ForEach(b => b.NotifyAllPropertiesChanged());
        }

        public DelegateCommand NewGameCommand => new DelegateCommand(parameter =>
        {
            //gameLogic.NewGame();
            ////gameState = GameState.Playing;
            //ChangeGameState(GameState.Playing);
            StartNewGame();
            Boxes.ForEach(b => b.NotifyAllPropertiesChanged());
        });

        public DelegateCommand NewGameWithOptionsCommand => new DelegateCommand(async parameter =>
        {
            var newGameDialog = new NewGameDialog()
            {
                X = X,
                Y = Y,
                Level = Level
            };
            if (await newGameDialog.ShowAsync() == ContentDialogResult.Primary)
            {
                gameLogic = new GameLogic(newGameDialog.X, newGameDialog.Y, newGameDialog.Level);
                //gameLogic.NewGame();
                //gameState = GameState.Playing;
                //Boxes.ForEach(b => b.NotifyAllPropertiesChanged());
                StartNewGame();
                NotifyAllPropertiesChanged();
            }
        });

        public DelegateCommand HintCommand => new DelegateCommand(parameter =>
        {
            selectedItem.DisplayValue = selectedItem.Value;
            ValidateWhenChangeAt(selectedItem);
        });

        public DelegateCommand SolveGameCommand => new DelegateCommand(parameter =>
        {
            foreach (var b in Boxes.Where(b => !b.IsFixed))
            {
                b.DisplayValue = b.Value;
                // TODO ugly
                b.BoxValue.IsInvalidBlock = b.BoxValue.IsInvalidCol = b.BoxValue.IsInvalidRow = false;
                b.NotifyPropertyChanged("Background");
            }

            ChangeGameState(GameState.Solved);
        });

        public ICommand LevelChangedCommand => new DelegateCommand(parameter =>
        {
            StartNewGame(parameter as DispGameLevel);
            NotifyPropertyChanged(nameof(Level));
            Boxes.ForEach(b => b.NotifyAllPropertiesChanged());
        });

        public DelegateCommand PauseCommand => new DelegateCommand(parameter =>
        {
            if (gameState == GameState.Playing)
            {
                ChangeGameState(GameState.Pause);
            }
            else if (gameState == GameState.Pause)
            {
                ChangeGameState(GameState.Playing);
            }
        });

        public MainPageViewModel()
        {
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;

            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

            int.TryParse(localSettings.Values["BlockWidth"] as string, out int x);
            if (x < 2 || x > 4)
            {
                x = 3;
            }

            int.TryParse(localSettings.Values["BlockHeight"] as string, out int y);
            if (y < 2 || y > 4)
            {
                y = 3;
            }

            GameLevel level = GameLevel.Medium;

            gameLogic = new GameLogic(x, y, level);

            StartNewGame();
        }

        private void Timer_Tick(object sender, object e)
        {
            usedTime = usedTime.Add(TimeSpan.FromSeconds(1));
            NotifyPropertyChanged(nameof(UsedTime));
        }

        private void StartNewGame(GameLevel? level = null)
        {
            usedTime = TimeSpan.Zero;
            NotifyPropertyChanged(nameof(UsedTime));

            gameLogic.NewGame(level);
            ChangeGameState(GameState.Playing);
        }

        private void ChangeGameState(GameState newState)
        {
            gameState = newState;
            if (gameState == GameState.Playing)
            {
                timer.Start();
            }
            else
            {
                timer.Stop();
            }

            NotifyPropertyChanged(nameof(PlayingVisibility));
            NotifyPropertyChanged(nameof(PauseVisibility));
            NotifyPropertyChanged(nameof(IsPauseOrPlaying));
            NotifyPropertyChanged(nameof(IsPlaying));
            NotifyPropertyChanged(nameof(IsHintEnabled));
        }
    }

    enum GameState
    {
        NotStart,
        Playing,
        Pause,
        Finished,
        Solved
    }
}
