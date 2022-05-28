/*
Handwriting Recognition with WPF and Microsoft.Ink

Original publication:
https://www.codeproject.com/Articles/5282936/Handwriting-Recognition-WPF-and-Microsoft-Ink

    Copyright © 2020 by Sergey A Kryukov
    http://www.codeproject.com/Members/SAKryukov
    http://www.SAKryukov.org
*/

namespace Handwriting.Main {

    partial class TheApplication {
        internal System.Windows.Media.ImageSource ApplicationIcon { get; private set; }
        private static void LoadApplicationIcon(TheApplication current, System.IO.MemoryStream iconStream) {
            Handwriting.Resources.Resources.IconMain.Save(iconStream);
            if (iconStream.Length < 1) return;
            iconStream.Seek(0, System.IO.SeekOrigin.Begin);
            current.ApplicationIcon = System.Windows.Media.Imaging.BitmapFrame.Create(iconStream);
        } //LoadApplicationIcon
    } //class TheApplication

} //namespace SAHand.Main
