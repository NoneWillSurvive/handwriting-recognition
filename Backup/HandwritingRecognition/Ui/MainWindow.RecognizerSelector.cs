/*
Handwriting Recognition with WPF and Microsoft.Ink

Original publication:
https://www.codeproject.com/Articles/5282936/Handwriting-Recognition-WPF-and-Microsoft-Ink

    Copyright © 2020 by Sergey A Kryukov
    http://www.codeproject.com/Members/SAKryukov
    http://www.SAKryukov.org
*/

namespace Handwriting.Ui {
    using Microsoft.Ink;

    public partial class MainWindow {

        private int SetupRecognizerSet() {
            var defaultRecognizer = recognizerSet.GetDefaultRecognizer();
            int defaultRecognizerIndex = 0;
            for (int index = 0; index < recognizerSet.Count; ++index) {
                var recognizer = recognizerSet[index];
                if (recognizer.Languages.Length > 0)
                    this.listLanguages.Items.Add(recognizer);
                if (recognizer.Name == defaultRecognizer.Name) // Recognizer class has identity problem, so only this ways
                    defaultRecognizerIndex = index;
            } //loop
            var count = listLanguages.Items.Count;
            if (count < 1) return count;
            listLanguages.SelectedIndex = defaultRecognizerIndex;
            listLanguages.Focus();
            return count;
        } //SetupRecognizerSet
        
        private Recognizers recognizerSet = new Microsoft.Ink.Recognizers();
    
    } //class MainWindow

} //namespace SAHand.Ui
