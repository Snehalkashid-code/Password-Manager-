using System;
using System.Collections.Generic;
using System.Text;

class Program
{
    static List<string[]> passwords = new List<string[]>();
    static string masterPassword = "admin123";
    static string key = "SecretKey99";

    static string Encrypt(string text)
    {
        char[] chars = text.ToCharArray();
        for (int i = 0; i < chars.Length; i++)
            chars[i] = (char)(chars[i] ^ key[i % key.Length]);
        return Convert.ToBase64String(Encoding.UTF8.GetBytes(new string(chars)));
    }

    static string Decrypt(string encryptedText)
    {
        byte[] bytes = Convert.FromBase64String(encryptedText);
        char[] chars = Encoding.UTF8.GetString(bytes).ToCharArray();
        for (int i = 0; i < chars.Length; i++)
            chars[i] = (char)(chars[i] ^ key[i % key.Length]);
        return new string(chars);
    }

    static string GeneratePassword(int length)
    {
        string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%";
        Random rnd = new Random();
        char[] pass = new char[length];
        for (int i = 0; i < length; i++)
            pass[i] = chars[rnd.Next(chars.Length)];
        return new string(pass);
    }

    static void AddPassword()
    {
        Console.Write("Website/App Name: ");
        string website = Console.ReadLine();

        Console.Write("Username or Email: ");
        string username = Console.ReadLine();

        Console.Write("Password: ");
        string password = Console.ReadLine();

        passwords.Add(new string[] { website, username, Encrypt(password) });
        Console.WriteLine("Password saved successfully.");
    }

    static void ViewPasswords()
    {
        if (passwords.Count == 0)
        {
            Console.WriteLine("No passwords saved yet.");
            return;
        }

        Console.WriteLine("\n--- All Passwords ---");
        for (int i = 0; i < passwords.Count; i++)
        {
            Console.WriteLine("Number   : " + (i + 1));
            Console.WriteLine("Website  : " + passwords[i][0]);
            Console.WriteLine("Username : " + passwords[i][1]);
            Console.WriteLine("Password : " + Decrypt(passwords[i][2]));
            Console.WriteLine("---------------------");
        }
    }

    static void SearchPassword()
    {
        Console.Write("Enter website name to search: ");
        string search = Console.ReadLine().ToLower();

        bool found = false;
        foreach (var entry in passwords)
        {
            if (entry[0].ToLower().Contains(search))
            {
                Console.WriteLine("\nWebsite  : " + entry[0]);
                Console.WriteLine("Username : " + entry[1]);
                Console.WriteLine("Password : " + Decrypt(entry[2]));
                found = true;
            }
        }

        if (!found)
            Console.WriteLine("No entry found.");
    }

    static void DeletePassword()
    {
        if (passwords.Count == 0)
        {
            Console.WriteLine("No passwords to delete.");
            return;
        }

        ViewPasswords();
        Console.Write("Enter number to delete: ");

        if (int.TryParse(Console.ReadLine(), out int num) && num >= 1 && num <= passwords.Count)
        {
            string name = passwords[num - 1][0];
            passwords.RemoveAt(num - 1);
            Console.WriteLine(name + " deleted successfully.");
        }
        else
        {
            Console.WriteLine("Invalid number.");
        }
    }

    static void Main()
    {
        Console.WriteLine("=== PASSWORD MANAGER ===");
        Console.Write("Enter Master Password: ");
        string input = Console.ReadLine();

        if (input != masterPassword)
        {
            Console.WriteLine("Wrong password. Access denied.");
            return;
        }

        Console.WriteLine("Login successful. Welcome!\n");

        while (true)
        {
            Console.WriteLine("\n=========================");
            Console.WriteLine("1. Add Password");
            Console.WriteLine("2. View All Passwords");
            Console.WriteLine("3. Search Password");
            Console.WriteLine("4. Delete Password");
            Console.WriteLine("5. Generate Strong Password");
            Console.WriteLine("6. Exit");
            Console.WriteLine("=========================");
            Console.Write("Enter Choice: ");

            string choice = Console.ReadLine();

            if (choice == "1")
                AddPassword();
            else if (choice == "2")
                ViewPasswords();
            else if (choice == "3")
                SearchPassword();
            else if (choice == "4")
                DeletePassword();
            else if (choice == "5")
            {
                Console.Write("Enter password length (8 to 20): ");
                if (int.TryParse(Console.ReadLine(), out int len) && len >= 8 && len <= 20)
                    Console.WriteLine("Generated Password: " + GeneratePassword(len));
                else
                    Console.WriteLine("Invalid length. Enter between 8 and 20.");
            }
            else if (choice == "6")
            {
                Console.WriteLine("Goodbye!");
                break;
            }
            else
            {
                Console.WriteLine("Invalid choice. Try again.");
            }
        }
    }
}
