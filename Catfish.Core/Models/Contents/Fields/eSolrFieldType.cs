
namespace Catfish.Core.Models.Contents.Fields
{
    public enum eSolrFieldType
    {
        /// <summary>
        /// Single text
        /// </summary>
        _t,

        /// <summary>
        /// Multiple texts
        /// </summary>
        _ts,

        /// <summary>
        /// Single point
        /// </summary>
        _i,

        /// <summary>
        /// Multiple points
        /// </summary>
        _is,
        
        /// <summary>
        /// Single string
        /// </summary>
        _s, 
        
        /// <summary>
        /// Multiple strings
        /// </summary>
        _ss, 
        
        /// <summary>
        /// Single long (plong)
        /// </summary>
        _l, 
        
        /// <summary>
        /// Multiple long (plongs)
        /// </summary>
        _ls, 
        
        /// <summary>
        /// Single boolean
        /// </summary>
        _b, 
        
        /// <summary>
        /// Multiple booleans
        /// </summary>
        _bs, 
        
        /// <summary>
        /// Single float (ploat)
        /// </summary>
        _f, 
        
        /// <summary>
        /// Multiple floats (pfloats)
        /// </summary>
        _fs, 
        
        /// <summary>
        /// Single double (pdouble)
        /// </summary>
        _d, 
        
        /// <summary>
        /// Multiple doubles (pdoubles)
        /// </summary>
        _ds, 
        
        /// <summary>
        /// Single date (pdate)
        /// </summary>
        _dt, 
        
        /// <summary>
        /// Multiple dates (pdates)
        /// </summary>
        _dts, 
        
        /// <summary>
        /// Location
        /// </summary>
        _p,

        /// <summary>
        /// location_rpt
        /// </summary>
        _srpt,

        /// <summary>
        /// Delimited payloads float
        /// </summary>
        _dpf,

        /// <summary>
        /// Delimited payloads int
        /// </summary>
        _dpi,

        /// <summary>
        /// Delimited payloads string
        /// </summary>
        _dps
    }
}
