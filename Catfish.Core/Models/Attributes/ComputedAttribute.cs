using System;
using System.Collections.Generic;
using System.Text;

namespace Catfish.Core.Models.Attributes
{
    /// <summary>
    /// Computed attribute: indicates that the property should always be saved
    /// to the database irrespective of whether or not it was explicitely modified.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public class ComputedAttribute: Attribute
    {
    }
}
