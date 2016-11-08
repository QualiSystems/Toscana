## TOSCA Cloud Service Archive

### TOSCA CSAR file

TOSCA CSAR file is a ZIP compressed file which contains a set of TOSCA YAML files, drivers and icons, that represent cloud environment.
The archive must contain the TOSCA.meta file, whcih points to the TOSCA YAML entry point file. If any of the TOSCA YAML files 
imports another YAML file that is missing in the archive, loading will fail.

<pre>
|-- TOSCA-Metadata
|   +-- TOSCA.meta
+-- entry.yaml
+-- icon.png
+-- driver.zip
</pre>

**TOSCA.meta file structure** 
<pre>
TOSCA-Meta-File-Version: 1.0
CSAR-Version: 1.1
Created-By: Toscana
Entry-Definitions: entry.yaml
</pre>

**Loading a tosca.zip with all the _imports_ included in the archive**

```C#
ToscaCloudServiceArchive toscaCloudServiceArchive = ToscaCloudServiceArchive.Load("tosca.zip");
```

**Loading a tosca.zip when some of the _imports_ reside at alternative location**

```C#
ToscaCloudServiceArchive toscaCloudServiceArchive = ToscaCloudServiceArchive.Load("tosca.zip", @"C:\tosca_imports\");
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