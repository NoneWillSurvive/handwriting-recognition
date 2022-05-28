namespace Handwriting.Main {
    using StrokeCollection = System.Windows.Ink.StrokeCollection;
    using MemoryStream = System.IO.MemoryStream;
    using ManualResetEvent = System.Threading.ManualResetEvent;
    using Microsoft.Ink;

    static class TextRecognizer {

        internal static string Recognize(StrokeCollection strokes, Recognizer recognizer, Main.IRecognitionResultSelector selector) {
            using (var ink = new Ink()) {
                PopulateInk(ink, strokes);
                if (recognizer == null)
                    return ink.Strokes.ToString(); //default recognizer and language
                using (var context = recognizer.CreateRecognizerContext()) {
                    if (ink.Strokes.Count < 1) return null;
                    context.Strokes = ink.Strokes;
                    if (selector == null) {
                        RecognitionStatus status;
                        var result = context.Recognize(out status);
                        if (status == RecognitionStatus.NoError)
                            return result.TopString;
                        else
                            return null;
                    } else
                        return RecognizeWithAlternates(context, selector);
               } //using context
            } //using ink
        } //Recognize

        private static string RecognizeWithAlternates(RecognizerContext context, Main.IRecognitionResultSelector selector) {
            RecognitionResult result = null;
            ManualResetEvent completionEvent = new ManualResetEvent(false);
            context.RecognitionWithAlternates += (object sender, RecognizerContextRecognitionWithAlternatesEventArgs e) => {
                var eventid = System.Threading.Thread.CurrentThread.ManagedThreadId;
                result = e.Result;
                completionEvent.Set();
            }; //context.RecognitionWithAlternates
            context.BackgroundRecognizeWithAlternates();
            completionEvent.WaitOne();
            return selector.Select(context.Recognizer, result);
        } //RecognizeWithAlternates

        private static void PopulateInk(Ink ink, StrokeCollection strokes) {
            using (var ms = new MemoryStream()) {
                strokes.Save(ms);
                ink.Load(ms.ToArray());
            } // using
        } //PopulateInk

    } //class TextRecognizer

} //namespace SAHand.Main
