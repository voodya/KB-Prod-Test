using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Статический класс для генерации столбцов в сетке с гарантированным путем.
/// </summary>
public static class PathGenerator
{
    private static readonly Random _random = new Random();

    /// <summary>
    /// Генерирует следующий столбец для существующей сетки.
    /// </summary>
    /// <param name="grid">Текущая сетка (список столбцов).</param>
    /// <param name="canReachLastColumn">Массив, показывающий, какие ячейки в последнем столбце были достижимы.</param>
    /// <param name="rowCount">Количество строк в сетке.</param>
    /// <returns>Новый сгенерированный столбец.</returns>
    public static int[] GenerateNextColumn(IReadOnlyList<int[]> grid, bool[] canReachLastColumn, int rowCount)
    {
        // Попытка сгенерировать столбец, который не блокирует все пути
        for (int attempt = 0; attempt < 50; attempt++)
        {
            var newColumn = GenerateRandomColumn(rowCount);
            var canReachNewColumn = CalculateReachability(newColumn, canReachLastColumn, rowCount);

            if (canReachNewColumn.Any(canReach => canReach))
            {
                return newColumn;
            }
        }

        // Если не удалось, создаем путь принудительно
        return ForceCreatePath(canReachLastColumn, rowCount);
    }

    /// <summary>
    /// Вычисляет достижимость для потенциального нового столбца.
    /// Этот метод можно сделать public, если нужно проверять столбцы извне.
    /// </summary>
    public static bool[] CalculateReachability(int[] newColumn, bool[] canReachLastColumn, int rowCount)
    {
        var canReach = new bool[rowCount];

        // Шаг 1: Движение "Вперёд"
        for (int i = 0; i < rowCount; i++)
        {
            if (newColumn[i] == 0 && canReachLastColumn[i])
            {
                canReach[i] = true;
            }
        }

        // Шаг 2: Движение "Вверх" и "Вниз"
        for (int pass = 0; pass < rowCount; pass++)
        {
            for (int i = 0; i < rowCount - 1; i++) // Вниз
            {
                if (canReach[i] && newColumn[i + 1] == 0) canReach[i + 1] = true;
            }
            for (int i = rowCount - 1; i > 0; i--) // Вверх
            {
                if (canReach[i] && newColumn[i - 1] == 0) canReach[i - 1] = true;
            }
        }
        return canReach;
    }

    private static int[] GenerateRandomColumn(int rowCount)
    {
        var column = new int[rowCount];
        for (int i = 0; i < rowCount; i++)
        {
            column[i] = _random.Next(0, 2);
        }
        return column;
    }

    private static int[] ForceCreatePath(bool[] canReachLastColumn, int rowCount)
    {
        var newColumn = new int[rowCount];
        for (int i = 0; i < rowCount; i++) newColumn[i] = 1;

        var reachableIndexes = new List<int>();
        for (int i = 0; i < rowCount; i++)
        {
            if (canReachLastColumn[i]) reachableIndexes.Add(i);
        }

        if (reachableIndexes.Any())
        {
            int indexToOpen = reachableIndexes[_random.Next(reachableIndexes.Count)];
            newColumn[indexToOpen] = 0;
        }
        else
        {
            newColumn[_random.Next(rowCount)] = 0;
        }
        return newColumn;
    }
}

class Program
{
    static void Main(string[] args)
    {
        // --- Настройки ---
        int numberOfRows = 5;
        int numberOfColumns = 30;

        // --- Инициализация ---
        // Теперь `Main` управляет состоянием
        var grid = new List<int[]>();

        // Начальное состояние: все ячейки "виртуального" нулевого столбца достижимы
        bool[] canReachLastColumn = Enumerable.Repeat(true, numberOfRows).ToArray();

        Console.WriteLine($"Генерируем сетку {numberOfRows}x{numberOfColumns} с помощью статического класса...");

        // --- Основной цикл генерации ---
        for (int i = 0; i < numberOfColumns; i++)
        {
            // 1. Генерируем новый столбец на основе текущего состояния
            int[] newColumn = PathGenerator.GenerateNextColumn(grid, canReachLastColumn, numberOfRows);

            // 2. Добавляем его в нашу сетку
            grid.Add(newColumn);

            // 3. Обновляем состояние достижимости для следующей итерации
            canReachLastColumn = PathGenerator.CalculateReachability(newColumn, canReachLastColumn, numberOfRows);
        }

        PrintGrid(grid, numberOfRows);

        Console.WriteLine("\nГотово! Путь по пустым ячейкам всегда существует.");
    }

    /// <summary>
    /// Вспомогательный метод для вывода сетки в консоль.
    /// </summary>
    public static void PrintGrid(IReadOnlyList<int[]> grid, int rowCount)
    {
        if (!grid.Any()) return;

        Console.WriteLine(new string('-', grid.Count * 2 + 1));
        for (int row = 0; row < rowCount; row++)
        {
            Console.Write("|");
            foreach (var column in grid)
            {
                char cellChar = column[row] == 0 ? ' ' : '█';
                Console.Write($"{cellChar}|");
            }
            Console.WriteLine();
        }
        Console.WriteLine(new string('-', grid.Count * 2 + 1));
    }
}