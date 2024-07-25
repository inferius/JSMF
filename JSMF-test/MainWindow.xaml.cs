using System.Windows;
using JSMF.Parser;
using JSMF.Parser.Tokenizer;

namespace JSMF
{
    /// <summary>
    /// Interakční logika pro MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var tokens = TokenRegistredWords.ReadString(new InputStream("`ahoj ${ah} $fsa \\${pp}`"));
            var d = "z";
        }
    }
}
