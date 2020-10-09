using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace FinPos.Client.Controls
{
    public class ShadowedTextBox : TextBox
    {
        #region Properties
        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Label.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register("Label", typeof(string), typeof(ShadowedTextBox), new UIPropertyMetadata("Label"));

        public Style LabelStyle
        {
            get { return (Style)GetValue(LabelStyleProperty); }
            set { SetValue(LabelStyleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LabelStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LabelStyleProperty =
            DependencyProperty.Register("LabelStyle", typeof(Style), typeof(ShadowedTextBox), new UIPropertyMetadata(null));


        public bool HasText
        {
            get { return (bool)GetValue(HasTextProperty); }
            private set { SetValue(HasTextPropertyKey, value); }
        }

        private static readonly DependencyPropertyKey HasTextPropertyKey =
            DependencyProperty.RegisterReadOnly("HasText", typeof(bool), typeof(ShadowedTextBox), new PropertyMetadata(false));
        public static readonly DependencyProperty HasTextProperty = HasTextPropertyKey.DependencyProperty;

        #endregion
        public ShadowedTextBox()
           : base()
        {
        }

        AdornerLayer myAdornerLayer;
        AdornerLabel myAdornerLabel;
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            myAdornerLayer = AdornerLayer.GetAdornerLayer(this);
            myAdornerLabel = new AdornerLabel(this, this.Label, this.LabelStyle);
            UpdateAdorner(this);

            DependencyPropertyDescriptor focusProp = DependencyPropertyDescriptor.FromProperty(FrameworkElement.IsFocusedProperty, typeof(FrameworkElement));
            if (focusProp != null)
            {
                focusProp.AddValueChanged(this, delegate
                {
                    UpdateAdorner(this);
                });
            }

            DependencyPropertyDescriptor containsTextProp = DependencyPropertyDescriptor.FromProperty(ShadowedTextBox.HasTextProperty, typeof(ShadowedTextBox));
            if (containsTextProp != null)
            {
                containsTextProp.AddValueChanged(this, delegate
                {
                    UpdateAdorner(this);
                });
            }
        }
        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            HasText = this.Text != "";

            base.OnTextChanged(e);
        }
        protected override void OnDragEnter(DragEventArgs e)
        {
            myAdornerLayer.RemoveAdorners<AdornerLabel>(this); // requires AdornerExtensions.cs

            base.OnDragEnter(e);
        }
        protected override void OnDragLeave(DragEventArgs e)
        {
            UpdateAdorner(this);

            base.OnDragLeave(e);
        }
        private void UpdateAdorner(FrameworkElement elem)
        {
            if (((ShadowedTextBox)elem).HasText || elem.IsFocused)
            {
                // Hide the Shadowed Label
                this.ToolTip = this.Label;
                myAdornerLayer.RemoveAdorners<AdornerLabel>(elem);  // requires AdornerExtensions.cs
            }
            else
            {
                // Show the Shadowed Label
                this.ToolTip = null;
                if (!myAdornerLayer.Contains<AdornerLabel>(elem))  // requires AdornerExtensions.cs
                    myAdornerLayer.Add(myAdornerLabel);
            }
        }
    }
}
