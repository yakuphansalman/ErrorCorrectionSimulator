#include<time.h>

#include "f.h"
#include "memory.h"
#include "compare.h"
#include "corrector.h"
#include "hammingcode.h"

static int isSeeded = 0;

__declspec(dllexport) uint32_t run(uint32_t data, uint8_t data_length,
                                   uint64_t* out_memory_in,uint64_t* out_memory_out,
                                   uint32_t* out_data_old, uint32_t* out_data_new,
                                   uint32_t* out_f_old, uint32_t* out_f_new,
                                   uint32_t* out_syndrome, uint32_t* out_total_length){
    if(!isSeeded){
        srand(time(NULL));
        isSeeded = 1;
    }

    uint8_t check_length = checkLength(data_length);
    uint8_t length = data_length + check_length;

    uint64_t memory_in = codeword(data, data_length);
    printf("DATA MEMORY IN\n");
    print_bits(memory_in, length);
    uint64_t memory_out = corruptedWord(memory_in, length);
    printf("DATA MEMORY OUT\n");
    print_bits(memory_out, length);

    uint32_t data_old = decode_data(memory_in, length);
    printf("DATA OLD\n");
    print_bits(data_old, data_length);
    uint32_t data_new = decode_data(memory_out, length);
    printf("DATA NEW\n");
    print_bits(data_new, data_length);

    uint32_t f_old = decode_check(memory_out, length);
    printf("F OLD\n");
    print_bits(f_old, check_length);
    uint32_t f_new = checkBits(data_new, data_length);
    printf("F NEW\n");
    print_bits(f_new, check_length);

    uint32_t syndrome = findCorrupted(f_old, f_new);
    printf("SYNDROME\n");
    print_bits(syndrome, check_length);

    if(!syndrome) printf("NO ERRORS\n");
    else printf("ERROR AT: %d\n", syndrome);

    uint32_t correctWord =  correctedWord(memory_out, length, syndrome);
    printf("DATA OUT\n");
    print_bits(correctWord, data_length);

    *out_memory_in = memory_in;
    *out_memory_out = memory_out;
    *out_data_new = data_new;
    *out_data_old = data_old;
    *out_f_new = f_new;
    *out_f_old = f_old;
    *out_syndrome = syndrome;
    *out_total_length = length;

    return correctWord;
}

