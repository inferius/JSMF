namespace JSMF.EventArgs
{
    public enum ConsoleEventArgsType
    {
        ConsoleLog,
        // TODO: Doplnit zbytek, zatim staci rozfungovat jen log
        ConsoleInfo,
        ConsoleDebug,
        ConsoleWarning,
        ConsoleError,
        ConsoleGroup,
        ConsoleGroupCollapsed,
        ConsoleClear,
    }
}