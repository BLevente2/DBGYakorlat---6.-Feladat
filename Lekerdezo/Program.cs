using System;
using System.IO;
using System.Globalization;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("This program processes a text file to calculate and report the number of cars, the average price, the number of red cars, and details about the most expensive car.");

        Console.Write("Please enter the file path: ");
        string filePath = Console.ReadLine();

        if (string.IsNullOrEmpty(filePath))
        {
            Console.WriteLine("No file path provided. Exiting.");
            return;
        }

        while (!File.Exists(filePath))
        {
            Console.WriteLine($"File not found at: {filePath}");
            Console.Write("Please provide the correct file path: ");
            filePath = Console.ReadLine();
            if (string.IsNullOrEmpty(filePath))
            {
                Console.WriteLine("No file path provided. Exiting.");
                return;
            }
        }

        int totalCars = 0;
        int redCarsCount = 0;
        double totalPrice = 0;
        double maxPrice = double.MinValue;
        string mostExpensiveCar = "";

        try
        {
            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                try
                {
                    string[] parts = line.Split(',');
                    if (parts.Length != 3)
                    {
                        Console.WriteLine($"Invalid format: {line}");
                        continue;
                    }

                    string id = parts[0].Trim();
                    string priceStr = parts[1].Replace(" Ft", "").Trim().Replace(" ", "");
                    string color = parts[2].Trim();

                    if (!double.TryParse(priceStr, NumberStyles.Any, CultureInfo.InvariantCulture, out double price))
                    {
                        Console.WriteLine($"Invalid price format in: {line}");
                        continue;
                    }

                    totalCars++;
                    totalPrice += price;

                    if (color.Equals("piros", StringComparison.OrdinalIgnoreCase))
                    {
                        redCarsCount++;
                    }

                    if (price > maxPrice)
                    {
                        maxPrice = price;
                        mostExpensiveCar = line;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing line: {line}");
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }

            if (totalCars > 0)
            {
                double averagePrice = totalPrice / totalCars;
                Console.WriteLine($"Average price: {averagePrice:F2} Ft");
            }
            else
            {
                Console.WriteLine("No records to process.");
            }

            Console.WriteLine($"Number of red cars: {redCarsCount}");

            if (!string.IsNullOrEmpty(mostExpensiveCar))
            {
                Console.WriteLine($"Most expensive car: {mostExpensiveCar}");
            }
            else
            {
                Console.WriteLine("No data on the most expensive car.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing file: {ex.Message}");
        }
    }
}
