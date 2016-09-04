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

### TOSCA CSAR file

TOSCA CSAR file is a ZIP compressed file which contains a set of TOSCA YAML files, drivers and icons, that represent cloud environment.
The archive must contain the TOSCA.meta file, whcih points to the TOSCA YAML entry point file. If any of the TOSCA YAML files 
imports another YAML file that is missing in the archive, loading will fail.

**Loading a tosca.zip with all the _imports_ included in the archive**

```C#
ToscaCloudServiceArchive toscaCloudServiceArchive = ToscaCloudServiceArchive.Load("tosca.zip");
```

**Loading a tosca.zip when some of the _imports_ reside at alternative location**

```C#
ToscaCloudServiceArchive toscaCloudServiceArchive = ToscaCloudServiceArchive.Load("tosca.zip", @"C:\tosca_imports\");
```

### License
The software is released under Apache License v2.0. 
