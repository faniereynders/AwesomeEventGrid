namespace AwesomeEventGrid.Abstractions
{
    public interface ISubscription
    {
        string EndpointUrl { get; set; }
        string[] EventTypes { get; set; }
        string Name { get; set; }
        string SubjectBeginsWith { get; set; }
        string SubjectEndsWith { get; set; }
        string Topic { get; set; }
    }
}