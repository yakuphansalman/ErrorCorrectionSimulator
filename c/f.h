#include "hammingcode.h"

uint32_t checkBits(uint32_t data_bits, uint8_t data_length){
    uint8_t check_length = checkLength(data_length);
    uint8_t length = check_length + data_length;

    uint32_t check_bits = 0;

    uint64_t expanced_bits = expancedBits(data_bits, data_length, check_bits, check_length);

    for(uint8_t i = 0; i< length; i++){
        if(i == 0 ||isPowerOfTwo(i+1)) continue;
        if(((expanced_bits>> i )&1) == 0) continue;
        if(!check_bits) check_bits+= i+1;
        else{
            check_bits = check_bits^(i+1);
        }
    }
    return check_bits;
}

uint64_t codeword(uint32_t data_bits, uint8_t data_length){
    uint32_t check_bits = checkBits(data_bits, data_length);
    uint8_t check_length = checkLength(data_length);

    return expancedBits(data_bits, data_length, check_bits, check_length);
}
