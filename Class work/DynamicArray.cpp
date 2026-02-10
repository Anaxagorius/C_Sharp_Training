#include <iostream>
#include <vector>
#include <utility>  // for std::swap

using namespace std;

/**
 * Optimized Bubble Sort (in-place) – unchanged from previous version.
 * Works perfectly with std::vector, which is C++'s safe, dynamic array.
 */
void bubble_sort(vector<int>& arr)
{
    size_t n = arr.size();
    if (n <= 1) {
        return;
    }

    bool swapped;
    for (size_t i = 0; i < n - 1; ++i) {
        swapped = false;
        for (size_t j = 0; j < n - 1 - i; ++j) {
            if (arr[j] > arr[j + 1]) {
                std::swap(arr[j], arr[j + 1]);
                swapped = true;
            }
        }
        if (!swapped) {
            break;
        }
    }
}

/**
 * Fully dynamic version: array size and contents determined at runtime.
 * 
 * Expert rationale:
 * - std::vector<int> IS the modern C++ dynamic array – it grows/shrinks automatically,
 *   bounds-checked in debug, exception-safe, and integrates seamlessly with algorithms.
 * - We reserve() upfront for O(1) amortized push_back and to avoid reallocations.
 * - Raw dynamic arrays (new[]/delete[]) are error-prone (leaks, dangling pointers,
 *   manual sizing). Only use them when interfacing with C APIs or extreme constraints.
 * - This version reads size + elements from stdin → truly dynamic.
 */
int main()
{
    size_t n;
    cout << "Enter the number of elements: ";
    if (!(cin >> n)) {
        cerr << "Invalid input for size.\n";
        return 1;
    }

    vector<int> arr;
    arr.reserve(n);  // Optimization: pre-allocate to prevent reallocations

    cout << "Enter " << n << " integers (space-separated): ";
    for (size_t i = 0; i < n; ++i) {
        int value;
        if (!(cin >> value)) {
            cerr << "Invalid input at element " << i + 1 << ".\n";
            return 1;
        }
        arr.push_back(value);
    }

    cout << "\nUnsorted array: ";
    for (const int& val : arr) {
        cout << val << " ";
    }
    cout << "\n";

    bubble_sort(arr);

    cout << "Sorted array:   ";
    for (const int& val : arr) {
        cout << val << " ";
    }
    cout << "\n";

    return 0;
}