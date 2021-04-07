using App.Models;
using App.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace App.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public IDataStore<User> DataStore => DependencyService.Get<IDataStore<User>>();

        private string _title = string.Empty;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        protected void SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value)) return;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            changed?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
