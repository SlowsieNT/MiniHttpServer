# MiniHttpServer
Simple http server

## Source code?
Yes, ANYONE is ALLOWED to do ANYTHING.
It is OPTIONAL to leave credits.

## Command Line
`minihttp.exe settings.json`

## JSON Structure
```cs
public class JServerSettings {
    public string[] ServerPrefixes;
    public string ServerPath;
    public bool ServerAllowIndexOf;
    public bool ServerLog;
    public int ServerBufferSize;
    public string[] DefaultPages;
    public string[] MimeTypes;
    public object[][] PathRules;
    public string IndexOfDirLine;
    public string IndexOfFileLine;
    public string IndexOfFileHeader;
    public string IndexOfFileFooter;
    public string IndexOfNeedle1;
}
```
