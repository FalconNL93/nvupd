using System.CommandLine;
using System.CommandLine.Binding;

namespace Nvupd.Cli.Models;

public class CliOptions
{
    public bool RunAsService { get; set; }
    public bool ForceUpdate { get; set; }
    public bool Verbose { get; set; }
}

public class CliOptionsBinder : BinderBase<CliOptions>
{
    private readonly Option<bool> _runAsService;
    private readonly Option<bool> _forceUpdate;
    private readonly Option<bool> _verbose;

    public CliOptionsBinder(Option<bool> runAsService, Option<bool> forceUpdate, Option<bool> verbose)
    {
        _runAsService = runAsService;
        _forceUpdate = forceUpdate;
        _verbose = verbose;
    }

    protected override CliOptions GetBoundValue(BindingContext bindingContext) => new()
    {
        RunAsService = bindingContext.ParseResult.GetValueForOption(_runAsService),
        ForceUpdate = bindingContext.ParseResult.GetValueForOption(_forceUpdate),
        Verbose = bindingContext.ParseResult.GetValueForOption(_verbose)
    };
}