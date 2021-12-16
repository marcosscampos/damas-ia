using Checkers.Common;
using Checkers.Domain;
using Checkers.Presentation.Domain.Exceptions;
using Checkers.Presentation.Domain.Model;
using Checkers.Presentation.Domain.Model.Genome;
using Checkers.Presentation.Views;
using Checkers.Presentation.Views.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Checkers.Presentation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly Logger Logger = Logger.GetSimpleLogger();

        private static readonly int MaxTurns = 500;

        private CheckersMove currentMove;
        private CheckerBoard checkerBoard;

        private Thread aiThread;

        private List<CheckersMove> currentAvailableMoves;

        #region Default configuration
        public string Difficulty { get; set; } = "medium";
        public int Depth { get; set; } = 4;
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            InitializeCheckers();

            if (ConstantsSettings.RunningGeneticAlgo)
            {
                aiThread = new Thread(new ThreadStart(GeneticAlgoLoop));
                aiThread.SetApartmentState(ApartmentState.STA);
                aiThread.Start();
            }
            else if (ConstantsSettings.IsAiDuel)
            {
                aiThread = new Thread(new ThreadStart(RunAIDuel));
                aiThread.SetApartmentState(ApartmentState.STA);
                aiThread.Start();
            }
        }

        public void Button_Click(object sender, RoutedEventArgs e)
        {
            Thread myNewThread = new Thread(() => ButtonClickWork(sender));
            myNewThread.SetApartmentState(ApartmentState.STA);
            myNewThread.Start();
        }

        private void InitializeCheckers()
        {

            this.Dispatcher.Invoke(() =>
            {
                checkerBoard = new CheckerBoard();
                checkerBoard.MakeBoard(new RoutedEventHandler(Button_Click));
                string currentTurn = checkerBoard.CurrentPlayerTurn == PlayerColor.Red ? "Vermelha" : "Preta";

                lst.ItemsSource = checkerBoard.BoardArray;

                currentMove = null;
                var dificuldade = "";

                switch(Difficulty)
                {
                    case "hard":
                        dificuldade = "Difícil";
                        break;
                    case "medium":
                        dificuldade = "Médio";
                        break;
                    case "easy":
                        dificuldade = "Fácil";
                        break;
                    default:
                        break;
                }

                SetTitle($"Jogo de Damas | Turno da Peça {currentTurn} | Dificuldade: {dificuldade}");

                DisableAllButtons();
                EnableButtonsWithMove();
            });
        }

        private void GeneticAlgoLoop()
        {
            int roundNumber = 0;
            while (roundNumber < ConstantsSettings.NumberOfRounds)
            {
                DoGeneticAlgo();
            }

            GeneticProgress.GetGeneticProgressInstance().Delete();
        }

        private void DoGeneticAlgo()
        {
            GeneticProgress currentProgress = GeneticProgress.GetGeneticProgressInstance();
            WinningGenome winningGenome = WinningGenome.GetWinningGenomeInstance();
            RandomGenome randomGenome = RandomGenome.GetRandomGenomeInstance();

            while (currentProgress.NumberOfGames < ConstantsSettings.NumberOfSimulations)
            {
                try
                {
                    RunAIDuel();
                }
                catch (AIException ex)
                {
                    Logger.Error("Erro de AI: " + ex.Message);
                    InitializeCheckers();
                    continue;
                }

                currentProgress.NumberOfGames++;
                object winner = checkerBoard.GetWinner();
                if (winner != null && winner is PlayerColor winningPlayer && winningPlayer == PlayerColor.Red)
                {
                    currentProgress.NumberOfRandomGenomeWins++;
                }

                Logger.Info("Jogo somente AI finalizado, o ganhador é: " + winner);
                Logger.Info(string.Format(
                    "Current Stats -- NumberOfGamesPlayed: {0}, NumberOfRandomGenomeWins {1}",
                    currentProgress.NumberOfGames,
                    currentProgress.NumberOfRandomGenomeWins));

                InitializeCheckers();
            }

            if (currentProgress.NumberOfRandomGenomeWins > currentProgress.NumberOfGames / 2)
            {
                winningGenome.SetNewWinningGenome(randomGenome);
            }

            randomGenome.MutateGenomeAndSave();

            currentProgress.ResetValues();
            currentProgress.NumberOfRounds++;
        }
        private void RunAIDuel()
        {
            int numberOfTurns = 0;
            while (checkerBoard.GetWinner() == null)
            {
                //AI vs AI
                CheckersMove aiMove = AiController.MinimaxStart(checkerBoard, Depth, Difficulty);
                if (aiMove != null && numberOfTurns++ < MaxTurns)
                {
                    while (aiMove != null)
                    {
                        MakeMove(aiMove);
                        aiMove = aiMove.NextMove;
                        Thread.Sleep(ConstantsSettings.TimeToSleepBetweenMoves);
                    }
                }
                else
                {
                    throw new AIException("A IA não encontrou possíveis jogadas para continuar o jogo..");
                }
            }
        }

        private void ButtonClickWork(object sender)
        {
            Button button = (Button)sender;
            CheckersSquareUserControl checkerSquareUC = (CheckersSquareUserControl)((Grid)button.Parent).Parent;
            Logger.Info("Row: " + checkerSquareUC.CheckersPoint.Row + " Column: " + checkerSquareUC.CheckersPoint.Column);
            DisableAllButtons();
            if (currentMove == null)
            {
                currentMove = new CheckersMove();
            }

            if (currentMove.SourcePoint == null)
            {
                currentMove.SourcePoint = checkerSquareUC.CheckersPoint;
                SetBackgroundColor(checkerSquareUC, Brushes.Green);

                currentAvailableMoves = checkerSquareUC.CheckersPoint.GetPossibleMoves(checkerBoard);

                currentAvailableMoves.Add(new CheckersMove(checkerSquareUC.CheckersPoint, checkerSquareUC.CheckersPoint));

                ColorBackgroundOfPoints(currentAvailableMoves, Brushes.Aqua);

                EnableButtonsWithPossibleMove(currentAvailableMoves);
            }
            else
            {
                currentMove.DestinationPoint = checkerSquareUC.CheckersPoint;
                SetBackgroundColor(checkerSquareUC, Brushes.Green);

                MakeMoveReturnModel returnModel = MakeMove(GetMoveFromList(checkerSquareUC.CheckersPoint));
                if (returnModel.WasMoveMade && returnModel.IsTurnOver && ConstantsSettings.IsAiGame)
                {
                    DisableAllButtons();

                    //AI needs to make a move now
                    CheckersMove aiMove = AiController.MinimaxStart(checkerBoard, Depth, Difficulty);
                    if (aiMove != null)
                    {
                        while (aiMove != null)
                        {
                            MakeMove(aiMove);
                            aiMove = aiMove.NextMove;
                            Thread.Sleep(ConstantsSettings.TimeToSleepBetweenMoves);
                        }
                    }
                }
            }
        }

        private CheckersMove GetMoveFromList(CheckersPoint checkersPoint)
        {
            foreach (CheckersMove move in currentAvailableMoves)
            {
                if (move.DestinationPoint.Equals(checkersPoint))
                {
                    return move;
                }
            }

            return null;
        }

        private MakeMoveReturnModel MakeMove(CheckersMove moveToMake)
        {
            bool moveWasMade = false;
            bool isTurnOver = false;
            CheckersPoint source = moveToMake.SourcePoint;
            CheckersPoint destination = moveToMake.DestinationPoint;

            Logger.Info("Piece1 " + source.Row + ", " + source.Column);
            Logger.Info("Piece2 " + destination.Row + ", " + destination.Column);

            if (source != destination)
            {
                isTurnOver = checkerBoard.MakeMoveOnBoard(moveToMake);

                CheckersSquareUserControl sourceUC = checkerBoard.BoardArray[source.Row][source.Column];
                CheckersSquareUserControl destUC = checkerBoard.BoardArray[destination.Row][destination.Column];

                sourceUC.UpdateSquare();
                destUC.UpdateSquare();

                moveWasMade = true;

                object winner = checkerBoard.GetWinner();
                if (winner != null && winner is PlayerColor winnerColor && !ConstantsSettings.RunningGeneticAlgo)
                {
                    string newWinner = winnerColor == PlayerColor.Red ? "Vermelha" : "Preta";
                    MessageBox.Show($"Peça {newWinner} é a ganhadora!!");

                    var  dialog = MessageBox.Show("Jogar de novo? Padrão de Dificuldade: Médio", "Reiniciar jogo", MessageBoxButton.YesNo);


                    if (dialog == MessageBoxResult.Yes)
                        this.ReiniciarJogo();
                    else
                        Environment.Exit(0);
                }
            }
            string currentTurn = checkerBoard.CurrentPlayerTurn == PlayerColor.Red ? "Vermelha" : "Preta";

            ColorBackgroundOfPoints(currentAvailableMoves, Brushes.Black);

            var dificuldade = "";

            switch (Difficulty)
            {
                case "hard":
                    dificuldade = "Difícil";
                    break;
                case "medium":
                    dificuldade = "Médio";
                    break;
                case "easy":
                    dificuldade = "Fácil";
                    break;
                default:
                    break;
            }

            SetTitle($"Jogo de Damas | Turno da Peça {currentTurn} | Dificuldade: {dificuldade}");
            EnableButtonsWithMove();
            currentMove = null;

            return new MakeMoveReturnModel()
            {
                IsTurnOver = isTurnOver,
                WasMoveMade = moveWasMade
            };
        }

        private void SetBackgroundColor(UserControl control, Brush colorToSet)
        => Application.Current.Dispatcher.BeginInvoke(
                      DispatcherPriority.Background,
                      new Action(() => control.Background = colorToSet));
        

        private void SetTitle(string titleToSet)
        => Application.Current.Dispatcher.BeginInvoke(
                      DispatcherPriority.Background,
                      new Action(() => this.Title = titleToSet));
        

        private void EnableButtonsWithMove()
        {
            List<CheckersMove> totalPossibleMoves = checkerBoard.GetMovesForPlayer();

            foreach (CheckersMove move in totalPossibleMoves)
            {
                CheckersPoint source = move.SourcePoint;
                int col = source.Column;
                int row = source.Row;

                CheckersSquareUserControl sourceUserControl = checkerBoard.BoardArray[row][col];

                Application.Current.Dispatcher.BeginInvoke(
                      DispatcherPriority.Background,
                      new Action(() => sourceUserControl.button.IsEnabled = true));
            }
        }

        private void EnableButtons<T>()
        {
            foreach (List<CheckersSquareUserControl> list in checkerBoard.BoardArray)
            {
                foreach (CheckersSquareUserControl squareUC in list)
                {
                    if (squareUC.CheckersPoint.Checker != null && squareUC.CheckersPoint.Checker is T)
                    {
                        squareUC.button.IsEnabled = true;
                    }
                }
            }
        }

        private void DisableAllButtons()
        {
            foreach (List<CheckersSquareUserControl> list in checkerBoard.BoardArray)
            {
                foreach (CheckersSquareUserControl squareUC in list)
                {
                    Application.Current.Dispatcher.BeginInvoke(
                      DispatcherPriority.Background,
                      new Action(() => squareUC.button.IsEnabled = false));
                }
            }
        }

        private void EnableAllButtons()
        {
            foreach (List<CheckersSquareUserControl> list in checkerBoard.BoardArray)
            {
                foreach (CheckersSquareUserControl squareUC in list)
                {
                    squareUC.button.IsEnabled = true;
                }
            }
        }

        private void ColorBackgroundOfPoints(List<CheckersMove> list, Brush backgroundColor)
        {
            if (list != null)
            {
                foreach (CheckersMove checkerPoint in list)
                {
                    Application.Current.Dispatcher.BeginInvoke(
                      DispatcherPriority.Background,
                      new Action(() => checkerBoard.BoardArray[checkerPoint.DestinationPoint.Row][checkerPoint.DestinationPoint.Column].Background = backgroundColor));
                }
            }
        }

        private void EnableButtonsWithPossibleMove(List<CheckersMove> list)
        {
            foreach (CheckersMove checkerPoint in list)
            {
                Application.Current.Dispatcher.BeginInvoke(
                      DispatcherPriority.Background,
                      new Action(() => checkerBoard.BoardArray[checkerPoint.DestinationPoint.Row][checkerPoint.DestinationPoint.Column].button.IsEnabled = true));
            }
        }

        private void UpdateSquares()
        {
            foreach (List<CheckersSquareUserControl> list in checkerBoard.BoardArray)
            {
                foreach (CheckersSquareUserControl squareUC in list)
                {
                    squareUC.UpdateSquare();
                }
            }
        }

        private void RestartGame(object sender, RoutedEventArgs e)
        {
            if (aiThread != null)
            {
                aiThread.Abort();
            }

            InitializeCheckers();
        }

        private void ExitGame(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void DificuldadeFacil(object sender, RoutedEventArgs e) 
        {
            if (aiThread != null)
            {
                aiThread.Abort();
            }

            this.Difficulty = "easy";
            this.Depth = 2;
            InitializeCheckers();
        }

        private void DificuldadeMedio(object sender, RoutedEventArgs e) 
        {
            if (aiThread != null)
            {
                aiThread.Abort();
            }

            this.Difficulty = "medium";
            this.Depth = 4;
            InitializeCheckers();
        }

        private void DificuldadeDificil(object sender, RoutedEventArgs e) 
        {
            if (aiThread != null)
            {
                aiThread.Abort();
            }

            this.Difficulty = "hard";
            this.Depth = 8;
            InitializeCheckers();
        }
		
		private void ReiniciarJogo()
        {
            if (aiThread != null)
            {
                aiThread.Abort();
            }

            this.Difficulty = "medium";
            this.Depth = 4;
            InitializeCheckers();
        }
    }
}
