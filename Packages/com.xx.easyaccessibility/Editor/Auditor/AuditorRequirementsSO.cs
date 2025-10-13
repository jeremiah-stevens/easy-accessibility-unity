using UnityEngine;
using System.Collections.Generic;
using System;

namespace EasyAccessibility
{
    [CreateAssetMenu(fileName = "AuditorRequirementsSO", menuName = "Scriptable Objects/AuditorRequirementsSO")]
    public class AuditorRequirementsSO : ScriptableObject
    {
        public AccessibilityReport report;
    }
}