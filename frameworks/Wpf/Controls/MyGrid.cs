using Extensions;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Wpf.Controls
{
    public class MyGrid : Grid
    {
        #region fields

        private string _rows;
        private string _columns;
        private double _rowGap = 10;
        private double _columnGap = 10;

        #endregion

        #region props

        public string Rows
        {
            get => _rows;
            set { _rows = value; UpdateGrid(); }
        }

        public string Columns
        {
            get => _columns;
            set { _columns = value; UpdateGrid(); }
        }

        public double RowGap
        {
            get => _rowGap;
            set { _rowGap = value; UpdateGrid(); }
        }

        public double ColumnGap
        {
            get => _columnGap;
            set { _columnGap = value; UpdateGrid(); }
        }

        #endregion

        #region methods

        private void UpdateGrid()
        {
            ColumnDefinitions.Clear();
            RowDefinitions.Clear();

            if (!_columns.IsEmpty())
                UpdateColumns();

            if (!_rows.IsEmpty())
                UpdateRows();
        }

        private void UpdateColumns()
        {
            string[] columns = _columns.Split(' ');
            foreach (string column in columns)
            {
                GridLength gridLength;

                if (double.TryParse(column, out double width))
                    gridLength = new GridLength(width);
                else
                {
                    if (column.ToLower() == "auto")
                        gridLength = new GridLength(0, GridUnitType.Auto);
                    else if (column == "*")
                        gridLength = new GridLength(0, GridUnitType.Star);
                    else
                        continue;
                }

                ColumnDefinitions.Add(new ColumnDefinition() { Width = gridLength });

                if (column != columns.Last() && ColumnGap > 0)
                {
                    var gap = new GridLength(ColumnGap);
                    ColumnDefinitions.Add(new ColumnDefinition() { Width = gap });
                }
            }
        }

        private void UpdateRows()
        {
            var rows = _rows.Split(' ').ToList();

            foreach (string row in rows)
            {
                GridLength gridLength;

                if (double.TryParse(row, out double height))
                    gridLength = new GridLength(height);
                else
                {
                    if (row.ToLower() == "auto")
                        gridLength = new GridLength(1, GridUnitType.Auto);
                    else if (row == "*")
                        gridLength = new GridLength(1, GridUnitType.Star);
                    else
                        continue;
                }

                RowDefinitions.Add(new RowDefinition() { Height = gridLength });

                if (rows.IndexOf(row) < rows.Count - 1)
                {
                    var gap = new GridLength(RowGap);
                    RowDefinitions.Add(new RowDefinition() { Height = gap });
                }
            }
        }

        #endregion
    }
}
