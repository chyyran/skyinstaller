using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace TrailsHelper.Views
{
    public partial class GameDisplay : UserControl
    {
        public GameDisplay()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
