
#include <iostream>

constexpr int CAPACITY = 10;

// Utility to print current elements
void printArray(const int arr[], int size, const char* label = "Array") {
    std::cout << label << " (size=" << size << "): [";
    for (int i = 0; i < size; ++i) {
        std::cout << arr[i];
        if (i + 1 < size) std::cout << ", ";
    }
    std::cout << "]\n";
}

/*
 * Insert Operation:
 * Inserts 'value' at 'index' without replacing existing elements.
 * Shifts elements to the right from index..size-1.
 * Returns true on success, false if capacity/full or index invalid.
 */
bool insertAt(int arr[], int& size, int index, int value) {
    if (size >= CAPACITY) return false;          // no space left
    if (index < 0 || index > size) return false; // index must be within [0, size]

    // Shift right: start from the end to avoid overwriting
    for (int i = size; i > index; --i) {
        arr[i] = arr[i - 1];
    }
    arr[index] = value;
    ++size;
    return true;
}

/*
 * Delete Operation:
 * Deletes element at 'index'.
 * Shifts elements left from index+1..size-1.
 * Returns true on success, false if empty or index invalid.
 */
bool deleteAt(int arr[], int& size, int index) {
    if (size <= 0) return false;                 // nothing to delete
    if (index < 0 || index >= size) return false;// index must be within [0, size-1]

    // Shift left to fill the gap
    for (int i = index; i < size - 1; ++i) {
        arr[i] = arr[i + 1];
    }
    --size;
    return true;
}

int main() {
    // Initial fixed-size array and logical size
    int arr[CAPACITY] = {1, 2, 3, 4, 5}; // rest are unspecified; only size matters
    int size = 5;

    printArray(arr, size, "Initial");

    // Demonstrations
    // 1) Insert 99 at index 2: result should be [1, 2, 99, 3, 4, 5]
    if (insertAt(arr, size, 2, 99)) {
        printArray(arr, size, "After insert 99 at index 2");
    } else {
        std::cout << "Insert failed (full or bad index)\n";
    }

    // 2) Delete at index 4 (which is value 4 in the original sequence):
    //    result should be [1, 2, 99, 3, 5]
    if (deleteAt(arr, size, 4)) {
        printArray(arr, size, "After delete at index 4");
    } else {
        std::cout << "Delete failed (empty or bad index)\n";
    }

    // 3) Example edge cases:
    // Insert at beginning
    insertAt(arr, size, 0, -1);
    printArray(arr, size, "After insert -1 at index 0");

    // Insert at end (append)
    insertAt(arr, size, size, 777);
    printArray(arr, size, "After append 777");

    // Delete first
    deleteAt(arr, size, 0);
    printArray(arr, size, "After delete index 0");

    // Delete last
    deleteAt(arr, size, size - 1);
    printArray(arr, size, "After delete last index");

    return 0;
}
