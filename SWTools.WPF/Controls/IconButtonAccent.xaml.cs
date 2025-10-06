using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace SWTools.WPF.Controls {
    /// <summary>
    /// IconButtonAccent.xaml 的交互逻辑
    /// </summary>
    public partial class IconButtonAccent : UserControl {
        // Text 属性
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(IconButtonAccent));
        public string Text {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        // Icon 属性
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(string), typeof(IconButtonAccent));
        public string Icon {
            get { return (string)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        // Click 事件
        public static readonly RoutedEvent ClickEvent =
            ButtonBase.ClickEvent.AddOwner(typeof(IconButtonAccent));
        
        public event RoutedEventHandler Click {
            add { AddHandler(ClickEvent, value); }
            remove { RemoveHandler(ClickEvent, value); }
        }

        public IconButtonAccent() {
            InitializeComponent();
        }
    }
}