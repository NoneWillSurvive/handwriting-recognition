namespace Handwriting.Main {
    using CharUnicodeInfo = System.Globalization.CharUnicodeInfo;
    using UnicodeCategory = System.Globalization.UnicodeCategory;

    static class DefinitionSet {

        internal static char WordSeparator { get { return wordSeparator; } }
        
        static DefinitionSet() {
            for (ushort codePoint = 0; codePoint < ushort.MaxValue; codePoint++) {
                char separator = System.Convert.ToChar(codePoint);
                if (CharUnicodeInfo.GetUnicodeCategory(separator) == UnicodeCategory.SpaceSeparator) {
                    wordSeparator = separator;
                    break;
                } //if
            } //loop
        } //DefinitionSet constructor
        private static char wordSeparator;

    } //class DefinitionSet

} //namespace Handwriting.Main
