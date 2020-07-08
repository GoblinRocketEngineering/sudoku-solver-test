using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.Extensions.Logging;
using sudoku.Models;
using sudoku.Solvers;

namespace sudoku.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        private const string dictionaryBoardDelimiter = "|";

        private Dictionary<string, string> convertBoard(int[,] board)
        {
            var dictionaryBoard = new Dictionary<string, string>();

            for (var x = 0; x < board.GetLength(0); x++)
            {
                for (var y = 0; y < board.GetLength(1); y++)
                {
                    var value = board[x, y];
                    dictionaryBoard[$"{x}{dictionaryBoardDelimiter}{y}"] = value == 0 ? "" : value.ToString();
                }
            }

            return dictionaryBoard;
        }
        private int[,] convertBoard(Dictionary<string, string> dictionaryBoard)
        {
            var boardSize = determineBoardSize(dictionaryBoard);
            var board = new int[boardSize, boardSize];

            foreach(var cell in dictionaryBoard)
            {
                var coords = cell.Key.Split(dictionaryBoardDelimiter);
                var x = Convert.ToInt32(coords[0]);
                var y = Convert.ToInt32(coords[1]);
                var value = string.IsNullOrWhiteSpace(cell.Value) ? 0 : Convert.ToInt32(cell.Value);

                board[x, y] = value;
            }

            return board;
        }
        private int determineBoardSize(Dictionary<string, string> dictionaryBoard) => Convert.ToInt32(Math.Sqrt(dictionaryBoard.Count));

        public IActionResult Index()
        {
            var board = new int[9, 9]
            {
                { 5, 6, 0, 0, 0, 2, 1, 7, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 4, 9 },
                { 4, 1, 0, 9, 0, 0, 5, 0, 0 },
                { 0, 0, 0, 0, 1, 0, 0, 0, 0 },
                { 7, 0, 0, 0, 0, 0, 0, 0, 4 },
                { 0, 0, 0, 0, 5, 0, 0, 0, 0 },
                { 0, 0, 7, 0, 0, 4, 0, 2, 8 },
                { 2, 3, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 5, 4, 2, 0, 0, 0, 9, 1 }
            };

            var model = new HomeViewModel { Grid = convertBoard(board) };

            return View(model);
        }

        [HttpPost]
        public IActionResult Index(Dictionary<string, string> grid)
        {
            var model = new HomeViewModel { Grid = grid };
            var solver = new SimpleSolver(determineBoardSize(grid));
            var board = convertBoard(grid);
            
            if (solver.Solve(board))
            {
                model.Grid = convertBoard(solver.GetFirstSolution());
            }

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
