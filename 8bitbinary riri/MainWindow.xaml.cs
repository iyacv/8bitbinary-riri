using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
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

namespace _8bitbinary_riri
{
  
    public partial class MainWindow : Window
    {
        private int currentScore = 0;
        private int timeInterval = 60;
        private int roundNumber = 1;
        private DispatcherTimer timer;
        private List<TextBox> textBoxes;

        public MainWindow()
        {
            InitializeComponent();
            InitializeGame();
        }

        private void InitializeGame()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(timeInterval);
            timer.Tick += Timer_Tick;

            textBoxes = new List<TextBox> { textBox1, textBox2, textBox3, textBox4, textBox5, textBox6, textBox7, textBox8 };

            StartNewRound();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            timerTextBlock.Text = "Time Left: 0s";
            GameOver();
        }

        private void StartNewRound()
        {
            Random rnd = new Random();
            int randomNumber = rnd.Next(256); // Generate a random 8-bit number (0-255)
            randomNumberTextBlock.Text = randomNumber.ToString();

            foreach (TextBox textBox in textBoxes)
            {
                textBox.Text = "0"; // Reset all text boxes to 0
            }

            timer.Stop();
            timeInterval -= (int)(timeInterval * 0.066); // Reduce time interval for next round
            if (timeInterval <= 0) timeInterval = 0;
            timer.Interval = TimeSpan.FromSeconds(timeInterval);
            timer.Start();
            timerTextBlock.Text = $"Time Left: {timeInterval}s";
        }

        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            int index = int.Parse(button.Tag.ToString()) - 1; // Get the index of corresponding textbox
            TextBox textBox = textBoxes[index];
            int currentValue = int.Parse(textBox.Text);
            textBox.Text = (currentValue == 0) ? "1" : "0"; // Toggle between 0 and 1
            // Check the answer after each selection
            CheckAnswer();
        }

        private void CheckAnswer()
        {
            string binaryNumber = "";
            foreach (TextBox textBox in textBoxes)
            {
                binaryNumber += textBox.Text;
            }
            int decimalNumber = Convert.ToInt32(binaryNumber, 2);
            int correctAnswer = int.Parse(randomNumberTextBlock.Text);

            if (decimalNumber == correctAnswer)
            {
                currentScore++;
                scoreTextBlock.Text = $"Score: {currentScore}";
                StartNewRound();
            }
        }

        private void GameOver()
        {
            MessageBox.Show($"Game Over! Your final score is: {currentScore}");
            timer.Stop();
            currentScore = 0;
            timeInterval = 60;
            roundNumber = 1;
            scoreTextBlock.Text = "Score: 0";
            timerTextBlock.Text = "Time Left: 60s";
            StartNewRound();
        }
    }
}