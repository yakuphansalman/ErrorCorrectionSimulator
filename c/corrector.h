#include "hammingcode.h"

uint32_t correctedWord(uint64_t codeword,uint8_t length, uint32_t syndrome){
    if(syndrome == 1 ||isPowerOfTwo(syndrome)) return decode_data(codeword, length);
    codeword = codeword ^((uint64_t)1 << (syndrome - 1));
    return decode_data(codeword, length);
}
