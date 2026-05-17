#pragma once

#include <stdint.h>
#include <stdlib.h>

uint8_t isPowerOfTwo(uint32_t n){
    return (n>0)&&((n&(n-1)) == 0);
}

uint8_t checkLength(uint8_t data_length){
    uint8_t k = 0;
    while (data_length > (1 << k)-1-k){
        k++;
    }
    return k;
}
uint64_t expancedBits(uint32_t data_bits, uint8_t data_length, uint32_t check_bits, uint8_t check_length){
    uint8_t data_i = 0; uint8_t check_i = 0;
    uint8_t i = 0;
    uint8_t length = data_length + check_length;
    uint64_t expanced_bits = 0;
    while(i < length){
        if(i == 0 || isPowerOfTwo(i+1)){
            expanced_bits = expanced_bits | ((check_bits >> check_i) &1) << i;
            check_i++;
        }
        else{
            expanced_bits = expanced_bits | ((data_bits >> data_i ) &1) << i;
            data_i++;
        }
        i++;
    }
    return expanced_bits;
}




uint32_t decode_data(uint64_t codeword, uint8_t length){
    uint32_t data = 0;
    uint8_t i = 0;
    uint8_t data_i = 0;
    while(i < length){
        if(i==0 || isPowerOfTwo(i+1)){
            i++; continue;
        }
        data = (data & ~(1 << data_i)) | (((codeword >> i) & 1) << data_i);
        data_i++;
        i++;
    }
    return data;
}
uint16_t decode_check(uint64_t codeword, uint8_t length){
    uint16_t check = 0;
    uint8_t i = 0;
    uint8_t check_i = 0;
    while(i < length){
        if(i== 0 || isPowerOfTwo(i+1)){
            check = (check & ~(1 << check_i)) | (((codeword >> i) & 1) << check_i);
            check_i++;
        }
        i++;
    }
    return check;
}

void print_bits(uint64_t word, uint8_t length){
    for(uint8_t i = length -1; i<= length -1; i--){
        printf("%d\t", ((word >> i)&1));
    }
    printf("\n");
}



