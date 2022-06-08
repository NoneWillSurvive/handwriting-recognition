namespace Handwriting.Ui {
    using System.Windows;
    using System.Windows.Controls;
    using Microsoft.Ink;

    public partial class RecognitionResultSelectorWindow : Window, Main.IRecognitionResultSelector {
        
        public RecognitionResultSelectorWindow() {
            InitializeComponent();
            this.Icon = Main.TheApplication.Current.ApplicationIcon;
            buttonOk.Click += (object sender, RoutedEventArgs e) => {
                DialogResult = true;
            };
        } //RecognitionResultSelectorWindow

        string Main.IRecognitionResultSelector.Select(Recognizer recognizer, RecognitionResult result) {
            panel.Children.Clear();
            string topResult = result.TopString;
            var confidenceLevelPropertySupported = IsConfidenceLevelPropertySupported(recognizer);
            // with some eingines, throws "The specified property identifier was invalid" exception, so confidenceLevelPropertySupported is tested
            if (confidenceLevelPropertySupported && result.TopConfidence == RecognitionConfidence.Strong) return topResult;
            var words = topResult.Split(Main.DefinitionSet.WordSeparator);
            int currentPosition = 0;
            int[] positions = new int[words.Length];
            ComboBox selectedCombo = null;
            for (var index = 0; index < words.Length; ++index) {
                positions[index] = currentPosition;
                currentPosition += words[index].Length + 1;
                var alts = result.GetAlternatesFromSelection(positions[index], words[index].Length);
                bool considerAlternates = true;
                if (confidenceLevelPropertySupported && alts[0].Confidence == RecognitionConfidence.Strong)
                    considerAlternates = false;
                if (considerAlternates && alts.Count > 1) {
                    var cb = new ComboBox();
                    if (selectedCombo == null) selectedCombo = cb;
                    cb.VerticalAlignment = VerticalAlignment.Center;
                    cb.Margin = comboBoxSample.Margin;
                    foreach (var alt in alts)
                        cb.Items.Add(alt.ToString());
                    panel.Children.Add(cb);
                    cb.SelectedIndex = 0;
                } else {
                    var tb = new TextBlock();
                    tb.Text = words[index];
                    tb.VerticalAlignment = VerticalAlignment.Center;
                    tb.Margin = comboBoxSample.Margin;
                    panel.Children.Add(tb);
                } //if
            } //loop words
            if (selectedCombo != null) selectedCombo.Focus();
            if (ShowDialog() != true) return null;
            string textResult = string.Empty;
            foreach (var child in panel.Children) {
                var tb = child as TextBlock;
                var cb = child as ComboBox;
                if (tb != null)
                    textResult += tb.Text + Main.DefinitionSet.WordSeparator;
                else if (cb != null)
                    textResult += cb.SelectedItem.ToString() + Main.DefinitionSet.WordSeparator;
            } //loop
            return textResult.Trim();
        } //Select

        private static bool IsConfidenceLevelPropertySupported(Recognizer recognizer) {
            bool result = false;
            foreach (var guid in recognizer.SupportedProperties)
                if (guid == RecognitionProperty.ConfidenceLevel) {
                    result = true;
                    break;
                } //if
            return result;
        } //IsConfidenceLevelPropertySupported

    } //class RecognitionResultSelectorWindow

} //namespace SAHand.Ui
