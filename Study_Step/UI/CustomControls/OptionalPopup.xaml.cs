using Study_Step.Models;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Study_Step.CustomControls
{
    public partial class OptionalPopup : UserControl
    {
        public static readonly DependencyProperty PlacementTargetProperty =
            DependencyProperty.Register(
                nameof(PlacementTarget),
                typeof(UIElement),
                typeof(OptionalPopup),
                new PropertyMetadata(null));

        public UIElement PlacementTarget
        {
            get => (UIElement)GetValue(PlacementTargetProperty);
            set => SetValue(PlacementTargetProperty, value);
        }

        public static readonly DependencyProperty PlacementProperty =
            DependencyProperty.Register(
                nameof(Placement),
                typeof(PlacementMode),
                typeof(OptionalPopup),
                new PropertyMetadata(PlacementMode.Bottom));

        public PlacementMode Placement
        {
            get => (PlacementMode)GetValue(PlacementProperty);
            set => SetValue(PlacementProperty, value);
        }

        public static readonly DependencyProperty ButtonsProperty =
            DependencyProperty.Register(
                nameof(Buttons),
                typeof(ObservableCollection<PopupButtonElement>),
                typeof(OptionalPopup),
                new FrameworkPropertyMetadata(
                    null,
                    FrameworkPropertyMetadataOptions.AffectsRender,
                    OnButtonsChanged));

        public static readonly DependencyProperty HorizontalOffsetProperty =
            DependencyProperty.Register(
                nameof(HorizontalOffset),
                typeof(double),
                typeof(OptionalPopup),
                new PropertyMetadata(0.0, OnOffsetChanged));

        public static readonly DependencyProperty VerticalOffsetProperty =
            DependencyProperty.Register(
                nameof(VerticalOffset),
                typeof(double),
                typeof(OptionalPopup),
                new PropertyMetadata(0.0, OnOffsetChanged));

        public static readonly DependencyProperty WidthProperty =
            DependencyProperty.Register(
                nameof(Width),
                typeof(int),
                typeof(OptionalPopup),
                new FrameworkPropertyMetadata(120));

        public double HorizontalOffset
        {
            get => (double)GetValue(HorizontalOffsetProperty);
            set => SetValue(HorizontalOffsetProperty, value);
        }

        public double VerticalOffset
        {
            get => (double)GetValue(VerticalOffsetProperty);
            set => SetValue(VerticalOffsetProperty, value);
        }
        public int Width
        {
            get => (int)GetValue(WidthProperty);
            set => SetValue(WidthProperty, value);
        }

        private static void OnOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (OptionalPopup)d;
            control.UpdatePopupPosition();
        }

        public ObservableCollection<PopupButtonElement> Buttons
        {
            get => (ObservableCollection<PopupButtonElement>)GetValue(ButtonsProperty);
            set => SetValue(ButtonsProperty, value);
        }

        private static void OnButtonsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (OptionalPopup)d;
            if (e.NewValue == null)
            {
                // Создаем новую коллекцию, если её нет
                control.Buttons = new ObservableCollection<PopupButtonElement>();
            }
        }

        public static readonly DependencyProperty IsOpenProperty =
        DependencyProperty.Register(
            nameof(IsOpen),
            typeof(bool),
            typeof(OptionalPopup),
            new FrameworkPropertyMetadata(
                false,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnIsOpenChanged));

        public bool IsOpen
        {
            get => (bool)GetValue(IsOpenProperty);
            set => SetValue(IsOpenProperty, value);
        }

        private static void OnIsOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (OptionalPopup)d;
            if (control._popup != null)
                control._popup.IsOpen = (bool)e.NewValue;
        }

        private void UpdatePopupPosition()
        {
            if (_popup != null)
            {
                _popup.HorizontalOffset = HorizontalOffset;
                _popup.VerticalOffset = VerticalOffset;

                // Трюк для форсирования обновления позиции
                _popup.HorizontalOffset += 0.1;
                _popup.HorizontalOffset -= 0.1;
            }
        }

        private Popup _popup;

        public OptionalPopup()
        {
            InitializeComponent();
            Buttons = new ObservableCollection<PopupButtonElement>();
            Loaded += (s, e) => InitializePopup();
        }

        private void InitializePopup()
        {
            _popup = Template.FindName("OptPopup", this) as Popup;
            if (_popup != null) {
                _popup.Closed += (sender, args) => {
                    IsOpen = false;
                    _popup.HorizontalOffset = HorizontalOffset;
                    _popup.VerticalOffset = VerticalOffset;
                };
            }
        }
    }
}
