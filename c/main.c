#include<stdio.h>
#include<stdint.h>
#include<stdlib.h>

#include "pipeline.h"


int main(int argc, char *argv[]){
    uint32_t data = atoi(argv[1]);
    uint8_t length = atoi(argv[2]);

    run(data, length);

}
