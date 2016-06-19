# Toscana

[![build status](http://teamcity.codebetter.com/app/rest/builds/buildType:id:Toscana/statusIcon)](http://teamcity.codebetter.com/viewType.html?buildTypeId=Toscana&guest=1) [![code coverage](https://img.shields.io/teamcity/coverage/Toscana.svg)](http://teamcity.codebetter.com/viewType.html?buildTypeId=Toscana&guest=1) [![NuGet version](https://badge.fury.io/nu/Toscana.svg)](https://badge.fury.io/nu/Toscana) 


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

## Example

### Parse TOSCA Simple Profile

```C#
ToscaSimpleProfile toscanSimpleProfile = new ToscaSimpleProfile.Parse(@"
tosca_definitions_version: tosca_simple_yaml_1_0
description: Template for deploying a single server with predefined properties.")
```

### Combine TOSCA Simple Profiles

```C#
ToscaSimpleProfileBuilder builder = new ToscaSimpleProfileBuilder();
builder.Append(toscanSimpleProfile1);
builder.Append(toscanSimpleProfile2);
ToscaSimpleProfile combinedTosca = builder.Build();

```
### License
The software is released under [Apache License v2.0](LICENSE). 
