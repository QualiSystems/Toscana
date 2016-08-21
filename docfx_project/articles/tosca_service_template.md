## TOSCA Service Template

A TOSCA Service Template (YAML) document contains element definitions of building blocks for cloud application, 
or complete models of cloud applications. 

From [TOSCA Simple Profile in YAML Version 1.0](http://docs.oasis-open.org/tosca/TOSCA-Simple-Profile-YAML/v1.0/csprd02/TOSCA-Simple-Profile-YAML-v1.0-csprd02.html):
>A Service Template is typically used to specify the "topology" (or structure) and "orchestration" (or invocation of management behavior) of IT services so that they can be provisioned and managed in accordance with constraints and policies. 

>Specifically, TOSCA Service Templates optionally allow definitions of a TOSCA Topology Template, TOSCA types (e.g., Node, Relationship, Capability, Artifact, etc.), groupings, policies and constraints along with any input or output declarations.
 
In Toscana, Service Template is represented by [ToscaServiceTemplate](../api/Toscana.ToscaServiceTemplate.md) class.

Toscana allows parsing YAML from an external file or from a stream.

### Loading TOSCA Service Template from a file

**Make sure to include the required usings**
```C#
using System.IO;
using Toscana;
```

**Parse TOSCA Service Template from a YAML file**
```C#
ToscaServiceTemplate serviceTemplate = ToscaServiceTemplate.Parse("tosca.yaml");
```

### Loading TOSCA Service Template from stream
**Make sure to include the required usings**
```C#
using Toscana;
```

**Parse TOSCA Service Template from a stream**
```C#
ToscaServiceTemplate serviceTemplate = ToscaServiceTemplate.Parse(stream);
```

