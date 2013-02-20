using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using Alba.Framework.Linq;
using Alba.Framework.Sys;
using DpChangedEventArgs = System.Windows.DependencyPropertyChangedEventArgs;

namespace Alba.Framework.Controls
{
    public partial class ProgressSpinner
    {
        public static readonly DependencyProperty IsLoadingProperty =
            DependencyProperty.Register(Props.GetName((ProgressSpinner o) => o.IsLoading),
                typeof(bool), typeof(ProgressSpinner), new PropertyMetadata(false, IsLoading_Changed));
        public static readonly DependencyProperty IsDisplayingProperty =
            DependencyProperty.Register(Props.GetName((ProgressSpinner o) => o.IsDisplaying),
                typeof(bool), typeof(ProgressSpinner), new PropertyMetadata(true, IsDisplaying_Changed));

        public ProgressSpinner ()
        {
            InitializeComponent();
            AutoHide();
            Loaded += OnLoaded;
        }

        private void OnLoaded (object sender, RoutedEventArgs routedEventArgs)
        {
            ((Style)Resources["stygrdRoot"]).Triggers[0].EnterActions.Add(
                new BeginStoryboard { Storyboard = (Storyboard)Resources["animShow"] });
        }

        public bool IsLoading
        {
            get { return (bool)GetValue(IsLoadingProperty); }
            set { SetValue(IsLoadingProperty, value); }
        }

        public bool IsDisplaying
        {
            get { return (bool)GetValue(IsDisplayingProperty); }
            set { SetValue(IsDisplayingProperty, value); }
        }

        private static void IsDisplaying_Changed (DependencyObject d, DpChangedEventArgs e)
        {
            var @this = (ProgressSpinner)d;
            @this.AutoHide();
        }

        private static void IsLoading_Changed (DependencyObject d, DpChangedEventArgs e)
        {
            var @this = (ProgressSpinner)d;
            var isLoading = (bool)e.NewValue;
            if (isLoading)
                @this.IsDisplaying = true;
        }

        private void AutoHide ()
        {
            Dispatcher.QueueExecute(DispatcherPriority.ApplicationIdle, () => { IsDisplaying = false; });
        }
    }
}