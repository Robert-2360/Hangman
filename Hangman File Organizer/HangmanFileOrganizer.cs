// HangmanFileOrganizer.cs
// Takes a file of words from an outside source and organizes it 
// into a new file that can be used in the Hangman Game.

using System;
using System.IO;

class HangmanFileOrganizer
{
   static void Main(string[] args)
   {
      /* The section will:
       * Iterate through the file in a single pass
       * Count the number of words in the give file
       * Determine the largest word length
       * This information will be needed for the next section */

      // initialize variables
      string inputFileName = @"D:\OneThousandCommonWords.txt";
      int totalWords = 0;
      int longestWordLength = 0;

      // Open file for reading
      FileStream input = new FileStream(inputFileName, FileMode.Open, FileAccess.Read);
      StreamReader fileReader = new StreamReader(input);

      // Iterate through the file
      string word = fileReader.ReadLine();
      while (word != null)
      {
         // Count the total number of words
         totalWords++;

         // Determine longest word length
         if (word.Length > longestWordLength)
         {
            longestWordLength = word.Length;
         }

         // Read next word in the file
         word = fileReader.ReadLine();
      }

      //display results
      Console.WriteLine("Total number of words in the original file is: {0}", totalWords);
      Console.WriteLine("The maximum word length is: {0}", longestWordLength);
      Console.WriteLine();

      // Close file
      fileReader.Close();



      /* This section will:
       * Iterate through the file again
       * Verify the number of words in the file
       * Determine the number of non-compliant words
       * Non-compliant means: having a letter or symbol other than a lower case letter
       * Examples of non-compliant: Florida, don't, sam23, to-date, john@aol.com, goHigh
       * Display a report on:
       *    Total number of words for each length
       *    Total number of non-compliant words
       *    The sum these words for verification */

      // initialize variables
      bool isAllLowerCase = true;
      int numberNonCompliant = 0;
      int verifyCount = 0;
      int[] wordLengths = new int[longestWordLength + 1];

      // Open file for reading again
      input = new FileStream(inputFileName, FileMode.Open, FileAccess.Read);
      fileReader = new StreamReader(input);

      // Iterate through the file again
      word = fileReader.ReadLine();
      while (word != null)
      {
         // Reset flag
         isAllLowerCase = true;

         // Count the number of non-compliant words and flag its existence
         foreach (char letter in word)
         {
            if (!Char.IsLower(letter))
            {
               numberNonCompliant++;
               isAllLowerCase = false;
               break;
            }
         }

         // Count the number of compliant words for each length
         if (isAllLowerCase)
         {
            wordLengths[word.Length]++;
         }

         // Read next word in the file
         word = fileReader.ReadLine();
      }

      // Display results and sum each word length for verification
      for (int i = 0; i <= longestWordLength; i++)
      {
         double percentage = 100.0 * wordLengths[i] / totalWords;
         Console.WriteLine("Total words of length-{0, -2} is: {1, 3} or {2, 4:F1}%",
            i, wordLengths[i], percentage);
         verifyCount += wordLengths[i];
      }
      Console.WriteLine();
      Console.WriteLine("Total number of non-compliant words is: {0, -2} or {1:F1}%",
         numberNonCompliant, 100.0 * numberNonCompliant / totalWords);
      Console.WriteLine();
      Console.WriteLine("Verify total number of words in the original file: {0}",
         verifyCount + numberNonCompliant);
      Console.WriteLine();

      // Close file
      fileReader.Close();




      /* This section will:
       * Establish a minimum and maximum word length
       * Load all compliant words into a string array
       * Sort the array (So that it is easier to locate a word if necessary later)
       * Write the array into a text file (This file will be the one used for the game)
       */

      // initialize variables
      int minimumWordLength = 4;
      int maximumWordLength = 9;
      int wordArraySize = 0;
      int index = 0;
      string outputFileName = @"D:\HangmanWords.txt";

      // calculate size of array
      for (int i = minimumWordLength; i <= maximumWordLength; i++)
      {
         wordArraySize += wordLengths[i];
      }

      // initialize array
      string[] hangmanWords = new string[wordArraySize];

      // Open file for reading again
      input = new FileStream(inputFileName, FileMode.Open, FileAccess.Read);
      fileReader = new StreamReader(input);

      // Iterate through the file again
      word = fileReader.ReadLine();
      while (word != null)
      {
         // Reset flag
         isAllLowerCase = true;

         // Flag non-compliant words
         foreach (char letter in word)
         {
            if (!Char.IsLower(letter))
            {
               isAllLowerCase = false;
               break;
            }
         }

         // Add appropriate word into array
         if (word.Length >= minimumWordLength && word.Length <= maximumWordLength && isAllLowerCase)
         {
            hangmanWords[index] = word;
            index++;
         }

         // Read next word in the file
         word = fileReader.ReadLine();
      }

      // Close file
      fileReader.Close();

      // Sort the array
      Array.Sort(hangmanWords);

      // Open file for writing
      FileStream output = new FileStream(outputFileName, FileMode.Create, FileAccess.Write);
      StreamWriter fileWriter = new StreamWriter(output);

      // Write array into a file and check for correctness if necessary
      foreach (string w in hangmanWords)
      {
         fileWriter.WriteLine(w);
         //Console.WriteLine("{0} {1}", w, w.Length);
      }

      // Close the file
      fileWriter.Close();

      Console.WriteLine("File {0} was just created,", outputFileName);
      Console.WriteLine(" with {0} words in its file.", hangmanWords.Length);
      Console.WriteLine();

      // Freeze console window
      Console.ReadLine();
   }
}