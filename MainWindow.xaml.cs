using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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
using CSharpClipboard.Annotations;

namespace CSharpClipboard
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        public SourceTargetViewModel Model { get; set; } = new SourceTargetViewModel()
        {
            Footer = "Welcome",
            Source = "Paste Here...(from excel, etc)",
            Target = "Result will be written here",
            Verb = "FormatDict",  
            IncludeHeaders = true
        };

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = Model;
        }

        private void PasteAndTransform_OnClick(object sender, RoutedEventArgs e)
        {
            Model.Source = Clipboard.GetText(TextDataFormat.Text);
            Transform_OnClick(sender, e);
        }

        private void Transform_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Model.Target = Model.Provider.Transform(Model);
            }
            catch (Exception exception)
            {
                Model.Target = exception.ToString();
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


