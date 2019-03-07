# Toscana

[![Join the chat at https://gitter.im/QualiSystems/Toscana](https://badges.gitter.im/QualiSystems/Toscana.svg)](https://gitter.im/QualiSystems/Toscana?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

[![Build status](https://ci.appveyor.com/api/projects/status/5f7grtkr9r4ao0er/branch/master?svg=true)](https://ci.appveyor.com/project/johnathanvidu/toscana/branch/master) [![codecov](https://codecov.io/gh/QualiSystems/Toscana/branch/master/graph/badge.svg)](https://codecov.io/gh/QualiSystems/Toscana) [![NuGet version](https://badge.fury.io/nu/Toscana.svg)](https://badge.fury.io/nu/Toscana) 


## What is Toscana?
Toscana, which stands for TOSCA Net Analyzer, is a .NET library for validating, parsing and analyzing TOSCA YAML format.


## What is TOSCA?
TOSCA, which stands for Topology and Orchestration Specification for Cloud Applications, 
is a language in YAML grammar for describing service templates by means of Topology Templates and towards enablement 
of interaction with a TOSCA instance model perhaps by external APIs or plans.  

To learn learn more about OASIS TOSCA visit [OASIS Open Standards channel on YouTube](https://www.youtube.com/playlist?list=PLaYKtNo_BitZXdvyNDwBi290IHxdi459v)

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

There are two main file formats in the TOSCA standard: TOSCA Service Template and TOSCA Cloud Service Archive. 
The first one is in YML, while the last one is a ZIP file.

### TOSCA Service Template 

TOSCA YAML file is represented by _ToscaServiceTemplate_ class in Toscana library. 
Toscana allows parsing a single TOSCA YAML file into an instance of ToscaServiceTemplate class. 
Even if the file depends on other TOSCA YAML files via _imports_ parsing will succeeds.

**Load a TOSCA Service Template file**
```C#
ToscaServiceTemplate toscaServiceTemplate = ToscaServiceTemplate.Load("service-template.yaml");
```

**Create a new TOSCA Service Template and save it to a file**
```C#
ToscaServiceTemplate toscaServiceTemplate = new ToscaServiceTemplate
{
    ToscaDefinitionsVersion = "tosca_simple_yaml_1_0"
};
toscaServiceTemplate.NodeTypes.Add("node_type_name", new ToscaNodeType() );
toscaServiceTemplate.Save("service-template.yaml");
```

### TOSCA Cloud Service Archive 

TOSCA CSAR file is a ZIP compressed file which contains a set of TOSCA YAML files, drivers and icons, that represent cloud environment.
The archive must contain the TOSCA.meta file, whcih points to the TOSCA YAML entry point file. If any of the TOSCA YAML files 
imports another YAML file that is missing in the archive, loading will fail.

**Load a tosca.zip with all the _imports_ included in the archive**

```C#
ToscaCloudServiceArchive toscaCloudServiceArchive = ToscaCloudServiceArchive.Load("cloud-archive.zip");
```

**Load a tosca.zip when some of the _imports_ reside at alternative location**

```C#
ToscaCloudServiceArchive toscaCloudServiceArchive = ToscaCloudServiceArchive.Load("cloud-archive.zip", @"c:\cloud-imports\");
```

**Creating Cloud Service Archive**

```C#
ToscaMetadata toscaMetadata = new ToscaMetadata
{ 
    CsarVersion = new Version(1,0,0), 
    EntryDefinitions = "entry.yaml", 
    ToscaMetaFileVersion = new Version(1,0,0), 
    CreatedBy = "Anonymous" 
};
ToscaServiceTemplate toscaServiceTemplate = new ToscaServiceTemplate
{
    ToscaDefinitionsVersion = "tosca_simple_yaml_1_0"
};
ToscaCloudServiceArchive cloudServiceArchive = new ToscaCloudServiceArchive(toscaMetadata);
cloudServiceArchive.AddToscaServiceTemplate("entry.yaml", toscaServiceTemplate);
```

**Validating Cloud Service Archive**

```C#
List<ValidationResult> results;
if ( !cloudServiceArchive.TryValidate(out results) )
{
    foreach(ValidationResult validationResult in results)
    {
        Console.WriteLine(validationResult.ErrorMessage);
    }
}
```

### Documentation

Toscana documentation is powered by DocFX Documentation Generation Tool for API Reference and Markdown files based on XML documentation comments.

To build the documentation from command-line:

```
> docfx .\toscana.docs\docfx.json
```

To run a web server that hosts Toscana HTML documentation:
```
> docfx serve .\Toscana.Docs\_site
```

### Powershell Module
Toscana is also available as a Powershell module that allows loading and validating Tosca related files.

**Installation**
```PS
PS>  Import-Module Toscana.PS\bin\Debug\Toscana.PS.dll
```

**Loading Cloud Service Archive**

```PS
PS>  Get-ToscaCloudServiceArchive Tosca.zip
```

**Loading Cloud Service Archive with imports at alternative path**

```PS
PS>  Get-ToscaCloudServiceArchive Tosca.zip C:\Imports
```

**Loading Cloud Service Template**

```PS
PS>  Get-ToscaServiceTemplate tosca.yaml
```


### License
The software is released under [Apache License v2.0](LICENSE). 