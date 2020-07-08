using System.Collections.Generic;

namespace sudoku.Models
{
    public class HomeViewModel
    {
        public HomeViewModel()
        {
            Grid = new Dictionary<string, string>();

            //for (var gridId = 1; gridId <= 81; gridId++)
            //{
            //    Grid.Add(gridId.ToString(), "");
            //}
        }

        public Dictionary<string, string> Grid;
    }
}
