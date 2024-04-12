using System;

namespace shared.Managers
{
    public class PasswordGenerator
    {
        private const string Numbers = "0123456789";
        private const string LowercaseLetters = "abcdefghijklmnopqrstuvwxyz";
        private const string UppercaseLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string Symbols = "!@#$%^&*()-_=+[{]};:'\",<.>/?";

        private static Random random = new Random();

        public static string GeneratePassword(int length)
        {
            if (length < 8)
            {
                throw new ArgumentException("Password length must be at least 8 characters.");
            }

            string allCharacters = Numbers + LowercaseLetters + UppercaseLetters + Symbols;
            char[] password = new char[length];

            // Add at least one character from each character set
            password[0] = Numbers[random.Next(Numbers.Length)];
            password[1] = LowercaseLetters[random.Next(LowercaseLetters.Length)];
            password[2] = UppercaseLetters[random.Next(UppercaseLetters.Length)];
            password[3] = Symbols[random.Next(Symbols.Length)];

            // Fill the rest of the password with random characters
            for (int i = 4; i < length; i++)
            {
                password[i] = allCharacters[random.Next(allCharacters.Length)];
            }

            // Shuffle the password characters to randomize the order
            for (int i = 0; i < length; i++)
            {
                int swapIndex = random.Next(length);
                char temp = password[i];
                password[i] = password[swapIndex];
                password[swapIndex] = temp;
            }

            return new string(password);
        }
    }
}
