using System;
using CsvHelper;
using System.IO;
using System.Globalization;
using System.Linq;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using System.Threading.Tasks;

namespace CSharpMod
{
    class Program
    {
        // declare global variables
        static List<PlayerInformation> list = new List<PlayerInformation>();

        static List<String> maleList = new List<String>();
        static List<String> femaleList = new List<String>();
        static string tempOccurrence = "";
        static string tempPercentageMatch = "";
        static int percentageMatch = 0;

        static List<String> matchList = new List<String>();
        static List<int> percentageMatchList = new List<int>();

        static string finalOutput = "";

        // create Main method
        static void Main(string[] args)
        {
            // call receive input method
            // to read and validate input
            ReceiveInput();


            // call group by gender method
            // pass list with duplicates removed
            GroupByGender(removeDuplicates(list));

            // calculate percentage match for each male against each female
            for (int i = 0; i < maleList.Count; i++)
            {
                for (int j = 0; j < femaleList.Count; j++)
                {

                    var input = $"{maleList[i].ToLower()}matches{femaleList[j].ToLower()}";

                    // call calculate occurrence method
                    calculateOccurrence(input);

                    // call calculate Percentage method
                    calculatePercentage(tempOccurrence);

                    // call output final list method
                    outputFinalList(maleList[i], femaleList[j], percentageMatch);

                    // reset temp variables
                    tempOccurrence = "";
                    tempPercentageMatch = "";
                }
            }


            // sort results of final list
            sortResults(matchList, percentageMatchList);

            // output final list to file
            outputToFile(finalOutput);
        }

        // receive input method
        public static void ReceiveInput()
        {

            // use streamreader and Csv Helper to add information from CSV file
            // to a list of type Player Information
            using (var streamReader = new StreamReader(@"./people.csv"))
            {
                using (var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture))
                {
                    var records = csvReader.GetRecords<PlayerInformation>().ToList();
                    list = records;
                }
            }

            // validate name and gender of each entry
            for (int i = 0; i < list.Count; i++)
            {
                if (validateInput(list[i].Name.ToLower(), list[i].Gender) == false)
                {
                    return;
                }
            }
        }

        // validate input method
        public static bool validateInput(string name, string gender)
        {
            // validate
            // check if empty string
            if (name == "" || gender == "")
            {
                // alert user to error
                Console.WriteLine("Empty String Inputted");
                Console.WriteLine("Please Alter CSV file and try again.");

                // if user input is invalid return false
                return false;
            }

            // check if all characters in alphabet
            if (name.All(Char.IsLetter) == false || gender.All(Char.IsLetter) == false)
            {
                // alert user to error
                Console.WriteLine("Character Outside of Alphabet Used");
                Console.WriteLine("Please Alter CSV file and try again.");

                // if user input is invalid return false
                return false;
            }

            // if user input is valid return true
            return true;
        }

        // method to remove duplicates from list
        public static List<PlayerInformation> removeDuplicates(List<PlayerInformation> fullList)
        {
            // iterate through list
            for (int i = 0; i < fullList.Count; i++)
            {
                // iterate through list at one point further than previous iteration
                for (int j = i + 1; j < fullList.Count; j++)
                {
                    // if duplicate found
                    // remove duplicate
                    if (fullList[i].Name == fullList[j].Name && fullList[i].Gender == fullList[j].Gender)
                    {
                        fullList.Remove(fullList[j]);
                    }
                }
            }

            // return updated list
            return fullList;
        }

        // method to group players by gender
        public static void GroupByGender(List<PlayerInformation> fullList)
        {
            // iterate through list
            for (int i = 0; i < fullList.Count; i++)
            {

                // if gender is male add to male list
                if ((fullList[i].Gender) == "m")
                {
                    maleList.Add(fullList[i].Name);
                }
                // if gender is female add to female list
                else
                {
                    femaleList.Add(fullList[i].Name);
                }
            }
        }


        // calculate occurrence method
        public static void calculateOccurrence(string input)
        {
            // create counter variable
            var inputCounter = 0;

            // loop through input string
            for (int i = 0; i < 1; i++)
            {
                // create counter to keep track of character occurrence
                var occurrenceCounter = 1;

                // loop through input string 
                for (int j = i + 1; j < input.Length; j++)
                {
                    // check if there are more occurrences of the respective letter
                    if (input[i] == input[j])
                    {
                        // increase counter
                        occurrenceCounter++;

                        // remove character from string
                        input = input.Remove(j, 1);
                    }
                }

                // add occurrence to global tempOccurrence variable
                tempOccurrence += occurrenceCounter;

                // remove character from string
                input = input.Remove(i, 1);

                // increase input counter
                inputCounter++;
            }

            // call calculate occurrence method until input counter
            // is smaller than or equal to input length
            if (inputCounter <= input.Length)
            {
                calculateOccurrence(input);
            }
        }


        // calculate percentageMatch method
        public static void calculatePercentage(string occurrence)
        {
            // store length of occurrence string
            var length = occurrence.Length;

            // loop through string 
            for (int i = 0; i < length / 2; i++)
            {
                // store char at i
                char beginningChar = occurrence[i];

                // store char at length - i
                char endChar = occurrence[(length - (i + 1))];

                // change chars to doubles
                double beginningInt = Char.GetNumericValue(beginningChar);
                double endInt = Char.GetNumericValue(endChar);

                // add doubles together
                double addedNumbers = beginningInt + endInt;

                // add calculated number to global tempPercentageMatch variable
                tempPercentageMatch += addedNumbers;
            }

            //  if string is an odd number in length
            if (length % 2 == 1)
            {
                // add number at middle index to global tempPercentageMatch variable
                tempPercentageMatch += occurrence[length / 2];
            }

            // set occurrence to number stored in global tempPercentageMatch variable
            occurrence = tempPercentageMatch;

            // if number stored in global tempPercentageMatch variable is 2 digits 
            if (tempPercentageMatch.Length < 3)
            {
                // set global percentageMatch variable to equal global tempPercentageMatch variable
                percentageMatch = int.Parse(tempPercentageMatch);
                return;
            }

            // if number stored in global tempPercentageMatch variable 
            // is 2 greater than 2 digits 
            // reset global tempPercentageMatch variable 
            // call calculate percentageMatch method again
            tempPercentageMatch = "";
            calculatePercentage(occurrence);
        }

        // method to sort results
        public static void sortResults(List<string> matches, List<int> percentages)
        {
            // create new array of type match information
            MatchInformation[] matchInformation = new MatchInformation[matches.Count];

            // iterate through the array and update values storing reference 
            // to the match string and the percentage match
            for (int i = 0; i < matchInformation.Length; i++)
            {
                matchInformation[i] = new MatchInformation { MatchString = matches[i], MatchPercentage = percentages[i] };
            }

            // create IEnumerable which is equal to the sorted list
            IEnumerable<MatchInformation> query = matchInformation.OrderByDescending(match => match.MatchPercentage).ThenBy(match => match.MatchString);

            // add sorted list to final out put 
            foreach (MatchInformation match in query)
            {
                finalOutput += $"{match.MatchString}\n";
            }
        }

        // output final list method
        public static void outputFinalList(string firstName, string secondName, int match)
        {
            var output = "";

            // if percentageMatch match is greater than or equal to 80%
            //  add match string and append good match 
            if (match >= 80)
            {
                output = $"{firstName} matches {secondName} {match}%, good match";
                matchList.Add(output);
                percentageMatchList.Add(match);

            }
            else
            {
                // else add match string 
                output = $"{firstName} matches {secondName} {match}%";
                matchList.Add(output);
                percentageMatchList.Add(match);
            }


        }

        // asynchronous output final string method
        public static async Task outputToFile(string output)
        {
            // write output to output file
            await File.WriteAllTextAsync("output.txt", output);
        }
    }

    // create PlayerInformation class to structure information from CSV file
    public class PlayerInformation
    {
        [Name("name")]
        public string Name { get; set; }
        [Name("gender")]
        public string Gender { get; set; }
    }

    // create Matchinformation class to sort final list
    public class MatchInformation
    {
        public string MatchString { get; set; }

        public int MatchPercentage { get; set; }
    }
}

