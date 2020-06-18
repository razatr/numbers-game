using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
                try
                {
                    min_number = int.Parse(MinNumberText.Text);
                    max_number = int.Parse(MaxNumberText.Text);
                    max_steps = int.Parse(NumberOfStepsText.Text);
                    honesty = int.Parse(ComputerHonestyText.Text);
                }
                catch(FormatException err)
                {
                    StatusBar.Text = err.Message;
                    return;
                }

                if(min_number > max_number)
                {
                    StatusBar.Text = "Максимальное число должно быть больше минимального!";
                    return;
                }

                number = rnd.Next(min_number, max_number);

                difficulty = ((max_number - min_number + 1) / Convert.ToInt32(Math.Pow(2, max_steps))) *(2 - Math.Abs(honesty - 50)/50);
                StatusBar.Text = "";
                StartGame();
            }
            else
            {
                StatusBar.Text = "";
                EndGame();
            }
        }

        private void SendAnswer(object sender, RoutedEventArgs e)
        {
            int answer;
            try
            {
                answer = int.Parse(AnswerText.Text);
            }
            catch (FormatException err)
            {
                StatusBar.Text = err.Message;
                return;
            }
            AnswerText.Text = "";
            int random = rnd.Next(0, 100);

            if (answer > max_number || answer < min_number)
            {
                StatusBar.Text = "Введите число из диапазона между " + min_number.ToString() + " и " + max_number.ToString();
                return;
            }

            StatusBar.Text = "";

            step++;


            if (answer == number)
            {
                //Верный ответ
                FeedbackText.Text = "Верный ответ";
                StatusBar.Text = "Победа, попробуйте усложнить настройки";
                EndGame();
                return;
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

        void ChangeSliderFromText(int min, int max, Slider slider, TextBox text_box)
        {
            int value;
            try
            {
                value = int.Parse(text_box.Text);
            }
            catch (FormatException err)
            {
                StatusBar.Text = err.Message;
                return;
            }
            if (value <= max && value >= min)
            {
                    slider.Value = value;
            }
            if (StatusBar != null)
                StatusBar.Text = "";
        }

        void LostFocusOnText(int min, int max, TextBox text_box)
        {
            int value;
            try
            {
                value = int.Parse(text_box.Text);
            }
            catch (FormatException err)
            {
                StatusBar.Text = err.Message;
                return;
            }

            if(value > max)
            {
                text_box.Text = max.ToString();
            }
            if (value < min)
            {
                text_box.Text = min.ToString();
            }
        }

        private void ChangeNumberOfStepsText(object sender, TextChangedEventArgs e)
        {
            ChangeSliderFromText(2, 20, NumberOfStepsSlider, NumberOfStepsText);
        }

        private void NumberOfStepsLostFocus(object sender, RoutedEventArgs e)
        {
            LostFocusOnText(2, 20, NumberOfStepsText);
        }

        private void ChangeMinText(object sender, TextChangedEventArgs e)
        {
            ChangeSliderFromText(0, 1000, MinNumberSlider, MinNumberText);
        }

        private void MinNumberLostFocus(object sender, RoutedEventArgs e)
        {
            LostFocusOnText(0, 1000, MinNumberText);
        }

        private void MaxNumberLostFocus(object sender, RoutedEventArgs e)
        {
            LostFocusOnText(1, 1000, MaxNumberText);
        }

        private void ChangeMaxText(object sender, TextChangedEventArgs e)
        {
            ChangeSliderFromText(0, 1000, MaxNumberSlider, MaxNumberText);
        }

        private void ChangeHonestlyText(object sender, TextChangedEventArgs e)
        {
            ChangeSliderFromText(0, 100, ComputerHonestySlider, ComputerHonestyText);
        }

        private void HonestyLostFocus(object sender, RoutedEventArgs e)
        {
            LostFocusOnText(0, 100, ComputerHonestyText);
        }
    }
}
