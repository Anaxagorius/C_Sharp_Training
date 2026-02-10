/*

#include <iostream>
#include <vector>
#include <algorithm>
#include <string>
#include <map>
#include <array>
#include <set>

using namespace std;

void BubbleSort(int array[], int n) {
    for (int i = 0; i < n - 1; i++) {
        for (int j = 0; j < n - i - 1; j++) {
            if (array[j] > array[j + 1]) {

                int temp = array[j];
                array[j] = array[j + 1];
                array[j + 1] = temp;
        
            }
        }
    }
}

int main(){

    int array[] = {64, 25, 12, 22, 11};

}

*/

#include <iostream>
using namespace std;
 
void bubbleSort(int array[], int n) {
    // Outer Loop: Traverse through all elements
    for (int i = 0; i < n - 1; i++) {
        // Inner Loop: Compare each element with the next one
        for (int j = 0; j < n -i- 1; j++) {
            // Swap if the current element is greater than the next
            if (array[j] > array[j + 1]) {
                // Swap Elements
                int temp = array[j];
                array[j] = array[j + 1];
                array[j + 1] = temp;
            }
        }
    }
}
 
int main() {
    // Fixed Array:
    int array[] = {64, 25, 12, 22, 11};
    int n = sizeof(array) / sizeof(array[0]);
 
    cout << "OG Array Elements: ";
        for (int i = 0; i < n; i++) {
            cout << array[i] << " ";
        }
    cout << endl;
 
 
    bubbleSort(array, n);
 
    cout << "Updated Array Elements: ";
        for (int i = 0; i < n; i++) {
            cout << array[i] << " ";
        }
    cout << endl;
 
    return 0;
}