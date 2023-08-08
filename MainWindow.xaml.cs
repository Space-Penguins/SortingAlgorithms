using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace WpfSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int[]? arr = null;
        private ColumnSeries<int>? columnSeries = null;
        private int sortingSpeed = 500;
        private int arraySize = 10;

        public int[] SetSize { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            InitializeChart();

            SetSize = new int[] { 10, 100, 1000, 10000 };

            // Bind the ComboBox's ItemsSource to the array
            ArraySelect.ItemsSource = SetSize;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            arraySize = (int)ArraySelect.SelectedItem;
            await InitializeAsync();
        }

        private void GenerateNewRandomArray_Click(object sender, RoutedEventArgs e)
        {
            arraySize = (int)ArraySelect.SelectedItem;
            arr = RandomArray(arraySize, 0, arraySize + 1);
            UpdateChart(arr);
        }

        private async void SelectionSortStart_Click(object sender, RoutedEventArgs e)
        {
            await SelectionSort(arr);
        }

        private async void BubbleSortStart_Click(object sender, RoutedEventArgs e)
        {
            await BubbleSort(arr);
        }

        private async void InsertionSortStart_Click(object sender, RoutedEventArgs e)
        {
            await InsertionSort(arr);
        }

        private async void QuickSortStart_Click(object sender, RoutedEventArgs e)
        {
            await QuickSort(arr, 0, arr.Length - 1);
        }

        // Method to initialize the chart
        void InitializeChart()
        {
            // Create the ColumnSeries once
            columnSeries = new ColumnSeries<int>
            {
                EasingFunction = null, // Disable animations
            };

            DataContext = new ViewModel
            {
                Series = new ISeries[] { columnSeries }
            };
        }

        public async Task InitializeAsync()
        {

            arr = RandomArray(arraySize, 0, arraySize + 1);
            //await SelectionSort(arr); // Use await here to wait for the completion of the asynchronous method

            // Initialize the chart with the original unsorted array
            UpdateChart(arr);
        }

        // Method to update the chart with a new array
        void UpdateChart(int[] array)
        {
            // Update the Values property of the existing ColumnSeries
            columnSeries.Values = array;
        }

        public async Task LoadDataAsync()
        {
            arr = RandomArray(10, -10, 10);
            await SelectionSort(arr); // Use await here to wait for the completion of the asynchronous method

            // Initialize the chart with the original unsorted array
            UpdateChart(arr);
        }

        private void SortingSpeed_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (e is null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            sortingSpeed = (int)SortingSpeed.Value;
        }

        /*  RandomArray(l, min, max)
        Makes a random int[] array with l elements ranging from min to max.
        PRE: l must be a postivie integer, min must be smaller than max.
        EXAMPLES: RandomArray(0, 1, 2)                 => []
                    RandomArray(10, -10, 10)             => [-7, 8, 1, -9, 0, -3, 6, 7, -2, 7]
        */
        int[] RandomArray(int length, int min, int max)
        {
            if (min > max)
                throw new System.Exception("min must be smaller than max.");

            int[] arr = new int[length];
            Random number = new();

            // Build array with random numbers
            for (int i = 0; i < length; i++)
            {
                arr[i] = number.Next(min, max);
            }
            return arr;
        }

        /*  Swap(arr,i,j)
            Takes an array and two integers. Swaps the elements on position i and j.
            PRE: Array must be an []int array and not be empty, i and j must be between in range of the array
            EXAMPLES: Swap(new int[] {0}, 0, 0)                       => [0]
                        Swap(new int[] { 5, 4, 3, 2, 1 }, 0, 1)         => [2, 1, 3, 4, 5]
        */
        void Swap(int[] arr, int i, int j)
        {
            (arr[i], arr[j]) = (arr[j], arr[i]);
        }

        /*  SelectionSort(arr)
            Sort an array from lowest to highest using the selection sort algorithm.
            The array in selection sort could be viewd as having two parts, the sorted and unsorted part.
            It repeatedly finds the smallest element in the unsorted part and Swaps it with the first element of the unsorted part.
            This expands the sorted part of the array.
            PRE: Array must be an []int array.
            COMPLEXITY: O(N^2)
                        Becaouse it is a nested loop and every loop takes O(N)
            EXAMPLES: SelectionSort(new int[] {})                        => []
                        SelectionSort(new int[] {42})                      => [42]
                        SelectionSort(new int[] { 5, 4, 3, 2, 1 })         => [1, 2, 3, 4, 5]
                        SelectionSort(new int[] {-3,2,-9,-3,1,3,-5,5,9,2}) => [-9, -5, -3, -3, 1, 2, 2, 3, 5]
        */
        async Task SelectionSort(int[] arr)
        {
            int n = arr.Length;

            // Iterate through the array and moves the boundary of the sorted part of the array
            for (int i = 0; i < n - 1; i++)
            {
                int minIndex = i;
                // Iterate through the arrays unsorted part to look for a lower number
                for (int j = i + 1; j < n; j++)
                {
                    if (arr[j] < arr[minIndex])
                    {
                        minIndex = j;
                    }
                }
                // Swap the lowest element in the unsorted part of the array with the current element
                Swap(arr, minIndex, i);

                // Update the chart with the current array after each step
                UpdateChart(arr);

                // Introduce a delay to visualize the sorting process
                await Task.Delay(sortingSpeed);
            }
        }

        /*  BubbleSort(arr)
            Sort an array from lowest to highest using the bubble sort algorithm.
            Iterate through the array, compairing the current element to the adjacent element.
            The higher one is place at the right side.
            PRE: Array must be an []int array.
            SIDE EFFECT: Print sorted array to terminal.
            COMPLEXITY: O(N^2)
                        Becaouse it is a nested loop and every loop takes O(N)
            EXAMPLES: SelectionSort(new int[] {})                        => []
                        SelectionSort(new int[] {42})                      => [42]
                        SelectionSort(new int[] { 5, 4, 3, 2, 1 })         => [1, 2, 3, 4, 5]
                        SelectionSort(new int[] {-3,2,-9,-3,1,3,-5,5,9,2}) => [-9, -5, -3, -3, 1, 2, 2, 3, 5]
        */
        async Task BubbleSort(int[] arr)
        {
            int n = arr.Length;

            for (int i = 0; i < n - 1; i++)
            {
                bool Swapped = false;

                for (int j = 0; j < n - i - 1; j++)
                {
                    if (arr[j] > arr[j + 1])
                    {
                        // Swap place with the left element with the right element
                        Swap(arr, j, j + 1);
                        UpdateChart(arr);
                        await Task.Delay(sortingSpeed);
                        Swapped = true;
                    }
                }
                // If there were no Swap in the inner loop, then break 
                if (!Swapped)
                {
                    break;
                }
            }
        }

        /*  InsertionSort(arr)
            Sort an array from lowest to highest using the insertion sort algorithm.
            Iterate through the array, compairing the current element to the adjacent element.
            If the adjacent element is smaller than the current element, move the greater element up one position
            to make space for the smaller element.
            PRE: array must be an []int array.
            COMPLEXITY: O(N^2)
                        Becaouse it is a nested loop and every loop takes O(N)
            EXAMPLES: SelectionSort(new int[] {})                        => []
                        SelectionSort(new int[] {42})                      => [42]
                        SelectionSort(new int[] { 5, 4, 3, 2, 1 })         => [1, 2, 3, 4, 5]
                        SelectionSort(new int[] {-3,2,-9,-3,1,3,-5,5,9,2}) => [-9, -5, -3, -3, 1, 2, 2, 3, 5]
        */
        async Task InsertionSort(int[] arr)
        {
            int n = arr.Length;

            for (int i = 0; i < n - 1; i++)
            {
                if (arr[i] > arr[i + 1])
                {
                    // Find where in the sorted part of the array to put the element
                    for (int j = i + 1; j >= 1; j--)
                    {
                        if (arr[j] < arr[j - 1])
                        {
                            // Swap place with the left element with the right element
                            Swap(arr, j, j - 1);
                            UpdateChart(arr);
                            await Task.Delay(sortingSpeed);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
        }

        /*  Partition(arr, low, high)
            Takes the last element of the array as a pivot and position it at the correct position in the sorted array.
            Then takes places all smaller elements to the left of the pivot and all the greater elements to the right.
            PRE: Array must be an []int array and low and high must be valid indices in the array, where low < high.
            EXAMPLES: Partition(new int[] {5}, 0, 0)                 => 0
                        Partition(new int[] {-3,2,-9,-3,1,3,-5,5,9,2}) => 5
        */
        int Partition(int[] arr, int low, int high)
        {
            int pivot = arr[high];
            int i = low - 1;

            for (int j = low; j < high; j++)
            {
                // If current element is smaller than the pivot
                if (arr[j] < pivot)
                {
                    // Increment index of smaller element
                    i++;
                    Swap(arr, i, j);
                }
            }
            Swap(arr, i + 1, high);
            return i + 1;
        }

        /*  QuickSort(arr, low, high)
            Sort an array from lowest to highest using the quick sort algorithm.
            Taking the last element of the array as pivot point and partitions the array around the pivot,
            the smaller elements to the left and the greater to the right of the pivot. This is done recursively
            for each new partition.
            PRE: Array must be an []int array and low and high must be valid indices in the array, where low < high.
            COMPLEXITY: O(N*log(N))
                        Becaouse the number of levels in the recursion tree is logarithmic to the size of the input array.
            EXAMPLES: QuickSort(new int[] {}, 0, 0)                        => []
                        QuickSort(new int[] {-3,2,-9,-3,1,3,-5,5,9,2}, 0, 9) => [-9, -5, -3, -3, 1, 2, 2, 3, 5]
        */
        async Task QuickSort(int[] arr, int low, int high)
        {
            if (low < high)
            {
                UpdateChart(arr);
                await Task.Delay(sortingSpeed);

                int pi = Partition(arr, low, high);

                await QuickSort(arr, low, pi - 1);
                await QuickSort(arr, pi + 1, high);
            }
        }
    }
}