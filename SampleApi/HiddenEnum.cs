using SampleApi.Attributes;

namespace SampleApi
{
    // If you ignore an entire model or enum like this, make sure that also use the attribute in all places where it is used

    [SwaggerIgnore]
    public enum HiddenEnum
    {
        Some,
        Thing,
        Goes,
        Here
    }
}