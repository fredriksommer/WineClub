using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows.Input;
using WineClub.Contracts.Services;
using WineClub.Contracts.ViewModels;
using WineClub.Core.Contracts.Services;
using WineClub.Core.Dtos;
using WineClub.Helpers;

namespace WineClub.ViewModels
{
    public class SignUpViewModel : ObservableValidator, INavigationAware
    {
        private readonly INavigationService _navigationService;
        private readonly IUserService _userService;
        private readonly List<ValidationResult> _errors = new();
        private static readonly List<UserDto> _users = new();

        public SignUpViewModel(INavigationService navigationService, IUserService userService)
        {
            _navigationService = navigationService;
            _userService = userService;
            GetUsers();

        }

        private string _firstName;

        /// <summary>
        /// Gets or sets first name.
        /// </summary>
        [Required(ErrorMessage = "First Name is Required")]
        [MinLength(2, ErrorMessage = "First Name should be longer than one character")]
        public string FirstName
        {
            get => _firstName;
            set
            {
                _errors.RemoveAll(v => v.MemberNames.Contains(nameof(FirstName)));

                TrySetProperty(ref _firstName, value, out IReadOnlyCollection<ValidationResult> errors);

                _errors.AddRange(errors);
                OnPropertyChanged(nameof(Errors));
                OnPropertyChanged(nameof(ErrorsHaveI));
                OnPropertyChanged(nameof(ShowButton));
            }
        }

        private string _lastName;

        /// <summary>
        /// Gets or sets last name.
        /// </summary>
        [Required(ErrorMessage = "Last Name is Required")]
        [MinLength(2, ErrorMessage = "Last Name should be longer than one character")]
        public string LastName
        {
            get => _lastName;
            set
            {
                _errors.RemoveAll(v => v.MemberNames.Contains(nameof(LastName)));

                TrySetProperty(ref _lastName, value, out IReadOnlyCollection<ValidationResult> errors);

                _errors.AddRange(errors);
                OnPropertyChanged(nameof(Errors));
                OnPropertyChanged(nameof(ErrorsHaveI));
                OnPropertyChanged(nameof(ShowButton));
            }
        }

        private string _password;

        /// <summary>
        /// Gets or sets password
        /// </summary>
        [Required(ErrorMessage = "Password is Required")]
        [MinLength(2, ErrorMessage = "Password should be longer than five characters")]
        public string Password
        {
            get => _password;
            set
            {
                _errors.RemoveAll(v => v.MemberNames.Contains(nameof(Password)));

                TrySetProperty(ref _password, value, out IReadOnlyCollection<ValidationResult> errors);
                ValidateProperty(Password2, nameof(Password2));

                _errors.AddRange(errors);
                OnPropertyChanged(nameof(Errors));
                OnPropertyChanged(nameof(ErrorsHaveI));
                OnPropertyChanged(nameof(ShowButton));
            }
        }

        private string _password2;

        /// <summary>
        /// Gets or sets password2 - used to compare with password.
        /// </summary>
        [Required(ErrorMessage = "Password is Required")]
        [MinLength(2, ErrorMessage = "Password should be longer than five characters")]
        [PasswordCheck(nameof(Password), "Password do not match")]
        public string Password2
        {
            get => _password2;
            set
            {
                _errors.RemoveAll(v => v.MemberNames.Contains(nameof(Password2)));

                TrySetProperty(ref _password2, value, out IReadOnlyCollection<ValidationResult> errors);

                _errors.AddRange(errors);
                OnPropertyChanged(nameof(Errors));
                OnPropertyChanged(nameof(ErrorsHaveI));
                OnPropertyChanged(nameof(ShowButton));
            }
        }



        private string _username;

        /// <summary>
        /// Gets or sets username.
        /// </summary>
        [Required(ErrorMessage = "Username is Required")]
        [MinLength(3, ErrorMessage = "Username should be longer than two characters")]
        [UsernameCheck(nameof(Username), "Try another username")]
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
                OnPropertyChanged(nameof(ShowButton));
            }
        }

        // For validation.
        public string Errors => string.Join(Environment.NewLine, from ValidationResult e in _errors select e.ErrorMessage);

        public bool ErrorsHaveI => Errors.Length > 0;

        public bool ShowButton => Errors.Length <= 0 && Username != null && Password == Password2 && Password != null;


        private async void GetUsers()
        {
            IEnumerable<UserDto> users = await _userService.GetUsersAsync();

            foreach (UserDto user in users)
            {
                _users.Add(user);
            }

        }

        private ICommand _createUserCommand;
        /// <summary>
        /// Command to create new user
        /// </summary>
        public ICommand CreateUserCommand
        {
            get
            {
                if (_createUserCommand == null)
                {
                    _createUserCommand = new RelayCommand(async () =>
                    {
                        PasswordHasher passwordHasher = new();

                        UserDto userDto = new()
                        {
                            Username = Username,
                            FirstName = FirstName,
                            LastName = LastName,
                            Password = passwordHasher.HashPassword(Password),
                            Role = Models.UserRoles.User

                        };

                        UserDto created = await _userService.CreateUserAsync(userDto);

                        if (created.Username.Equals(Username))
                        {
                            ContentDialog dialog = new()
                            {
                                Title = "User Successfully Created!",
                                PrimaryButtonText = "Login",
                                XamlRoot = _navigationService.Frame.XamlRoot,
                            };
                            ContentDialogResult result = await dialog.ShowAsync();

                            if (result == ContentDialogResult.Primary)
                            {
                                _navigationService.NavigateTo(typeof(LoginViewModel).FullName);
                            }
                        }
                        else
                        {
                            ContentDialog dialog = new()
                            {
                                Title = "Error!",
                                Content = "Something went wrong, please try again.",
                                PrimaryButtonText = "Ok",
                                XamlRoot = _navigationService.Frame.XamlRoot,
                            };
                            ContentDialogResult result = await dialog.ShowAsync();
                        }

                    }
                    );
                }
                return _createUserCommand;
            }

        }

        public void OnNavigatedFrom()
        {

        }

        public void OnNavigatedTo(object parameter)
        {

        }

        /// <summary>
        /// Custom validation to check username availability 
        /// </summary>
        private sealed class UsernameCheck : ValidationAttribute
        {
            private readonly string _errorMessage;

            public UsernameCheck(string propertyName, string errorMessage)
            {
                PropertyName = propertyName;
                _errorMessage = errorMessage;
            }

            public string PropertyName { get; }

            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                List<string> memberName = new() { PropertyName };
                string username = (string)value;
                UserDto userCheck = _users.FirstOrDefault(x => x.Username.ToLower() == username.ToLower());

                if (userCheck == null)
                {
                    return ValidationResult.Success;
                }

                return new ValidationResult(_errorMessage, memberName);
            }
        }

        /// <summary>
        /// Custom validation class to make sure password1 and password2 are equal
        /// </summary>
        private sealed class PasswordCheck : ValidationAttribute
        {
            private readonly string _errorMessage;

            public PasswordCheck(string propertyName, string errorMessage)
            {
                PropertyName = propertyName;
                _errorMessage = errorMessage;
            }

            public string PropertyName { get; }

            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                object instance = validationContext.ObjectInstance;
                object otherValue = instance.GetType().GetProperty(PropertyName).GetValue(instance);

                List<string> memberName = new() { "Password2" };

                return (value as string).Equals(otherValue) ? ValidationResult.Success : new ValidationResult(_errorMessage, memberName);
            }
        }
    }
}
