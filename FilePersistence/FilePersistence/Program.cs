using FilePersistence.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilePersistence
{
    class Program
    {
        static void Main(string[] args)
        {
            string textFilePath = "Data\\DataFile.txt";

            InitializeBakeryMenu(textFilePath);

            while (true)
            {
                DisplayMenu(textFilePath);
            }
            
        }

        /// <summary>
        /// Initialize the data file
        /// </summary>
        /// <param name="textfilePath"></param>
        static void InitializeBakeryMenu(string textfilePath)
        {
            List<BakeryMenu> BakeryMenuClassList = new List<BakeryMenu>();

            BakeryMenuClassList.Add(new BakeryMenu() {Flavor = "Chocolate", Category = "Cake", Price = 8.95, Type="Food"});

            WriteMenuItemsToTextFile(BakeryMenuClassList, textFilePath);
        }
        /// <summary>
        /// Display All List Items
        /// </summary>
        /// <param name="BakeryMenuClassList"></param>
        static void DisplayMenuItems(List<BakeryMenu> BakeryMenuClassList)
        {

            foreach (BakeryMenu flavor in BakeryMenuClassList)
            {
                Console.WriteLine("{0} \t {1}\t {2}\t {3}");
            }

            Console.WriteLine("Press any key to continue.");
            Console.ReadKey(true);
        }

        /// <summary>
        /// Writes list to data file
        /// </summary>
        /// <param name="BakeryMenuClassList"></param>
        /// <param name="dataFile"></param>
        static void WriteMenuItemsToTextFile(List<BakeryMenu> BakeryMenuClassList, string dataFile)
        {
            string menuItemString;

            List<string> menuItemsStringListWrite = new List<string>();

            // build list to write to the text file
            foreach (var flavor in BakeryMenuClassList)
            {
                menuItemString = flavor.Flavor + "," + flavor.Category + "," + flavor.Price + "," + flavor.Type;
                menuItemsStringListWrite.Add(menuItemString);
            }

            File.WriteAllLines(dataFile, menuItemsStringListWrite);
        }

        static List<BakeryMenu> ReadMenuItemsFromTextFile(string dataFile)
        {
            const char delineator = ',';

            List<string> menuItemsStringList = new List<string>();

            List<BakeryMenu> BakeryMenuClassList = new List<BakeryMenu>();

            try
            {
                menuItemsStringList = File.ReadAllLines(dataFile).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            foreach (string menuItemString in menuItemsStringList)
            {
                //use split method to separate each item with delineator
                string[] details = menuItemString.Split(delineator);

                BakeryMenuClassList.Add(new BakeryMenu() { Flavor = details[0], Category = details[1], });
            }

            return BakeryMenuClassList;
        }

        /// <summary>
        /// Displays the main menu
        /// </summary>
        /// <param name="path"></param>
        static void DisplayMenu(string path)
        {
            ConsoleMenu view = new ConsoleMenu(120, 40);

            //display menu
            view.DrawMenu(28, 15, new List<string>() { "1. Display All Menu Items", "2. Add An Item", "3. Delete An Item", "4. Update An Item", "5. Delete Entire Menu", "6. Exit" });

            //get user choice
            switch (view.PromptKey())
            {
                case ConsoleKey.D1:
                    DisplayAllRecords(path);
                    break;
                case ConsoleKey.D2:
                    AddRecord(path, view);
                    break;
                case ConsoleKey.D3:
                    DeleteRecord(path, view);
                    break;
                case ConsoleKey.D4:
                    UpdateRecord(path, view);
                    break;
                case ConsoleKey.D5:
                    WriteMenuItemsToTextFile(new List<BakeryMenu>(), path);
                    break;
                case ConsoleKey.D6:
                    Environment.Exit(1);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Displays the data
        /// </summary>
        /// <param name="path"></param>
        static void DisplayAllRecords(string path)
        {
            //open file
            List<BakeryMenu> Items = ReadMenuItemsFromTextFile(path);

            Console.Clear();

            DisplayMenuItems(Items);
        }

        /// <summary>
        /// Adds record the list
        /// </summary>
        /// <param name="path"></param>
        static void AddRecord(string path, ConsoleMenu view)
        {
            double addPrice;
            bool itemFound = false;

            //open file
            List<BakeryMenu> items = ReadMenuItemsFromTextFile(path);

            //prompt user to add a new Flavor
            view.DrawPromptBox("Enter New Flavor: ");
            string addFlavor = Console.ReadLine();

            //prompt user to choose a category
            view.DrawPromptBox("Enter Category: ");
            string addCategory = Console.ReadLine();

            //prompt user to choose a type
            view.DrawPromptBox("Enter a Type: ");
            string addType = Console.ReadLine();

            //prompt user to enter a price
            view.DrawPromptBox("What is the Item's price?: ");
            string response = Console.ReadLine();

            if (double.TryParse(response, out addPrice)) // Try to parse the string as an integer
            {
                items.Add(new BakeryMenu(addFlavor, addCategory, addPrice, addType));
            }
            else
            {
                Console.WriteLine("Not a valid price!");
            }

        }

        /// <summary>
        /// Deletes data from the file
        /// </summary>
        /// <param name="path"></param>
        /// <param name="view"></param>
        static void DeleteRecord(string path, ConsoleMenu view)
        {
            //read file
            List<BakeryMenu> items = ReadMenuItemsFromTextFile(path);

            //prompt user to delete an item
            view.DrawPromptBox("Which item to delete?");
            string response = Console.ReadLine();

            //remove menu item
            items.RemoveAll(item => item.Flavor == response);

            //update file
            WriteMenuItemsToTextFile(items, path);
        }

        /// <summary>
        /// Updates data from the file
        /// </summary>
        /// <param name="path"></param>
        /// <param name="view"></param>
        static void UpdateRecord(string path, ConsoleMenu view)
        {
            int newItem;
            bool itemFound = false;

            //read file
            List<BakeryMenu> items = ReadMenuItemsFromTextFile(path);

            //prompt user to update a menu item
            view.DrawPromptBox("Which Item would you like to update?");
            string response = Console.ReadLine();

            foreach (BakeryMenu item in items)
            {
                if (item.ID == response)
                    itemFound = true;
            }

            if (itemFound)
            {
                string score;

                view.DrawPromptBox("Enter new price:");
                score = Console.ReadLine();

                while (!Int32.TryParse(score, out newPrice))
                {
                    view.DrawPromptBox("Enter a valid price:");
                    score = Console.ReadLine();
                }

            }
            else
            {
                Console.Clear();
                Console.CursorVisible = false;
                view.DrawTextBox("No item found with that ID. Press any key to continue.");
                Console.ReadKey(true);
                return;
            }

            //update score
            foreach (BakeryMenu item in items)
            {
                if (item.ID == response)
                {
                    item.Price = newPrice;
                }
            }

            //update file
            WriteMenuItemsToTextFile(items, path);
        }
    }
}
        