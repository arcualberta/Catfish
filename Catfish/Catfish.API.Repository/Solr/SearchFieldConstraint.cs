﻿namespace Catfish.API.Repository.Solr
{
    public class SearchFieldConstraint
    {
        public enum eScope { Data, Metadata, Other }

        public eScope Scope { get; set; }
        public Guid ContainerId { get; set; }
        public Guid FieldId { get; set; }
        public string SearchText { get; set; }

        public static string ScopeStr(eScope scope)
        {
            switch (scope)
            {
                case eScope.Data:
                    return "data";
                case eScope.Metadata:
                    return "metadata";
                default:
                    throw new Exception("Unknown scope: " + scope.ToString());
            }
        }
        public static eScope Str2Scope(string str)
        {
            if (str == "data")
                return eScope.Data;
            if (str == "metadata")
                return eScope.Metadata;

            return eScope.Other;
        }

    }
}
