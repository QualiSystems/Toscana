using System.Collections.Generic;
using System.Linq;
using Toscana.Common;
using Toscana.Domain;
using Toscana.Exceptions;

namespace Toscana
{
    public interface IToscaSimpleProfileBuilder
    {
        IToscaSimpleProfileBuilder Append(ToscaSimpleProfile toscaSimpleProfile);
        ToscaSimpleProfile Build();
    }

    public class ToscaSimpleProfileBuilder : IToscaSimpleProfileBuilder
    {
        private readonly List<ToscaSimpleProfile> toscaSimpleProfiles = new List<ToscaSimpleProfile>();

        public IToscaSimpleProfileBuilder Append(ToscaSimpleProfile toscaSimpleProfile)
        {
            toscaSimpleProfiles.Add(toscaSimpleProfile);
            return this;
        }

        public ToscaSimpleProfile Build()
        {
            var combinedTosca = new ToscaSimpleProfile();
            foreach (var simpleProfile in toscaSimpleProfiles)
            {
                foreach (var nodeType in simpleProfile.NodeTypes)
                {
                    combinedTosca.NodeTypes.Add(nodeType.Key, nodeType.Value);
                }
            }
            foreach (var nodeType in combinedTosca.NodeTypes)
            {
                for (string derivedFrom = nodeType.Value.DerivedFrom; 
                    !string.IsNullOrEmpty(derivedFrom); 
                    derivedFrom = combinedTosca.NodeTypes[derivedFrom].DerivedFrom)
                {
                    if (!combinedTosca.NodeTypes.ContainsKey(derivedFrom))
                    {
                        throw new ToscaValidationException(string.Format("Definition of Node Type {0} is missing", derivedFrom));
                    }

                    foreach (var capability in combinedTosca.NodeTypes[derivedFrom].Capabilities)
                    {
                        nodeType.Value.Capabilities.Add(capability.Key, capability.Value);
                    }
                }
            }
            return combinedTosca;
        }
    }
}