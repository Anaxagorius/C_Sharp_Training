#include <iostream>
#include <vector>
#include <utility>  

using namespace std;   



/*void bubble_sort(vector<int>& arr)
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

int main()
{
    // Primary test case (matches your example)
    vector<int> data1 = {64, 34, 25, 12, 22, 11, 90};
    cout << "Unsorted: ";
    for (const int& val : data1) cout << val << " ";
    cout << '\n';

    bubble_sort(data1);

    cout << "Sorted:   ";
    for (const int& val : data1) cout << val << " ";
    cout << "\n\n";

    // Additional tests to verify edge cases and early-exit
    vector<int> data2 = {1, 2, 3, 4, 5};          // Already sorted → O(n)
    bubble_sort(data2);
    cout << "Already sorted test: ";
    for (const int& val : data2) cout << val << " ";
    cout << "\n";

    vector<int> data3 = {42};                    // Single element
    bubble_sort(data3);
    cout << "Single element test: ";
    for (const int& val : data3) cout << val << " ";
    cout << "\n";

    vector<int> data4;                           // Empty
    bubble_sort(data4);
    cout << "Empty vector test: " << (data4.empty() ? "empty" : "not empty") << "\n";

    return 0;
}*/

int main() {
    int myarr[2];

    myarr[0] = 12;
    myarr[1] = 10;

    if(myarr[0] > myarr[1]){
        int temp = myarr[0];
        myarr[0] = myarr[1];
        myarr[1] = temp;
    }

    cout << myarr[0] << " " << myarr[1] << endl;

    return 0;
}