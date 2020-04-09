# CIM Asset Parser
[![CircleCI](https://circleci.com/gh/DAXGRID/cim-asset-parser/tree/master.svg?style=shield&circle)](https://circleci.com/gh/DAXGRID/cim-asset-parser/tree/master)
[![MIT](https://img.shields.io/badge/license-MIT-green.svg?style=flat-square)](./LICENSE)

## Configuration
Either rename 'appsettings.example.json' to 'appsettings.json' or make a new
file, after that fill out all the empty fields in the appsettings file.

## Requirements running the application
* gnumake
* dotnet runtime 3.1

### Note
On windows it can be done using chocolatey with the following command:
``` sh
choco install make
```

## Running
Running the application
``` makefile
make start
```


CIM asset parser is a library for parsing the CIM-standard into a format that
can be used for the cim-asset management system.
