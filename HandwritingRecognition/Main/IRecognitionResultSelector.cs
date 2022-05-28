namespace Handwriting.Main {
    using Microsoft.Ink;
    
    interface IRecognitionResultSelector {
        string Select(Recognizer recognizer, RecognitionResult result);
    } //interface IRecognitionResultSelector

} //namespace SAHand.Main
