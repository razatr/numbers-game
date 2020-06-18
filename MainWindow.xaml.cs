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
using System.IO;

namespace Numbers
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        static Random rnd = new Random();

        static int min_number = 0;
        static int max_number = 1;
        static int step = 0;
        static int max_steps = 2;
        static int honesty = 100;
        static int number = rnd.Next(0, 1);
        static int difficulty = 1;

        void StartGame()
        {
            GameSettings.IsEnabled = false;
            OnPlay.IsEnabled = true;
            if(difficulty <= 10)
                LetsPlayButton.Content = "Прекратить";
            else if(difficulty <= 30)
                LetsPlayButton.Content = "Сложновато, другие настройки!";
            else if(difficulty <= 100)
                LetsPlayButton.Content = "Я не Ванга, перенастроить";
            else if (difficulty <= 1000)
            {
                LetsPlayButton.Content = "Остановить это безумие";
                StatusBar.Text = "Если вы сможете победить, считайте что вы прошли эту игру!";
            }
        }

        void EndGame()
        {
            GameSettings.IsEnabled = true;
            OnPlay.IsEnabled = false;
            LetsPlayButton.Content = "Играть";
            StatusBar.Text = "";
        }

        private void ComputerHonestySliderInit(object sender, EventArgs e) 
        {
            (sender as Slider).Value = 100;
        }

        private void ChangeStepNumber(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            (sender as Slider).Value = Math.Round(e.NewValue, 0);
            if (NumberOfStepsText != null)
                NumberOfStepsText.Text = (sender as Slider).Value.ToString();
        }

        private void ChangeMinNumber(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            (sender as Slider).Value = Math.Round(e.NewValue, 0);
            if (MinNumberText != null)
                MinNumberText.Text = (sender as Slider).Value.ToString();
        }

        private void ChangeMaxNumber(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            (sender as Slider).Value = Math.Round(e.NewValue, 0);
            if (MaxNumberText != null)
                MaxNumberText.Text = (sender as Slider).Value.ToString();
        }

        private void ChangeComputerHonesty(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            (sender as Slider).Value = Math.Round(e.NewValue, 0);
            if (ComputerHonestyText != null)
                ComputerHonestyText.Text = (sender as Slider).Value.ToString();
        }

        private void ComputerHonestyHint(object sender, MouseEventArgs e)
        {
            StatusBar.Text = "Определяет вероятность, с которой компьютер даст правильную подсказку. Поставьте 50, чтобы насладится игрой полностью)";
        }

        private void ComputerHonestyHintOff(object sender, MouseEventArgs e)
        {
            StatusBar.Text = "";
        }

        private void LetsPlay(object sender, RoutedEventArgs e)
        {
            if((sender as Button).Content.ToString() == "Играть")
            {
                min_number = int.Parse(MinNumberText.Text);
                max_number = int.Parse(MaxNumberText.Text);
                max_steps = int.Parse(NumberOfStepsText.Text);
                honesty = int.Parse(ComputerHonestyText.Text);
                number = rnd.Next(min_number, max_number);
                difficulty = ((max_number - min_number + 1) / Convert.ToInt32(Math.Pow(2, max_steps))) *(2 - Math.Abs(honesty - 50)/50);
                StartGame();
            }
            else
            {
                EndGame();
            }
        }

        private void SendAnswer(object sender, RoutedEventArgs e)
        {
            int answer = int.Parse(AnswerText.Text);
            AnswerText.Text = "";
            int random = rnd.Next(0, 100);
            StatusBar.Text = random.ToString();
            step++;

            if (answer == number)
            {
                //Верный ответ
                FeedbackText.Text = "Верный ответ";
                EndGame();
            }

            else if(answer > number && random <= honesty)
            {
                //Число меньше, чем введенное
                FeedbackText.Text = "Загаданное число меньше, чем введенное";
            }

            else
            {
                //Число больше, чем введенное
                FeedbackText.Text = "Загаданное число больше, чем введенное";
            }

            if(step == max_steps)
            {
                FeedbackText.Text = "Вы проиграли. Загаданное число - " + number.ToString();
                EndGame();
            }
        }

    }
}
