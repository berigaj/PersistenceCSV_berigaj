using FilePersistence.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FilePersistence.View;

namespace FilePersistence
{
    class Program
    {
        //view for program
        static ConsoleMenu view = new ConsoleMenu(120, 40);

        /// <summary>
        /// Main function
        /// </summary>
        /// <param name="args"></param>
        private static void Main(string[] args)
        {
            string textFilePath = "Data\\DataFile.txt";
            Console.BackgroundColor = ConsoleColor.DarkCyan;
            Console.ForegroundColor = ConsoleColor.White;

            InitializeBakeryMenu(textFilePath);

            List<BakeryMenu> items;

            while (true)
            {
                // update menu list
                items = ReadMenuItemsFromTextFile(textFilePath);

                // dispaly menu
                DisplayMenu(textFilePath, items);
            }

        }

        /// <summary>
        /// Initialize the date file
        /// </summary>
        /// <param name="textFilePath"></param>
        static void InitializeBakeryMenu(string textFilePath)
        {
            List<BakeryMenu> bakeryMenuClassList = new List<BakeryMenu>();

            // initialize the IList of high scores - note: no instantiation for an interface
            bakeryMenuClassList.Add(new BakeryMenu() { Flavor = "Chocolate Cake", Price = 8.95 });
            bakeryMenuClassList.Add(new BakeryMenu() { Flavor = "Lemon Cake", Price = 7.95 });
            bakeryMenuClassList.Add(new BakeryMenu() { Flavor = "Chocolate Eclair", Price = 2.35 });
            bakeryMenuClassList.Add(new BakeryMenu() { Flavor = "Cherry Tart", Price = 9.45 });

            WriteMenuItemsToTextFile(bakeryMenuClassList, textFilePath);
        }

        /// <summary>
        /// Writes list to data file
        /// </summary>
        /// <param name="BakeryMenuClassList"></param>
        /// <param name="dataFile"></param>
        static void WriteMenuItemsToTextFile(List<BakeryMenu> bakeryMenuClassList, string dataFile)
        {
            string menuItemString;

            List<string> menuItemsStringListWrite = new List<string>();

            // build list to write to the text file
            foreach (var item in bakeryMenuClassList)
            {
                menuItemString = item.Flavor + "," + item.Price;
                menuItemsStringListWrite.Add(menuItemString);
            }

            File.WriteAllLines(dataFile, menuItemsStringListWrite);
        }

        static List<BakeryMenu> ReadMenuItemsFromTextFile(string dataFile)
        {
            const char delineator = ',';

            List<string> menuItemsStringList = new List<string>();

            List<BakeryMenu> bakeryMenuClassList = new List<BakeryMenu>();

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

                bakeryMenuClassList.Add(new BakeryMenu() { Flavor = details[0], Price = Convert.ToDouble(details[1]) });
            }

            return bakeryMenuClassList;
        }

        /// <summary>
        /// Displays the main menu
        /// </summary>
        /// <param name="path"></param>
        static void DisplayMenu(string path, List<BakeryMenu> items)
        {
            //display menu
            view.DrawMenu(28, 15, new List<string>()
            { "1. Display All Menu Items", "2. Add An Item", "3. Delete An Item", "4. Update An Item", "5. Delete Entire Menu", "6. Exit" });

            //get user choice
            switch (view.PromptKey())
            {
                case ConsoleKey.D1:
                    DisplayAllRecords(path, items);
                    break;
                case ConsoleKey.D2:
                    AddRecord(path, items);
                    break;
                case ConsoleKey.D3:
                    DeleteRecord(path, items);
                    break;
                case ConsoleKey.D4:
                    UpdateRecord(path, items);
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

        static void DisplayAllRecords(string path, List<BakeryMenu> items)
        {
            Console.Clear();

            if (items.Count >= 18) Console.BufferHeight *= 2;

            //-------------------
            int gridRowNum = items.Count;
            int gridColNum = 2;
            int gridCellWidth = 20;
            int gridCellHeight = 1;
            int gridX = 120 / 2 - ((gridColNum * gridCellWidth) + gridColNum + 1) / 2;
            int gridY = 3;

            view.DrawGrid(gridX, gridY, gridRowNum, gridColNum, gridCellWidth, gridCellHeight);

            //-------------------

            int i = 1;
            foreach (BakeryMenu item in items)
            {
                view.WriteAt(gridX + 1, gridY + 1 * i, item.Flavor);
                view.WriteAt(gridX + gridCellWidth + 2, gridY + 1 * i, $"{item.Price}");
                i += 2;
            }

            Console.WriteLine("\n\n Press Any Key To Continue.");
            Console.ReadKey(true);

            
            Console.BufferHeight = 60;

        }

        /// <summary>
        /// Adds record the list
        /// </summary>
        /// <param name="path"></param>
        static void AddRecord(string path, List<BakeryMenu> items)
        {
            double addPrice;

            //prompt user to add a new Flavor
            view.DrawPromptBox("Enter New Item: ");
            string addFlavor = Console.ReadLine();

            //do update instead of add for new item
            foreach (BakeryMenu item in items)
            {
                if (item.Flavor.ToLower() == addFlavor.ToLower())
                {
                    UpdateRecord(path, items, true, item.Flavor);
                    return;
                }
            }

            view.DrawPromptBox("What is the Item's price?: ");
            string response = Console.ReadLine();

            if (double.TryParse(response, out addPrice)) // Try to parse the string as an integer
            {
                items.Add(new BakeryMenu(addFlavor, addPrice));
            }
            else
            {
                Console.WriteLine("Not a valid price!");
            }

            WriteMenuItemsToTextFile(items, path);

        }

        /// <summary>
        /// Deletes data from the file
        /// </summary>
        /// <param name="path"></param>
        /// <param name="view"></param>
        static void DeleteRecord(string path, List<BakeryMenu> items)
        {
            //prompt user to delete an item
            view.DrawPromptBox("Which item to delete?");
            string response = Console.ReadLine();

            //remove menu item
            items.RemoveAll(item => item.Flavor == response);

            //update file
            WriteMenuItemsToTextFile(items, path);
        }

        /// <summary>
        /// Updates an item
        /// </summary>
        /// <param name="path">file path</param>
        /// <param name="view">view</param>
        /// <param name="items">list of highscores</param>
        /// <param name="alreadyFound">is this coming from add highscore?</param>
        /// <param name="name">name of player already found</param>
        static void UpdateRecord(string path, List<BakeryMenu> items, bool alreadyFound = false, string name = "")
        {
            double newPrice;
            bool itemFound = false;
            string response;

            //handle if item was found in AddItem
            if (alreadyFound)
            {
                itemFound = true;
                response = name;
            }
            else
            {
                //prompt user to update a price
                view.DrawPromptBox("Which Item's price to update?");
                response = Console.ReadLine();

                foreach (BakeryMenu item in items)
                {
                    if (item.Flavor == response)
                        itemFound = true;
                }
            }

            //menu item found
            if (itemFound)
            {
                string price;

                view.DrawPromptBox("Enter new price:");
                price = Console.ReadLine();

                while (!double.TryParse(price, out newPrice))
                {
                    view.DrawPromptBox("Enter a valid price:");
                    price = Console.ReadLine();
                }

            }
            else
            {
                Console.Clear();
                Console.CursorVisible = false;
                view.DrawTextBox("No menu item found with that name. Press any key to continue.");
                Console.ReadKey(true);
                return;
            }

            //update price
            foreach (BakeryMenu item in items)
            {
                if (item.Flavor == response)
                {
                    item.Price = newPrice;
                }
            }

            //update file
            WriteMenuItemsToTextFile(items, path);
        }

        /// <summary>
        /// Delete all records in file
        /// </summary>
        static void DeleteAllRecords(string path)
        {
            view.DrawPromptBox("Delete all records? (yes / no): ");
            string response = Console.ReadLine().ToUpper();

            if (response == "YES")
                WriteMenuItemsToTextFile(new List<BakeryMenu>(), path);
        }
    }
}
        