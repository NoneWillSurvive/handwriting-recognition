/*
Handwriting Recognition with WPF and Microsoft.Ink

Original publication:
https://www.codeproject.com/Articles/5282936/Handwriting-Recognition-WPF-and-Microsoft-Ink

    Copyright © 2020 by Sergey A Kryukov
    http://www.codeproject.com/Members/SAKryukov
    http://www.SAKryukov.org
*/

// real help is here:
// https://docs.microsoft.com/en-us/previous-versions/dotnet/netframework-3.5/ms571346%28v%3dvs.90%29
// .NET 3.5:
// https://docs.microsoft.com/en-us/previous-versions/dotnet/netframework-3.5/w0x726c2%28v=vs.90%29 
// => 
//    Docs Previous Versions .NET .NET Framework 3.5 => General Reference for the .NET Framework => Additional Managed Reference Topics => Microsoft.Ink Name space =>...
// https://docs.microsoft.com/en-us/previous-versions/dotnet/netframework-3.5/ms581553%28v=vs.90%29
// Recognition topics:
// https://docs.microsoft.com/en-us/windows/win32/tablet/about-handwriting-recognition
//

// C++ help:
// https://docs.microsoft.com/en-us/windows/win32/tablet/tablet-pc-development-guide


//  Tablet PC Platform SDK 1.7 compatibility: https://blogs.msdn.microsoft.com/gavingear/2006/08/18/getting-started-with-tablet-pc-application-development-using-the-tablet-pc-platform-sdk-1-7/
// =>
// Supported Operating Systems:
// Windows XP Professional Edition
// Windows XP Tablet PC Edition 2005
// Windows 2000 Service Pack 4 (to 	Service Pack 4 with Update Rollup (5.0.2195) / September 13, 2005)
// Windows Server 2003 (April 24, 2003 to Service Pack 2 (5.2.3790) / March 13, 2007; 12 years ago)

// The following development environments/tools are reccomended for use with the Tablet PC Platform SDK 1.7:
// Visual Studio .NET 2003
// Visual Studio .NET 2005
// merged with Platform SDK starting with Vista
//



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
