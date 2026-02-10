#include <iostream>
#include <vector>
#include <algorithm>
#include <string>
#include <map>
#include <array>
#include <set>

using namespace std;    

void BubbleSortVector(std::vector<int>& vec)
{
    int n = vec.size();
    for (int i = 0; i < n - 1; i++) {
        for (int j = 0; j < n - i - 1; j++) {
            if (vec[j] > vec[j + 1]) {
                std::swap(vec[j], vec[j + 1]);
            }
        }
    }
}

void PrintVector(const std::vector<int>& vec)
{
    for (int num : vec) {
        std::cout << num << " ";
    }
    std::cout << std::endl;
}

int main()
{
    std::vector<int> vec = { 64, 34, 25, 12, 22, 11, 90};
    cout << "Original vector: ";
    PrintVector(vec);

    BubbleSortVector(vec);
    cout << "Sorted vector: ";
    PrintVector(vec);

    return 0;
}
