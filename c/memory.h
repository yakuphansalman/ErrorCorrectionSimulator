#include "hammingcode.h"

uint64_t corruptedWord(uint64_t codeword, uint8_t length){
    if(!length) return codeword;
    uint8_t bit = (uint8_t)rand()%length;
    codeword = codeword ^((uint64_t)1 << bit);
    return codeword;
}

