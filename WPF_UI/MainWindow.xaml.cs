using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace WPF_UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private enum GameCellValues
        {
            Empty = 0,
            X = 1,
            O = 2            
        }

        private enum GameStatus
        {
            Playing = 0,
            Player1Won = 1,
            Player2Won = 2,
            Tie = 3
        }
        
        private int[] gameGrid = new int[9];
        private bool player1Turn = true;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void gameButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button == null) return;

            String tag = button.Tag.ToString();
            if (String.IsNullOrEmpty(tag)) return;

            int cell = Convert.ToInt32(tag);
            int index = cell - 1;

            GameStatus gameStatus = CheckGameSituation();

            if (gameStatus != GameStatus.Playing) ResetGame();
            else if (PlayTurn(index))
            {
                gameStatus = CheckGameSituation(); // Update to send the right message when winning
                UpdateGameVisuals(button, index, gameStatus);
            }
        }

        private bool PlayTurn(int index)
        {
            if (gameGrid[index] != (int)GameCellValues.Empty) return false;
            
            gameGrid[index] = (int)(player1Turn ? GameCellValues.X : GameCellValues.O);
            player1Turn = !player1Turn;

            return true;
        }

        private GameStatus CheckGameSituation()
        {
            int lengthSquared = (int)Math.Sqrt(gameGrid.Length); // Should not be a problem (most of the time) (as long as the game grid is 3x3)

            // Check Tie
            {
                bool tie = true;
                for (int i = 0; i < gameGrid.Length - 1; i++)
                {
                    if (gameGrid[i] == (int)GameCellValues.Empty)
                    {
                        tie = false;
                        break;
                    }
                }

                if (tie) return GameStatus.Tie;
            }

            // Check Rows
            for (int i = 0; i < lengthSquared; i++)
            {
                if (gameGrid[(i * lengthSquared)] == gameGrid[(i * lengthSquared) + 1] && gameGrid[(i * lengthSquared) + 1] == gameGrid[(i * lengthSquared) + 2]) return (GameStatus)gameGrid[(i * lengthSquared) + 1];
            }

            // Check Columns
            for (int i = 0; i < lengthSquared; i++)
            {
                if (gameGrid[i] == gameGrid[i+lengthSquared] && gameGrid[i + lengthSquared] == gameGrid[i + 2*lengthSquared]) return (GameStatus)gameGrid[i];
            }

            // Check Diagonals
            if ((gameGrid[0] == gameGrid[4] && gameGrid[4] == gameGrid[8]) || (gameGrid[2] == gameGrid[4] && gameGrid[4] == gameGrid[6])) return (GameStatus)gameGrid[0];


            return GameStatus.Playing;
        }

        private void UpdateGameVisuals(Button button, int lastIndex, GameStatus status)
        {
            button.Content = (gameGrid[lastIndex] == (int)GameCellValues.X ? "X" : "O");

            switch (status)
            {
                case GameStatus.Playing:
                    textBlockGameStatus.Text = player1Turn ? "Player 1 turn" : "Player 2 turn";
                    break;

                case GameStatus.Player1Won:
                    textBlockGameStatus.Text = "Player 1 Wins!!!";
                    break;

                case GameStatus.Player2Won:
                    textBlockGameStatus.Text = "Player 2 Wins!!!";
                    break;

                case GameStatus.Tie:
                    textBlockGameStatus.Text = "It's a tie.";
                    break;
            }            
        }

        private void ResetGame()
        {
            for (int i = 0; i < gameGrid.Length; i++)
            {
                gameGrid[i] = (int)GameCellValues.Empty;
            }

            textBlockGameStatus.Text = "Player 1 turn";

            gameButton1.Content = "-";
            gameButton2.Content = "-";
            gameButton3.Content = "-";
            gameButton4.Content = "-";
            gameButton5.Content = "-";
            gameButton6.Content = "-";
            gameButton7.Content = "-";
            gameButton8.Content = "-";
            gameButton9.Content = "-";
        }
    }
}
