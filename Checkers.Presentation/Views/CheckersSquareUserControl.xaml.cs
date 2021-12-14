using Checkers.Presentation.Domain.Interface;
using Checkers.Presentation.Domain.Model;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Checkers.Presentation.Views
{
    /// <summary>
    /// Lógica interna para CheckersSquareUserControl.xaml
    /// </summary>
    public partial class CheckersSquareUserControl : UserControl, INotifyPropertyChanged, IMinimaxClonable
    {
        public CheckersSquareUserControl() { }
        public CheckersPoint CheckersPoint { get; set; }
        private Brush background;

        public Brush BackgroundColor
        {
            get => background;
            set
            {
                background = value;
                OnPropertyChanged("BackgroundColor");
            }
        }


        public CheckersSquareUserControl(Brush backgroundColor, CheckersPoint checkersPoint, RoutedEventHandler routedEventHandler)
        {
            InitializeComponent();
            this.Background = backgroundColor;
            this.CheckersPoint = checkersPoint;
            this.button.Click += routedEventHandler;

            UpdateSquare();
        }
        public object GetMinimaxClone() 
            => new CheckersSquareUserControl { CheckersPoint = (CheckersPoint)this.CheckersPoint.GetMinimaxClone() };

        public void UpdateSquare()
        {
            if (CheckersPoint != null && CheckersPoint.Checker != null)
            {
                try
                {
                    if (checkerImage != null)
                        this.Dispatcher.Invoke(() => 
                        {    
                            checkerImage.Source = CheckersPoint.Checker.BuildCheckerImageSource();
                        });
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao criar as peças no tabuleiro: {ex.Message}");
                    return;
                }
            } else
            {
                HideChecker();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private void HideChecker() => checkerImage.Visibility = Visibility.Collapsed;

    }
}
