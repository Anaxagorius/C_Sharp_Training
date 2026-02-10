#include <array>
#include <iostream>

class FixedArray {
public:
    std::array<int, 5> arr;
    int count;

    FixedArray() {
        arr[0] = 1;
        arr[1] = 2;
        arr[2] = 3;
        arr[3] = 4;
        arr[4] = 5;

        count = 5;
    }

    void addToBeginning(int val) {
        if (count == arr.size()) {
            // Array is full, cannot add more elements
            return;
        }

        for (int i = count; i > 0; --i) {
            arr[i] = arr[i - 1];
        }
        arr[0] = val;
        ++count;
    }

    void addElement(int val) {
        if (count < arr.size()) {
            arr[count] = val;
            ++count;
        }
    }

    void traverse() {
        if (count == 0) {
            std::cout << "Array is empty." << std::endl;
            return;
        }

        std::cout << "Array elements: ";
        for (int i = 0; i < count; ++i) {
            std::cout << arr[i] << " ";
        }
        std::cout << std::endl;
    }
};

int main() {
    FixedArray myArray;

    myArray.addToBeginning(10);

    std::cout << "Array after addToBeginning: ";
    for (int i = 0; i < myArray.count; ++i) {
        std::cout << myArray.arr[i] << " ";
    }
    std::cout << std::endl;

    return 0;
}