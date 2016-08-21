# Toscana

## What is Toscana?
Toscana, which stands for TOSCA Net Analyzer, is a .NET library for validating, parsing and analyzing TOSCA YAML format.


## What is TOSCA?
TOSCA, which stands for Topology and Orchestration Specification for Cloud Applications, 
is a language in YAML grammar for describing service templates by means of Topology Templates and towards enablement 
of interaction with a TOSCA instance model perhaps by external APIs or plans.  

## What is YAML?
YAML, which stands for "YAML Ain't Markup Language", is described as "a human friendly data serialization 
standard for all programming languages". Like XML, it allows to represent about any kind of data in a portable, 
platform-independent format. Unlike XML, it is "human friendly", which means that it is easy for a human to read 
or produce a valid YAML document.

## Getting started
Install Toscana nuget package from from Manage Nuget packages

Or from Nuget Package Manager Console: 

```Batch
> install-package Toscana
```

## Usage

### TOSCA YAML file

TOSCA YAML file is represented by _ToscaServiceTemplate_ class in Toscana library. 
Toscana allows parsing a single TOSCA YAML file into an instance of ToscaServiceTemplate class. 
Even if the file depends on other TOSCA YAML files via _imports_ parsing will succeeds.

**Parse a tosca.yaml**
```C#
using (Stream stream = File.Open("tosca.yaml", FileMode.Open))
{
    ToscaServiceTemplate toscaServiceTemplate = ToscaServiceTemplate.Parse(stream);
}
```



### License
The software is released under [Apache License v2.0](LICENSE). 
