using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Verko41
{
    public partial class AuthPage : Page
    {
        private string generatedCaptcha;
        private bool isCaptcha = false;
        private bool isCaptchaValid = true;

        public AuthPage()
        {
            InitializeComponent();
            captchaPanel.Visibility = Visibility.Hidden;
            captchaInputTB.Visibility = Visibility.Hidden;
        }

        private async void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTB.Text;
            string password = PasswordTB.Text;
            if (login == "" || password == "")
            {
                MessageBox.Show("Заполните пустые поля");
                return;
            }

            if (isCaptcha)
            {
                CaptchaCheck(captchaInputTB.Text);
            }

            User user = Verko41Entities.GetContext().User.ToList().Find(p => p.UserLogin == login && p.UserPassword == password);

            if (user != null && isCaptchaValid)
            {
                Manager.MainFrame.Navigate(new ProductPage(user));
                ClearInputFields();
                captchaPanel.Visibility = Visibility.Hidden;
                captchaInputTB.Visibility = Visibility.Hidden;
            }
            else
            {
                MessageBox.Show("Введены неверные данные");
                if (isCaptcha)
                {
                    LoginBtn.IsEnabled = false;
                    await Task.Delay(10000);
                    LoginBtn.IsEnabled = true;
                }
                isCaptcha = true; 
                ClearInputFields();
                ShowCaptcha();
                
            }
        }

        private void ShowCaptcha()
        {
            captchaPanel.Visibility = Visibility.Visible;
            captchaInputTB.Visibility = Visibility.Visible;
            generatedCaptcha = GenerateCaptcha();

            captchaOneWord.Text = generatedCaptcha[0].ToString();
            captchaTwoWord.Text = generatedCaptcha[1].ToString();
            captchaThreeWord.Text = generatedCaptcha[2].ToString();
            captchaFourWord.Text = generatedCaptcha[3].ToString();
        }

        private string GenerateCaptcha()
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            char[] captchaChars = new char[4];
            for (int i = 0; i < 4; i++)
            {
                captchaChars[i] = chars[random.Next(chars.Length)];
            }
            return new string(captchaChars);
        }



        private async void CaptchaCheck(string inputCaptcha)
        {
            if (inputCaptcha != generatedCaptcha)
            {
                ClearInputFields();
                isCaptchaValid = false;
                LoginBtn.IsEnabled = false;
                await Task.Delay(10000);
                LoginBtn.IsEnabled = true;
            }
            else
            {
               isCaptchaValid = true;
            }
        }

        private void ClearInputFields()
        {
            LoginTB.Text = "";
            PasswordTB.Text = "";
            captchaInputTB.Text = "";
        }

        private void GuestLogin_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new ProductPage(null));
            captchaPanel.Visibility = Visibility.Hidden;
            captchaInputTB.Visibility = Visibility.Hidden;
        }
    }
}

