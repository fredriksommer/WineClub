using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Windows.Input;
using WineClub.Contracts.Services;
using WineClub.Contracts.ViewModels;
using WineClub.Core.Contracts.Services;
using WineClub.Core.Dtos;
using WineClub.Helpers;
using WineClub.Services;

namespace WineClub.ViewModels
{
    public class LoginViewModel : ObservableValidator, INavigationAware
    {
        private readonly INavigationService _navigationService;
        private readonly UserLoggedInService _userLoggedInService;
        private readonly IUserService _userService;

        private readonly List<ValidationResult> _errors = new();

        public LoginViewModel(INavigationService navigationService, UserLoggedInService userLoggedInService, IUserService userService)
        {
            _navigationService = navigationService;
            _userLoggedInService = userLoggedInService;
            _userService = userService;
        }

        public string Errors => string.Join(Environment.NewLine, from ValidationResult e in _errors select e.ErrorMessage);
        public bool ErrorsHaveI => Errors.Length > 0;
        public bool EnableButton => Errors.Length <= 0 && Username != null && Password != null;

        private ICommand _loginCommand;
        /// <summary>
        /// Login command, including login logic.
        /// </summary>
        public ICommand LoginCommand
        {
            get
            {
                if (_loginCommand == null)
                {
                    _loginCommand = new RelayCommand(async () =>
                    {
                        byte[] salt = new byte[128 / 8];
                        using (RNGCryptoServiceProvider rngCsp = new())
                        {
                            rngCsp.GetNonZeroBytes(salt);
                        }

                        if (Password != null)
                        {
                            UserDto userCheck = new() { Username = Username, Password = Password };
                            try
                            {
                                IEnumerable<UserDto> users = await _userService.GetUsersAsync();
                                UserDto userExist = users.ToList().FirstOrDefault(x => x.Username == Username);
                                bool passwordChecker = false;


                                if (userExist != null)
                                {
                                    byte[] decodedHashedPassword = Convert.FromBase64String(userExist.Password);

                                    passwordChecker = PasswordHasher.VerifyHashedPasswordV2(decodedHashedPassword, Password);
                                }

                                if (userExist != null & passwordChecker)
                                {
                                    _userLoggedInService.LogIn(userExist);
                                    _navigationService.GoBack();
                                }
                                else
                                {
                                    ContentDialog dialog = new()
                                    {
                                        Title = "Unable to login",
                                        Content = "Username or password is wrong",
                                        PrimaryButtonText = "Try again",
                                        XamlRoot = _navigationService.Frame.XamlRoot,
                                    };
                                    ContentDialogResult result = await dialog.ShowAsync();
                                }
                            }
                            catch (HttpRequestException)
                            {
                                ContentDialog dialog = new()
                                {
                                    Title = "Error",
                                    Content = "Something went wrong!",
                                    PrimaryButtonText = "Try again",
                                    XamlRoot = _navigationService.Frame.XamlRoot,
                                };
                                ContentDialogResult result = await dialog.ShowAsync();
                            }
                        }
                    }
                    );
                }
                return _loginCommand;
            }

        }

        private ICommand _signUpCommand;
        /// <summary>
        /// Sign up command.
        /// </summary>
        public ICommand SignUpCommand
        {
            get
            {
                if (_signUpCommand == null)
                {
                    _signUpCommand = new RelayCommand(() =>
                    {
                        _navigationService.NavigateTo(typeof(SignUpViewModel).FullName);
                    }
                    );
                }
                return _signUpCommand;
            }

        }

        private string _username;

        /// <summary>
        /// Gets or sets users username.
        /// </summary>
        [Required(ErrorMessage = "Username is Required")]
        public string Username
        {
            get => _username;
            set
            {
                _errors.RemoveAll(v => v.MemberNames.Contains(nameof(Username)));

                TrySetProperty(ref _username, value, out IReadOnlyCollection<ValidationResult> errors);

                _errors.AddRange(errors);
                OnPropertyChanged(nameof(Errors));
                OnPropertyChanged(nameof(ErrorsHaveI));
                OnPropertyChanged(nameof(EnableButton));
            }
        }

        private string _password;

        /// <summary>
        /// Gets or sets users password.
        /// </summary>
        [Required(ErrorMessage = "Password is Required")]
        public string Password
        {
            get => _password;
            set
            {
                _errors.RemoveAll(v => v.MemberNames.Contains(nameof(Password)));

                TrySetProperty(ref _password, value, out IReadOnlyCollection<ValidationResult> errors);

                _errors.AddRange(errors);
                OnPropertyChanged(nameof(Errors));
                OnPropertyChanged(nameof(ErrorsHaveI));
                OnPropertyChanged(nameof(EnableButton));
            }
        }

        private string _hashedPassword;
        /// <summary>
        /// Gets or sets hashed password.
        /// </summary>
        public string HashedPassword
        {
            get => _hashedPassword;
            set => SetProperty(ref _hashedPassword, value);
        }

        public void OnNavigatedFrom()
        {

        }

        public void OnNavigatedTo(object parameter)
        {

        }
    }
}
