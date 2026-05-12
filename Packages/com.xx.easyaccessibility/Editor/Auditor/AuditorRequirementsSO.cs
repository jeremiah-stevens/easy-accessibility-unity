using System;
using System.Collections.Generic;
using UnityEngine;

namespace EasyAccessibility
{
    [CreateAssetMenu(
        fileName = "AuditorRequirementsSO",
        menuName = "Scriptable Objects/AuditorRequirementsSO"
    )]
    public class AuditorRequirementsSO : ScriptableObject
    {
        public AccessibilityReport report = new();
    }
}
