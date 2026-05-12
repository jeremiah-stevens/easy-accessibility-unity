using System;
using System.Collections.Generic;

namespace EasyAccessibility
{
    [Serializable]
    public class AccessibilityReport
    {
        public List<AuditorRequirement> requirements = new();
    }
}
