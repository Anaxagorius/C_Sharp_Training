#include <iostream>

using namespace std;

int main(){
    int array[] = {10, 40, 30, 20, 50, 15};
    int target = 30 ;

    int length = sizeof(array)/sizeof(array[0]);

    for (int index = 0; index < length; ++index)
    {
        if(array[index] == target){
            cout << "Found at index " << index << endl;
            return 0;
        }
    }

    cout << "Target not found" << endl;
    return 0;
}