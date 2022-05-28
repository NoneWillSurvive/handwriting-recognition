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
