using System;
using System.Collections.Generic;
using System.IO;
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
using System.Media;
using System.Threading;

namespace _8bitbinary_riri
{

    public partial class MainWindow : Window
    {
        private int currentScore = 0;
        private int timeInterval = 60;
        private int roundNumber = 1;
        private string playerName;
        private int totalPlayTimeInWhole = 0;
        private DispatcherTimer timer;
        private DispatcherTimer totalPlayTimeTimer;
        private List<TextBox> textBoxes;
       // private SoundPlayer Sound_bgmusic;
        private SoundPlayer Sound_selectbutton;
        //private SoundPlayer Sound_countdown10sec;

        public MainWindow(string playerName)
        {
            InitializeComponent();
            this.playerName = playerName;

            //Sound_bgmusic = new SoundPlayer(@"C:\Users\Riann\Downloads\8bitbgmusic.wav");
            Sound_selectbutton = new SoundPlayer(@"C:\Users\Riann\Downloads\8bitselectbutton.wav");
            InitializeGame();
        }

        private void InitializeGame()
        {
          
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1); // Set the timer interval to 1 second
            timer.Tick += Timer_Tick;

            totalPlayTimeTimer = new DispatcherTimer();
            totalPlayTimeTimer.Interval = TimeSpan.FromSeconds(1); 
            totalPlayTimeTimer.Tick += TotalPlayTimeTimer_Tick;

            textBoxes = new List<TextBox> { textBox1, textBox2, textBox3, textBox4, textBox5, textBox6, textBox7, textBox8 };

            StartNewRound();
            totalPlayTimeTimer.Start();
            
            
        }


        private void Timer_Tick(object sender, EventArgs e)
        {
            timeInterval--;

            int minutes = timeInterval / 60;
            int seconds = timeInterval % 60;
          
            string formattedTime = $"{minutes:00}:{seconds:00}";
            //update oras
            timerTextBlock.Text = $"Time Left: {formattedTime}";

            if (timeInterval <= 0)
            {
                GameOver(); 
            }
        }

        private void TotalPlayTimeTimer_Tick(object sender, EventArgs e)
        {
            totalPlayTimeInWhole++; //increment!!!
        }


        //////for checking lang para mabilis process to input binary
        //private string DecimalToBinary(int decimalNumber)
        //{
        //    // Convert decimal to binary
        //    string binary = Convert.ToString(decimalNumber, 2);

        //    // Pad with zeros to ensure it's 8 bits long
        //    binary = binary.PadLeft(8, '0');

        //    return binary;
        //}


        private void StartNewRound()
        {
            Random rnd = new Random();
            int randomNumber = rnd.Next(256); 
            randomNumberTextBlock.Text = randomNumber.ToString();

            //////// Convert  random number to its 8-bit binary representation
            //string binaryNumber = DecimalToBinary(randomNumber);
            //answerTextBox.Text = binaryNumber;

            foreach (TextBox textBox in textBoxes)
            {
                textBox.Text = "0"; // Reset all text boxes to 0
            }

            timer.Stop();

            if (roundNumber <= 11)
            {            
                int reductionAmount = Math.Max(60 - (roundNumber - 1) * 4, 20);
                timeInterval = reductionAmount;
            }

            else
            {
                // After round 11, keep the timer at 20 seconds
                timeInterval = 20;
            }

            timer.Interval = TimeSpan.FromSeconds(1); 
            timer.Start();
            timerTextBlock.Text = $"Time Left: {timeInterval}s";

            roundNumberTextBlock.Text = $"Round {roundNumber}"; //pang update  sa wpf para makita ano round

            roundNumber++;
        }

        private void SelectButton_Click(object sender, RoutedEventArgs e)

        {
            Button button = (Button)sender;
            int index = int.Parse(button.Tag.ToString()) - 1; // kunin index ng corresponding textbox
            TextBox textBox = textBoxes[index];

            if (textBox.Text == "0")
            {
                textBox.Text = "1";
            }
            else
            {
                textBox.Text = "0";
            }

            CheckAnswer();
            Sound_selectbutton.Play();
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

            int scoreToAdd = 0;
            if (decimalNumber == correctAnswer)
            {
                if (roundNumber >= 1 && roundNumber <= 5)
                {
                    scoreToAdd = 2;
                }
                else if (roundNumber >= 6 && roundNumber <= 10)
                {
                    scoreToAdd = 3;
                }
                else
                {
                    scoreToAdd = 4;
                }
                currentScore += scoreToAdd;
                scoreTextBlock.Text = $"Score: {currentScore}";
                StartNewRound();
            }
        }

        private List<(string playerName, int score, int totalPlayTime)> LoadTopPlayerScores()
        {
            string csvFilePath = "player_scores.csv";
            List<(string playerName, int score, int totalPlayTime)> topPlayerScores = new List<(string playerName, int score, int totalPlayTime)>();

            if (File.Exists(csvFilePath))
            {
                // Read all lines from the CSV file
                string[] lines = File.ReadAllLines(csvFilePath);

                // Parse each line to extract player data
                foreach (string line in lines)
                {
                    string[] parts = line.Split(',');
                    string playerName = parts[0];
                    int score = int.Parse(parts[1]);
                    int totalPlayTime = int.Parse(parts[2]);

                    topPlayerScores.Add((playerName, score, totalPlayTime));
                }
            }
            // Sort 
            for (int i = 0; i < topPlayerScores.Count - 1; i++)
            {
                for (int j = i + 1; j < topPlayerScores.Count; j++)
                {
                    if (topPlayerScores[j].score > topPlayerScores[i].score)
                    {
                        (string playerName, int score, int totalPlayTime) temp = topPlayerScores[i];
                        topPlayerScores[i] = topPlayerScores[j];
                        topPlayerScores[j] = temp;
                    }
                }
            }

            return topPlayerScores.Take(10).ToList(); 
        }

        private void GameOver()
        {
            SavePlayerDataToCSV(playerName, currentScore, totalPlayTimeInWhole);

            // Load top 10 player scores 
            List<(string playerName, int score, int totalPlayTime)> topPlayerScores = LoadTopPlayerScores();

            TopScoresWin topScoresWin = new TopScoresWin(topPlayerScores, currentScore);
            topScoresWin.ShowDialog();

            ResetGameState();
        }

        private void SavePlayerDataToCSV(string playerName, int score, int totalPlayTimeInWhole)
        {
            string csvFilePath = "player_scores.csv";

            // append to the CSV file
            using (StreamWriter sw = new StreamWriter(csvFilePath, true))
            {
                sw.WriteLine($"{playerName},{score},{totalPlayTimeInWhole}");
            }
        }

        private void ResetGameState()
        {
            currentScore = 0;
            timeInterval = 60;
            roundNumber = 1;

            scoreTextBlock.Text = "Score: 0";
            timerTextBlock.Text = "Time Left: 00:00";
            answerTextBox.Text = "";

            StartNewRound();
            
        }

    }
}