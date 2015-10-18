using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Project
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// 
    public sealed partial class Difficulty : Page
    {

        private MainPage parent;
        public Difficulty(MainPage parent)
        {
            this.InitializeComponent();
            this.parent = parent;
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            parent.Children.Add(parent.mainMenu);
            parent.Children.Remove(this);
        }

        // TASK 3: Function for setting difficulty
        private void changeDifficultySize(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
           // if (parent.game != null) { parent.game.difficulty = (float)e.NewValue; }
        }
        private void changeDifficultyGhost(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            // if (parent.game != null) { parent.game.difficulty = (float)e.NewValue; }
        }

        private void changeDifficultyLives(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            // if (parent.game != null) { parent.game.difficulty = (float)e.NewValue; }
        }

    }
}
