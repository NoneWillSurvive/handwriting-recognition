/*
Handwriting Recognition with WPF and Microsoft.Ink

Original publication:
https://www.codeproject.com/Articles/5282936/Handwriting-Recognition-WPF-and-Microsoft-Ink

    Copyright © 2020 by Sergey A Kryukov
    http://www.codeproject.com/Members/SAKryukov
    http://www.SAKryukov.org
*/

// wrong idea, InkCollector is not actually used:
// https://docs.microsoft.com/en-us/dotnet/framework/wpf/advanced/handwriting-recognition

namespace Handwriting.Ui {
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Controls;
    using System.Windows.Ink;

    public partial class MainWindow : Window {

        public MainWindow() {
            InitializeComponent();
            this.Icon = Main.TheApplication.Current.ApplicationIcon;
            if (SetupRecognizerSet() < 1)
                return;
            inkCanvas.DefaultDrawingAttributes.Width = 1;
            inkCanvas.DefaultDrawingAttributes.Height = 1;
            inkCanvas.EditingMode = InkCanvasEditingMode.Ink;
            inkCanvas.StrokeErased += (object innerSender, RoutedEventArgs eventArgs) => { if (inkCanvas.Strokes.Count < 1) EnableInkButtons(false); };
            inkCanvas.StrokeCollected += (object innerSender, InkCanvasStrokeCollectedEventArgs eventArgs) => { EnableInkButtons(true); };
            var emptyStrokeCollection = new StrokeCollection();
            this.KeyDown += (object sender, KeyEventArgs eventArgs) => {
                if (inkCanvas.Strokes.Count > 0 && eventArgs.Key == Key.Z && inkCanvas.EditingMode == InkCanvasEditingMode.EraseByStroke) {
                    inkCanvas.Strokes.Replace(inkCanvas.Strokes[inkCanvas.Strokes.Count - 1], emptyStrokeCollection);
                    return;
                } //if undo
                bool eraseMode = eventArgs.Key == Key.LeftCtrl || eventArgs.Key == Key.RightCtrl;
                bool selectMode = eventArgs.Key == Key.LeftShift || eventArgs.Key == Key.RightShift;
                if (!(eraseMode || selectMode)) return;
                else if (eraseMode)
                    this.radioEraserMode.IsChecked = true;
                else if (selectMode)
                    this.radioSelectorMode.IsChecked = true;
            }; //KeyDown
            this.KeyUp += (object sender, KeyEventArgs eventArgs) => {
                if (!(eventArgs.Key == Key.LeftCtrl || eventArgs.Key == Key.RightCtrl || eventArgs.Key == Key.LeftShift || eventArgs.Key == Key.RightShift)) return;
                this.radioInkMode.IsChecked = true;
            }; //KeyUp
            this.radioInkMode.Checked += (object sender, RoutedEventArgs eventArgs) => {
                inkCanvas.EditingMode = InkCanvasEditingMode.Ink;
                ShowStatus(this.radioInkMode.ToolTip.ToString());
            }; //radioInkMode.Checked
            this.radioInkMode.IsChecked = true;
            this.radioEraserMode.Checked += (object sender, RoutedEventArgs eventArgs) => {
                inkCanvas.EditingMode = InkCanvasEditingMode.EraseByStroke;
                ShowStatus(this.radioEraserMode.ToolTip.ToString());
            }; //radioEraserMode.Checked
            this.radioSelectorMode.Checked += (object sender, RoutedEventArgs eventArgs) => {
                inkCanvas.EditingMode = InkCanvasEditingMode.Select;
                ShowStatus(this.radioSelectorMode.ToolTip.ToString());
            }; //radioSelectorMode.Checked
            this.buttonRecognize.Click += (object sender, RoutedEventArgs eventArgs) => {
                var selector = new RecognitionResultSelectorWindow();
                Main.IRecognitionResultSelector selectorToUse = null;
                var strokes = inkCanvas.Strokes;
                if (inkCanvas.EditingMode == InkCanvasEditingMode.Select) {
                    var selectedStrokes = inkCanvas.GetSelectedStrokes();
                    if (selectedStrokes.Count > 0)
                        strokes = selectedStrokes;
                } //if
                var text = Main.TextRecognizer.Recognize(strokes, (Microsoft.Ink.Recognizer)(this.listLanguages.SelectedItem), selectorToUse);
                if (text == null) return;
                if (text.Length > 0 && this.checkBoxAppend.IsChecked == true)
                    text = Main.DefinitionSet.WordSeparator + text;
                else
                    this.textBoxTarget.Clear();
                AppendText(text);
            }; //buttonRecognize.Click
            this.buttonClearInk.Click += (object sender, RoutedEventArgs eventArgs) => {
                inkCanvas.Strokes.Clear();
                EnableInkButtons(false);
            }; //buttonClearInk.Click
            this.buttonCopy.Click += (object sender, RoutedEventArgs eventArgs) => {
                var text = this.textBoxTarget.Text;
                if (text.Trim().Length < 1) return;
                Clipboard.SetText(text);
            }; //buttonCopy.Click
        } //MainWindow

        private void ShowStatus(string status) {
            textBlockStatus.Text = status;
        } //ShowStatus

        private void EnableInkButtons(bool doEnable) {
            this.buttonClearInk.IsEnabled = doEnable;
            this.buttonRecognize.IsEnabled = doEnable;
        } //EnableInkButtons

        private void TextBoxSelectEnd() {
            textBoxTarget.Select(textBoxTarget.Text.Length, 0);
        } //TextBoxSelectEnd

        private void AppendText(string text) {
            TextBoxSelectEnd();
            textBoxTarget.SelectedText = text;
            TextBoxSelectEnd();
        } //AppendText

    } //class MainWindow

} //namespace SAHand.Ui
