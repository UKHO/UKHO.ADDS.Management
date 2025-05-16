using UKHO.ADDS.Mocks.Management.Override.Mocks.sample.Models;
using UKHO.ADDS.Mocks.Markdown;

namespace UKHO.ADDS.Mocks.Management.Override.Mocks.sample
{
    public class ExtraEndpoint : ServiceEndpointMock
    {
        public override void RegisterSingleEndpoint(IEndpointMock endpoint) =>
            endpoint.MapPut("/extra", (HttpRequest request, MockFileModel model) => { return Results.Ok("Here is my extra endpoint"); }).WithEndpointMetadata(endpoint, d => { d.Append(new MarkdownParagraph(new MarkdownStrongEmphasis("Don't ever call this endpoint"))); });
    }
}
