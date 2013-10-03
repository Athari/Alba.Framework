using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Alba.Framework.Windows.Controls
{
    public static partial class GridProps
    {
        private static void CalculateCellSizes_Changed (Grid grid, DpChangedEventArgs<bool> args)
        {
            if (args.NewValue)
                grid.SizeChanged += Grid_OnSizeChanged;
            else
                grid.SizeChanged -= Grid_OnSizeChanged;
        }

        private static void Grid_OnSizeChanged (object sender, SizeChangedEventArgs args)
        {
            var grid = (Grid)sender;
            foreach (RowDefinition row in grid.RowDefinitions)
                SetRowActualHeight(row, row.ActualHeight);
            foreach (ColumnDefinition column in grid.ColumnDefinitions)
                SetColumnActualWidth(column, column.ActualWidth);
        }

        private static void ForceCellSizes_Changed (Grid grid, DpChangedEventArgs<bool> args)
        {
            if (args.NewValue) {
                Action initDummyGrid = () => {
                    Grid parentGrid = (Grid)grid.Parent, dummyGrid = CreateDummyGrid(grid);
                    parentGrid.Children.Add(dummyGrid);
                    SetDummyGrid(grid, dummyGrid);
                };
                if (grid.IsLoaded)
                    initDummyGrid();
                else
                    grid.Loaded += (o, e) => initDummyGrid();
            }
            else {
                Grid parentGrid = (Grid)grid.Parent, dummyGrid = DestroyDummyGrid(grid);
                parentGrid.Children.Remove(dummyGrid);
                SetDummyGrid(grid, null);
            }
        }

        private static Grid CreateDummyGrid (Grid grid)
        {
            var dummyGrid = new Grid { Visibility = Visibility.Hidden };
            SetCalculateCellSizes(dummyGrid, true);
            foreach (RowDefinition row in grid.RowDefinitions) {
                var dummyRow = new RowDefinition { Height = row.Height, MinHeight = row.MinHeight, MaxHeight = row.MaxHeight };
                dummyGrid.RowDefinitions.Add(dummyRow);
                BindingOperations.SetBinding(row, RowDefinition.HeightProperty,
                    new Binding { Source = dummyRow, Path = new PropertyPath(RowActualHeightProperty) });
            }
            foreach (ColumnDefinition column in grid.ColumnDefinitions) {
                var dummyColumn = new ColumnDefinition { Width = column.Width, MinWidth = column.MinWidth, MaxWidth = column.MaxWidth };
                dummyGrid.ColumnDefinitions.Add(dummyColumn);
                BindingOperations.SetBinding(column, ColumnDefinition.WidthProperty,
                    new Binding { Source = dummyColumn, Path = new PropertyPath(ColumnActualWidthProperty) });
            }
            return dummyGrid;
        }

        private static Grid DestroyDummyGrid (Grid grid)
        {
            Grid dummyGrid = GetDummyGrid(grid);
            SetCalculateCellSizes(dummyGrid, false);
            foreach (RowDefinition row in grid.RowDefinitions)
                BindingOperations.ClearBinding(row, RowDefinition.HeightProperty);
            foreach (ColumnDefinition column in grid.ColumnDefinitions)
                BindingOperations.ClearBinding(column, ColumnDefinition.WidthProperty);
            return dummyGrid;
        }
    }
}