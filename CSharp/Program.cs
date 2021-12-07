// declare global variables
var firstName = "";
var secondName = "";
string stringToAnalyse = "";
string tempOccurrence = "";
string tempPercentageMatch = "";
int percentageMatch = 0;

// call receive input method
ReceiveInput();

// call calculate occurrence method
calculateOccurrence(stringToAnalyse);

// call calculate percentage method
calculatePercentage(tempOccurrence);

// output final string to console
Console.WriteLine(outputFinalString(firstName, secondName, percentageMatch));

// receive input method
void ReceiveInput()
{
    // request user input
    Console.WriteLine("Please Enter First Name");
    firstName = Console.ReadLine();

    // request user input
    Console.WriteLine("Please Enter Second Name");
    secondName = Console.ReadLine();

    // create string to analyse
    var outputString = $"{firstName.ToLower()}matches{secondName.ToLower()}";

    // validateInput output
    if (validateInput(outputString) == true)
    {
        // if all validations have passed
        // store output in global stringToAnalyse variable
        stringToAnalyse = outputString;
    }
    return;
}

// validate input method
bool validateInput(string input)
{
    // validate
    // check if empty string
    if (input.StartsWith("matches") || input == "matches")
    {
        // alert user to error
        Console.WriteLine("Empty String Inputted");

        // request input again
        ReceiveInput();

        // if user input is invalid return true
        return false;
    }

    // check if all characters in alphabet
    if (input.All(Char.IsLetter) == false)
    {
        // alert user to error
        Console.WriteLine("Character Outside of Alphabet Used");

        // request input again
        ReceiveInput();

        // if user input is invalid return true
        return false;
    }

    // if user input is valid return true
    return true;
}


// calculate occurrence method
void calculateOccurrence(string input)
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
void calculatePercentage(string occurrence)
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


// output final string method
string outputFinalString(string firstName, string secondName, int match)
{
    // if percentageMatch match is greater than or equal to 80%
    // append good match 
    if (match >= 80)
    {
        return $"{firstName} matches {secondName} {match}%, good match";
    }

    // else just output final string using String Interpolation
    return $"{firstName} matches {secondName} {match}%";
}

