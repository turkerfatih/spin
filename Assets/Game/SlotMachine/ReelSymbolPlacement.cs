using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{
    public static class ReelSymbolPlacement
    {
        public static void GenerateReelSymbols(ReelConfiguration reelDefinition, ref List<Symbol> result)
        {
            // Ensure the list exists and is reusable
            if (result == null)
            {
                result = new List<Symbol>();
            }
            else
            {
                result.Clear();
            }

            // Calculate total number of symbols
            int totalSymbols = reelDefinition.Items.Sum(item => item.Count);

            // Initialize result list with nulls to ensure proper size
            result.AddRange(Enumerable.Repeat<Symbol>(null, totalSymbols));

            // List of all available positions in the result list
            List<int> availablePositions = Enumerable.Range(0, totalSymbols).ToList();

            // Sort items by symbol count (most frequent symbols first)
            var sortedItems = reelDefinition.Items
                .Where(item => item.Count > 0) // Ensure no zero counts
                .OrderByDescending(item => item.Count)
                .ToList();

            // Place each symbol, starting with the most frequent
            foreach (var item in sortedItems)
            {
                PlaceSymbolsEvenly(result, availablePositions, item.SymbolType, item.Count);
            }
        }

        private static void PlaceSymbolsEvenly(List<Symbol> result, List<int> availablePositions, SymbolType symbolType,
            int count)
        {
            int totalAvailable = availablePositions.Count;
            if (count > totalAvailable)
            {
                throw new InvalidOperationException("Not enough positions to place symbols.");
            }

            // Calculate the ideal gap to spread symbols evenly
            int gap = totalAvailable / count;

            for (int i = 0; i < count; i++)
            {
                // Calculate the index in the available positions list
                int index = (i * gap) % availablePositions.Count;

                // Get the actual position in the result list
                int position = availablePositions[index];

                // Place the symbol in the result list
                result[position] = new Symbol(symbolType);

                // Remove the used position
                availablePositions.RemoveAt(index);
            }
        }

        public static void Generate(ReelConfiguration reelDefinition, ref List<Symbol> result)
        {

            var symList = GenerateDistribution(reelDefinition.Items);
            if (result == null)
            {
                result = new List<Symbol>();
            }
            else
            {
                result.Clear();
            }

            foreach (var symbolType in symList)
            {
                var symbol = new Symbol(symbolType);
                result.Add(symbol);
            }
        }

        static List<SymbolType> GenerateDistribution(List<ReelConfigurationItem> items)
        {
            var rationalPositions = new List<(double position, SymbolType item)>();

            // Generate rational positions for each item based on count
            foreach (var defItem in items)
            {
                var count = defItem.Count;
                for (int i = 1; i <= count; i++)
                {
                    double position = (double)i / (count+1 ); // Avoids positioning at the exact start or end
                    rationalPositions.Add((position, defItem.SymbolType));
                }
            }

            // Sort the items by their rational positions, then by name for tie-breaking
            rationalPositions = rationalPositions
                .OrderBy(x => x.position)
                .ThenBy(x => x.item)
                .ToList();

            // Extract sorted item sequence from sorted rational positions
            return rationalPositions.Select(x => x.item).ToList();
        }
    }
}