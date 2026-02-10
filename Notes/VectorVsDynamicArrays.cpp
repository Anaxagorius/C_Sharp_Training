#include <iostream>
#include <vector>

/**
 * Expert comparison: Raw dynamic arrays vs. std::vector<int>
 * 
 * In C++, "dynamic array" typically refers to manually allocated arrays using
 * new[] / delete[] (raw pointers). std::vector is the modern, safe dynamic container.
 * 
 * Exact differences – seasoned-programmer breakdown:
 * 
 * 1. Memory Management
 *    - Raw: Manual allocation/deallocation → prone to leaks, double-delete, dangling pointers.
 *    - vector: RAII (Resource Acquisition Is Initialization) – automatic, exception-safe cleanup.
 * 
 * 2. Resizing
 *    - Raw: Fixed size after allocation. Resizing requires manual realloc (new array, copy, delete old).
 *    - vector: Dynamic growth via push_back(), resize(), reserve(). Amortized O(1) push_back.
 * 
 * 3. Size & Capacity Tracking
 *    - Raw: You must track size separately (no built-in size()).
 *    - vector: Built-in size(), capacity(), empty() – always accurate.
 * 
 * 4. Bounds Safety
 *    - Raw: No checking → arr[100] on size 10 = undefined behavior (crash/exploit).
 *    - vector: at() throws std::out_of_range; operator[] is unchecked but safer context.
 * 
 * 5. Features & Integration
 *    - Raw: Bare contiguous memory – minimal overhead, but no extras.
 *    - vector: Iterators, algorithms (std::sort, etc.), swap, emplace_back, shrink_to_fit().
 * 
 * 6. Performance
 *    - Raw: Slightly lower overhead (no internal bookkeeping), but manual ops often negate this.
 *    - vector: Heavily optimized (reserve() eliminates reallocs), comparable or faster in practice.
 * 
 * Expert opinion: ALWAYS prefer std::vector unless you have extreme constraints
 * (e.g., C interop, no STL allowed, or embedded systems with no exceptions).
 * Raw dynamic arrays are error-prone and obsolete in modern C++ (C++11+).
 * The STL container is safer, more expressive, and usually just as fast.
 */

 // Example 1: Raw dynamic array (manual, unsafe)
void raw_dynamic_example()
{
    std::cout << "\n--- Raw Dynamic Array Example ---\n";
    
    int n = 7;
    int* arr = new int[n];  // Manual allocation
    
    // Manual initialization (like your test data)
    int init[] = {64, 34, 25, 12, 22, 11, 90};
    for (int i = 0; i < n; ++i) {
        arr[i] = init[i];
    }
    
    std::cout << "Unsorted: ";
    for (int i = 0; i < n; ++i) {
        std::cout << arr[i] << " ";
    }
    std::cout << "\n";
    
    // (Bubble sort omitted for brevity – would use arr[j] syntax)
    
    delete[] arr;  // MUST remember this – easy to forget/cause leaks
}

// Example 2: std::vector (safe, dynamic)
void vector_example()
{
    std::cout << "\n--- std::vector Example ---\n";
    
    std::vector<int> arr = {64, 34, 25, 12, 22, 11, 90};  // Dynamic from init list
    
    // Can grow dynamically
    arr.push_back(42);  // Now size 8 – impossible with raw without manual realloc
    
    std::cout << "Unsorted (with extra element): ";
    for (const int& val : arr) {
        std::cout << val << " ";
    }
    std::cout << "\n";
    
    // Automatic cleanup – no delete needed
}

int main()
{
    raw_dynamic_example();
    vector_example();
    
    // Optimization tip: For known size, reserve() upfront
    std::vector<int> optimized;
    optimized.reserve(1000);  // Prevents reallocations during push_back loop
    
    return 0;
}