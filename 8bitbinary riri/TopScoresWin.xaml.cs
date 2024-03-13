﻿using System;
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
    /// <summary>
    /// Interaction logic for TopScoresWin.xaml
    /// </summary>
    public partial class TopScoresWin : Window
    {
        public TopScoresWin(List<(string playerName, int score, int totalPlayTime)> topPlayerScores)
        {
            InitializeComponent();

            // Populate the list box with top player scores
            foreach (var (playerName, score, totalPlayTime) in topPlayerScores)
            {
                topScoresListBox.Items.Add($"{playerName} - Score: {score}, Total Play Time: {totalPlayTime} seconds");
            }
        }
    }
}
    