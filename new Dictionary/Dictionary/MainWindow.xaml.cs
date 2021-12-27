using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Dictionary
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const string path = "Input/UkrainianDictionary.txt";
        SmartDictionary smartDictionary;

        public MainWindow()
        {
            InitializeComponent();
            smartDictionary = new SmartDictionary(path);
        }

        private void InputTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string input_word = InputTextBox.Text;
            ShowSimilarWords(input_word);
        }

        private void AddToTheDictionary_Button_Click(object sender, RoutedEventArgs e)
        {
            string input_word = InputTextBox.Text;
            if (smartDictionary.Contains(input_word))
            {
                MessageBox.Show("Word is already in the dictionary!");
            }
            else
            {
                smartDictionary.AddWordToTheDictionary(input_word);
                ShowSimilarWords(input_word);
            }
        }

        private void ShowSimilarWords(string input_word)
        {
            if (smartDictionary.Contains(input_word))
            {
                InputTextBox.Foreground = Brushes.Black;
            }
            else
            {
                InputTextBox.Foreground = Brushes.Red;
            }

            List<string> similar = smartDictionary.FindClosestWords(input_word, 4);
            SimilarWords_TextBlock.Text = string.Join("\n", similar);
        }

    }
}
