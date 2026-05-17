#include "hammingcode.h"


uint32_t findCorrupted(uint32_t check0, uint32_t check1){
    return check0^check1;
}

