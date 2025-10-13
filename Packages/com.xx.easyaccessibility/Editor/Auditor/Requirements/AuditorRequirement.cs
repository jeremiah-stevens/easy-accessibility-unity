using System;
using System.Collections.Generic;

namespace EasyAccessibility
{
    [Serializable]
    public class AuditorRequirement
    {
        public string name;
        public string source;
        public string description;
        public string referenceLink;
        public Status status = Status.None;
        public List<Issue> issues = new();




        public virtual void Audit() { }




        public enum Status
        {
            None = 0,
            Pass = 1,
            Unsure = 2,
            Fail = 3,
        }




        [Serializable]
        public class Issue
        {
            public UnityEngine.Object asset;
            public string issue;
        }
    }
}