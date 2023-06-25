public class AuthorizationPipe : IPipe<Message>
{
    private static Dictionary<string, Permissions> _permissions = new(
        new[] {
            KeyValuePair.Create("YTE5Nzk0ZDYtMGJiYy00MjIzLTlhYzUtYjJmMzViNGNkOWZh", new Permissions { CanImportFile = true, CanProcessFile = true})
        }, StringComparer.InvariantCultureIgnoreCase
    );

    private static string[] _possibleActions = new[] {
        "ImportFile", "ProcessFile", "UpdateDB"
    };

    public (Message, PipelineState) process(Message input)
    {
        // If no valid action has been given, stop pipeline with an error
        if (input.Action == null || !_possibleActions.Contains(input.Action, StringComparer.InvariantCultureIgnoreCase))
        {
            return (input, PipelineState.WithError("Invalid action"));
        }

        // If no valid token has been given, stop pipeline with an error
        if (input.Token == null || !_permissions.ContainsKey(input.Token))
        {
            return (input, PipelineState.WithError("Unauthorized"));
        }

        var permissions = _permissions[input.Token];

        // If the user does not have permissions for the action, stop the pipeline with an error
        switch (input.Action.ToLower())
        {
            case "importfile":
                return permissions.CanImportFile ? (input, PipelineState.Ok) : (input, PipelineState.WithError("No permission to import files."));
            case "processfile":
                return permissions.CanProcessFile ? (input, PipelineState.Ok) : (input, PipelineState.WithError("No permission to process files."));
            case "updatedb":
                return permissions.CanUpdateDB ? (input, PipelineState.Ok) : (input, PipelineState.WithError("No permission to update DB."));
        }

        // All cases are covered above, if we get here something has gone terribly wrong
        return (input, PipelineState.WithError("Unhandled state in AuthorixationPipe"));
    }

    private record Permissions {
        public bool CanImportFile { get; init; } = false;
        public bool CanProcessFile { get; init; } = false;
        public bool CanUpdateDB { get; init; } = false;
    }
}