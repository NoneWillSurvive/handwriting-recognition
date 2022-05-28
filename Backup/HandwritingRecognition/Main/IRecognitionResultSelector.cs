/*
Handwriting Recognition with WPF and Microsoft.Ink

Original publication:
https://www.codeproject.com/Articles/5282936/Handwriting-Recognition-WPF-and-Microsoft-Ink

    Copyright © 2020 by Sergey A Kryukov
    http://www.codeproject.com/Members/SAKryukov
    http://www.SAKryukov.org
*/

namespace Handwriting.Main {
    using Microsoft.Ink;
    
    interface IRecognitionResultSelector {
        string Select(Recognizer recognizer, RecognitionResult result);
    } //interface IRecognitionResultSelector

} //namespace SAHand.Main
