using System;
using System.Collections.Generic;
using System.Text;

namespace SoundTransitTwitterUpdates.Requests
{
    public class AlexaRequestFormatter
    {
        private int indexIntoOriginal;

        private string originalText;

        private StringBuilder stringBuilder;

        private Dictionary<char, List<Scanner>> scanners;

        private delegate bool Scanner(int currentIndex);

        public AlexaRequestFormatter(string textToFormat)
        {
            originalText = textToFormat;
            indexIntoOriginal = 0;
            stringBuilder = new StringBuilder();
            AddScanner("Hh", ScanForLinks);
            AddScanner("0123456789", ScanForNumbers);
            AddScanner("Ss", ScanForSea);
            AddScanner("Dd", ScanForDep);
            AddScanner("S", ScanForST);
        }

        private void AddScanner(string characterSet, Scanner scanner)
        {
            if (scanners == null)
                scanners = new Dictionary<char, List<Scanner>>();

            foreach (var character in characterSet)
            {
                if (scanners.ContainsKey(character))
                {
                    scanners[character].Add(scanner);
                }
                else
                {
                    scanners.Add(character, new List<Scanner>()
                    {
                       scanner
                    });
                }
            }
        }

        public string Format()
        {
            for (int i = indexIntoOriginal; i < originalText.Length; i++)
            {
                if (scanners.ContainsKey(originalText[i]))
                {
                    foreach (var scanner in scanners[originalText[i]])
                    {
                        if (scanner(i))
                        {
                            i = indexIntoOriginal;
                            i--;
                            break;
                        }
                    }
                }
            }

            if (stringBuilder.Length == 0)
                return originalText;
            else
                return stringBuilder.Append(originalText.Substring(indexIntoOriginal)).ToString();
        }

        private bool ScanForLinks(int currentIndex)
        {
            if (currentIndex + 3 >= originalText.Length)
                return false;

            if (originalText[currentIndex] != 'h' && originalText[currentIndex] != 'H')
                return false;

            if (originalText[currentIndex + 1] != 't' && originalText[currentIndex + 1] != 'T')
                return false;

            if (originalText[currentIndex + 2] != 't' && originalText[currentIndex + 2] != 'T')
                return false;

            if (originalText[currentIndex + 3] != 'p' && originalText[currentIndex + 3] != 'P')
                return false;

            var subIndex = currentIndex + 4;

            if (originalText[subIndex] == 's' || originalText[subIndex] == 'S')
                subIndex++;

            if (originalText[subIndex] != ':')
                return false;

            if (originalText[subIndex + 1] != '/')
                return false;

            if (originalText[subIndex + 2] != '/')
                return false;

            subIndex += 3;

            stringBuilder.Append(originalText.Substring(indexIntoOriginal, currentIndex - indexIntoOriginal));

            while (subIndex < originalText.Length && originalText[subIndex] != ' ' && originalText[subIndex] != '\n' && originalText[subIndex] != ',')
                subIndex++;

            indexIntoOriginal = subIndex;

            return true;
        }

        private bool ScanForSea(int currentIndex)
        {
            if (currentIndex + 2 >= originalText.Length)
                return false;

            if (originalText[currentIndex] != 's' && originalText[currentIndex] != 'S')
                return false;

            if (originalText[currentIndex + 1] != 'e' && originalText[currentIndex + 1] != 'e')
                return false;

            if (originalText[currentIndex + 2] != 'a' && originalText[currentIndex + 2] != 'a')
                return false;

            if (currentIndex + 3 < originalText.Length && originalText[currentIndex + 3] != ' ' && originalText[currentIndex + 3] != ',' && originalText[currentIndex + 3] != '.')
                return false;

            stringBuilder.Append(originalText.Substring(indexIntoOriginal, currentIndex - indexIntoOriginal));
            stringBuilder.Append("Seattle");

            indexIntoOriginal = currentIndex + 3;

            return true;
        }

        private bool ScanForDep(int currentIndex)
        {
            if (currentIndex + 3 >= originalText.Length)
                return false;

            if (originalText[currentIndex] != 'd' && originalText[currentIndex] != 'D')
                return false;

            if (originalText[currentIndex + 1] != 'e' && originalText[currentIndex + 1] != 'E')
                return false;

            if (originalText[currentIndex + 2] != 'p' && originalText[currentIndex + 2] != 'P')
                return false;

            var subIndex = currentIndex + 3;

            if (originalText[subIndex] == '.')
                subIndex++;

            if (originalText[subIndex] != ')')
                return false;

            stringBuilder.Append(originalText.Substring(indexIntoOriginal, currentIndex - indexIntoOriginal));
            stringBuilder.Append("departure)");

            indexIntoOriginal = subIndex + 1;

            return true;
        }

        private bool ScanForST(int currentIndex)
        {
            if (currentIndex + 2 >= originalText.Length)
                return false;

            if (originalText[currentIndex] != 'S')
                return false;

            if (originalText[currentIndex + 1] != 'T')
                return false;

            if (originalText[currentIndex + 2] != ' ')
                return false;

            stringBuilder.Append(originalText.Substring(indexIntoOriginal, currentIndex - indexIntoOriginal));
            stringBuilder.Append("Sound Transit ");

            indexIntoOriginal = currentIndex + 3;

            return true;
        }

        private bool ScanForNumbers(int currentIndex)
        {
            var numericIndex = currentIndex;

            while (numericIndex < originalText.Length)
            {
                if (IsNumeric(originalText[numericIndex]))
                    numericIndex++;
                else
                    break;
            }

            if (numericIndex - currentIndex <= 2)
                return false;

            stringBuilder.Append(originalText.Substring(indexIntoOriginal, currentIndex - indexIntoOriginal));

            if ((numericIndex - currentIndex) % 2 != 0)
            {
                stringBuilder.Append(originalText[currentIndex]).Append(" ");
                currentIndex++;
            }

            while (currentIndex < numericIndex)
            {
                stringBuilder.Append(originalText[currentIndex]).Append(originalText[currentIndex + 1]).Append(" ");
                currentIndex += 2;
            }

            indexIntoOriginal = numericIndex;

            return true;

            bool IsNumeric(char character) => character >= 48 && character <= 57;
        }
    }
}
