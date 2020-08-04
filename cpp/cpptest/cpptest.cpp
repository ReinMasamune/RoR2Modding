#include "pch.h"
#include "cpptest.h"

void cpptest::Class1::Awake()
{
    //this->data[0] = 1;
    //this->data[1] = 5;
    this->Logger->LogWarning( "Hello???" );
    this->Logger->LogWarning( this->data[0] );
}
