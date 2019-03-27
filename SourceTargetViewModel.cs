using System.ComponentModel;
using System.Runtime.CompilerServices;
using CSharpClipboard.Annotations;

namespace CSharpClipboard
{
    public class SourceTargetViewModel : INotifyPropertyChanged
    {
        private string source;
        private string target;
        private string footer;
        private string verb;
        private Provider provider;
        private bool includeHeaders;

        public ProviderCollection Providers { get; set; } = new ProviderCollection();

        public Provider Provider
        {
            get => provider;
            set
            {
                if (Equals(value, provider)) return;
                provider = value;
                OnPropertyChanged();

                Source = provider.Example;
                Target = provider.Help;
            }
        }

        public string Verb
        {
            get => verb;
            set
            {
                if (value == verb) return;
                verb = value;
                OnPropertyChanged();
            }
        }

        public string Source
        {
            get => source;
            set
            {
                if (value == source) return;
                source = value;
                OnPropertyChanged();
            }
        }

        public string Target
        {
            get => target;
            set
            {
                if (value == target) return;
                target = value;
                OnPropertyChanged();
            }
        }

        public string Footer
        {
            get => footer;
            set
            {
                if (value == footer) return;
                footer = value;
                OnPropertyChanged();
            }
        }

        public bool IncludeHeaders
        {
            get => includeHeaders;
            set
            {
                if (value == includeHeaders) return;
                includeHeaders = value;
                OnPropertyChanged();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}