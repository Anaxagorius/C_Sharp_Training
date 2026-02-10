#include <iostream>
#include <vector>
#include <utility>  // for std::swap

/**
 * Optimized Bubble Sort implementation.
 * 
 * Features:
 * - Early termination: if no swaps occur in a pass, the array is sorted.
 * - Minimal inner-loop bound reduction.
 * - Uses std::swap for clean, efficient swapping.
 * - Works on std::vector<int> by reference (in-place, no copy).
 * 
 * Time complexity:
 *   Worst/Average: O(n²)
 *   Best (already sorted): O(n) thanks to the early-exit flag
 * 
 * Note: Bubble Sort is primarily educational. For real-world use,
 * prefer std::sort (introsort, average O(n log n)) unless you have a
 * specific reason (e.g., teaching, very small fixed-size arrays, or
 * hardware constraints where predictable O(n²) worst-case is acceptable).
 */
void bubbleSort(std::vector<int>& arr)
{
    if (arr.size() <= 1) {
        return;  // Already sorted
    }

    bool swapped;
    for (size_t i = 0; i < arr.size() - 1; ++i) {
        swapped = false;
        // Last i elements are already in place
        for (size_t j = 0; j < arr.size() - 1 - i; ++j) {
            if (arr[j] > arr[j + 1]) {
                std::swap(arr[j], arr[j + 1]);
                swapped = true;
            }
        }
        // If no swaps occurred, array is sorted
        if (!swapped) {
            break;
        }
    }
}

// Helper to print a vector
void printVector(const std::vector<int>& arr)
{
    for (const int& val : arr) {
        std::cout << val << " ";
    }
    std::cout << '\n';
}

// Example usage and tests
int main()
{
    std::vector<int> data1 = {64, 34, 25, 12, 22, 11, 90};
    std::cout << "Unsorted: ";
    printVector(data1);

    bubbleSort(data1);

    std::cout << "Sorted:   ";
    printVector(data1);

    // Test already-sorted case (should exit early)
    std::vector<int> data2 = {1, 2, 3, 4, 5};
    bubbleSort(data2);

    // Test single element
    std::vector<int> data3 = {42};
    bubbleSort(data3);

    // Test empty
    std::vector<int> data4;
    bubbleSort(data4);

    return 0;
}