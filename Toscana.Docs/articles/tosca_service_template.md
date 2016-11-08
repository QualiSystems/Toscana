## TOSCA Service Template

A TOSCA Service Template (YAML) document contains element definitions of building blocks for cloud application, 
or complete models of cloud applications. 

From [TOSCA Simple Profile in YAML Version 1.0](http://docs.oasis-open.org/tosca/TOSCA-Simple-Profile-YAML/v1.0/csprd02/TOSCA-Simple-Profile-YAML-v1.0-csprd02.html):
>A Service Template is typically used to specify the "topology" (or structure) and "orchestration" (or invocation of management behavior) of IT services so that they can be provisioned and managed in accordance with constraints and policies. 

>Specifically, TOSCA Service Templates optionally allow definitions of a TOSCA Topology Template, TOSCA types (e.g., Node, Relationship, Capability, Artifact, etc.), groupings, policies and constraints along with any input or output declarations.
 
In Toscana, Service Template is represented by **ToscaServiceTemplate** class.

TOSCA YAML file is represented by _ToscaServiceTemplate_ class in Toscana library. 
Toscana allows loading a single TOSCA YAML file into an instance of ToscaServiceTemplate class. 
Only the content of the YAML file is loaded without other files referenced by imports.

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

