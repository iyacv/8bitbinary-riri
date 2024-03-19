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
using System.Windows.Shapes;

namespace _8bitbinary_riri
{
    public partial class TopScoresWin : Window
    {
        public TopScoresWin(List<(string playerName, int score, int totalPlayTime)> topPlayerScores, int recentPlayerScore)
        {
            InitializeComponent();

            // Populate the list box with top player scores
            foreach ((string playerName, int score, int totalPlayTime) in topPlayerScores)
            {
                // Convert total playtime to minutes and seconds
                int minutes = totalPlayTime / 60;
                int seconds = totalPlayTime % 60;

                // Display total playtime in minutes and seconds format
                string totalTimeFormatted = $"{minutes} m. {seconds} sec/s.";

                topScoresListBox.Items.Add($"{playerName} - Score: {score}, Total Play Time: {totalTimeFormatted}");
            }
     
            currentPlayerScoreTextBox.Text = $"Your Score: {recentPlayerScore}";
        }
    }
}
