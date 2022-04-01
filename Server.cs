using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.IO;
using System.Threading;
using System.Text.RegularExpressions;

namespace minihttp
{
    // https://github.com/SlowsieNT/ctrl-cv
    public class JSON
    { // System.Web.Extensions.dll
        public static System.Web.Script.Serialization.JavaScriptSerializer m_JSS = new System.Web.Script.Serialization.JavaScriptSerializer();
        public static T Parse<T>(string aString)
        {
            try { return m_JSS.Deserialize<T>(aString); } catch { return default; }
        }
        public static string ToString<T>(T aObject)
        {
            try { return m_JSS.Serialize(aObject); } catch { return ""; }
        }
    }
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
    internal class Server {
        HttpListener m_Listener;
        internal string[] ServerPrefixes = new string[] {
            "http://localhost:8081/",
            "http://127.0.0.1:8081/"
        };
        internal string ServerPath = "www";
        internal string ServerTemplates = "tpl";
        internal bool ServerAllowIndexOf = true;
        internal bool ServerLog = true;
        public int ServerBufferSize = 255;
        internal string IndexOfDirLine = "";
        internal string IndexOfFileLine = "";
        internal string IndexOfFileHeader = "tpl/indexof.html";
        internal string IndexOfFileFooter = "tpl/indexof-footer.html";
        internal string IndexOfNeedle1 = "%dirsfiles%";
        internal object[][] PathRules;
        internal string[] DefaultPages = new string[] {
            "index.htm", "index.html", "default.htm", "default.html"
        };
        internal string[] MimeTypes = new string[] {
            "application/postscript","ai","audio/x-aiff","aif","audio/x-aiff","aifc","audio/x-aiff","aiff","text/plain","asc","application/atom+xml","atom","audio/basic","au","video/x-msvideo","avi","application/x-bcpio","bcpio","application/octet-stream","bin","image/bmp","bmp","application/x-netcdf","cdf","image/cgm","cgm","application/octet-stream","class","application/x-cpio","cpio","application/mac-compactpro","cpt","application/x-csh","csh","text/css","css","application/x-director","dcr","video/x-dv","dif","application/x-director","dir","image/vnd.djvu","djv","image/vnd.djvu","djvu","application/octet-stream","dll","application/octet-stream","dmg","application/octet-stream","dms","application/msword","doc","application/xml-dtd","dtd","video/x-dv","dv","application/x-dvi","dvi","application/x-director","dxr","application/postscript","eps","text/x-setext","etx","application/octet-stream","exe","application/andrew-inset","ez","image/gif","gif","application/srgs","gram","application/srgs+xml","grxml","application/x-gtar","gtar","application/x-hdf","hdf","application/mac-binhex40","hqx","text/html","htm","text/html","html","x-conference/x-cooltalk","ice","image/x-icon","ico","text/calendar","ics","image/ief","ief","text/calendar","ifb","model/iges","iges","model/iges","igs","application/x-java-jnlp-file","jnlp","image/jp2","jp2","image/jpeg","jpe","image/jpeg","jpeg","image/jpeg","jpg","application/x-javascript","js","audio/midi","kar","application/x-latex","latex","application/octet-stream","lha","application/octet-stream","lzh","audio/x-mpegurl","m3u","audio/mp4a-latm","m4a","audio/mp4a-latm","m4b","audio/mp4a-latm","m4p","video/vnd.mpegurl","m4u","video/x-m4v","m4v","image/x-macpaint","mac","application/x-troff-man","man","application/mathml+xml","mathml","application/x-troff-me","me","model/mesh","mesh","audio/midi","mid","audio/midi","midi","application/vnd.mif","mif","video/quicktime","mov","video/x-sgi-movie","movie","audio/mpeg","mp2","audio/mpeg","mp3","video/mp4","mp4","video/mpeg","mpe","video/mpeg","mpeg","video/mpeg","mpg","audio/mpeg","mpga","application/x-troff-ms","ms","model/mesh","msh","video/vnd.mpegurl","mxu","application/x-netcdf","nc","application/oda","oda","application/ogg","ogg","image/x-portable-bitmap","pbm","image/pict","pct","chemical/x-pdb","pdb","application/pdf","pdf","image/x-portable-graymap","pgm","application/x-chess-pgn","pgn","image/pict","pic","image/pict","pict","image/png","png","image/x-portable-anymap","pnm","image/x-macpaint","pnt","image/x-macpaint","pntg","image/x-portable-pixmap","ppm","application/vnd.ms-powerpoint","ppt","application/postscript","ps","video/quicktime","qt","image/x-quicktime","qti","image/x-quicktime","qtif","audio/x-pn-realaudio","ra","audio/x-pn-realaudio","ram","image/x-cmu-raster","ras","application/rdf+xml","rdf","image/x-rgb","rgb","application/vnd.rn-realmedia","rm","application/x-troff","roff","text/rtf","rtf","text/richtext","rtx","text/sgml","sgm","text/sgml","sgml","application/x-sh","sh","application/x-shar","shar","model/mesh","silo","application/x-stuffit","sit","application/x-koan","skd","application/x-koan","skm","application/x-koan","skp","application/x-koan","skt","application/smil","smi","application/smil","smil","audio/basic","snd","application/octet-stream","so","application/x-futuresplash","spl","application/x-wais-source","src","application/x-sv4cpio","sv4cpio","application/x-sv4crc","sv4crc","image/svg+xml","svg","application/x-shockwave-flash","swf","application/x-troff","t","application/x-tar","tar","application/x-tcl","tcl","application/x-tex","tex","application/x-texinfo","texi","application/x-texinfo","texinfo","image/tiff","tif","image/tiff","tiff","application/x-troff","tr","text/tab-separated-values","tsv","text/plain","txt","application/x-ustar","ustar","application/x-cdlink","vcd","model/vrml","vrml","application/voicexml+xml","vxml","audio/x-wav","wav","image/vnd.wap.wbmp","wbmp","application/vnd.wap.wbxml","wbmxl","text/vnd.wap.wml","wml","application/vnd.wap.wmlc","wmlc","text/vnd.wap.wmlscript","wmls","application/vnd.wap.wmlscriptc","wmlsc","model/vrml","wrl","image/x-xbitmap","xbm","application/xhtml+xml","xht","application/xhtml+xml","xhtml","application/vnd.ms-excel","xls","application/xml","xml","image/x-xpixmap","xpm","application/xml","xsl","application/xslt+xml","xslt","application/vnd.mozilla.xul+xml","xul","image/x-xwindowdump","xwd","chemical/x-xyz","xyz","application/zip","zi",""
        };
        internal Server(string aServerSettingsFile="") {
            if ("" != aServerSettingsFile && File.Exists(aServerSettingsFile)) {
                var vSettings = JSON.Parse<JServerSettings>(File.ReadAllText(aServerSettingsFile));
                if (null != vSettings)
                {
                    ServerPrefixes = vSettings.ServerPrefixes;
                    if (null != vSettings.ServerPath)
                        ServerPath = vSettings.ServerPath;
                    if (null != vSettings.DefaultPages)
                        DefaultPages = vSettings.DefaultPages;
                    if (null != vSettings.MimeTypes)
                        MimeTypes = vSettings.MimeTypes;
                    if (null != vSettings.PathRules)
                        PathRules = vSettings.PathRules;
                    if (default != vSettings.ServerBufferSize && vSettings.ServerBufferSize > 32 && vSettings.ServerBufferSize < 64 * 1024 * 1024)
                        ServerBufferSize = vSettings.ServerBufferSize;
                    // Index Of
                    if (default != vSettings.IndexOfDirLine && vSettings.IndexOfDirLine.Length > 0)
                        IndexOfDirLine = vSettings.IndexOfDirLine;
                    if (default != vSettings.IndexOfFileLine && vSettings.IndexOfFileLine.Length > 0)
                        IndexOfFileLine = vSettings.IndexOfFileLine;
                    if (default != vSettings.IndexOfFileHeader && vSettings.IndexOfFileHeader.Length > 0)
                        IndexOfFileHeader = vSettings.IndexOfFileHeader;
                    if (default != vSettings.IndexOfFileFooter && vSettings.IndexOfFileFooter.Length > 0)
                        IndexOfFileFooter = vSettings.IndexOfFileFooter;
                    if (default != vSettings.IndexOfNeedle1 && vSettings.IndexOfNeedle1.Length > 0)
                        IndexOfNeedle1 = vSettings.IndexOfNeedle1;
                    // Etc
                    ServerAllowIndexOf = vSettings.ServerAllowIndexOf;
                    ServerLog = vSettings.ServerLog;
                }
                else Console.WriteLine("Invalid settings");
            }
            DoRun();
        }
        byte[] GetBytes(string aString, Encoding aEncoding = null) {
            if (null == aEncoding) aEncoding = Encoding.UTF8;
            return aEncoding.GetBytes(aString);
        }
        void WriteText(HttpListenerContext aHLC, string aText, Encoding aEncoding = null) {
            if (null == aEncoding) aEncoding = Encoding.UTF8;
            var vBytes = GetBytes(aText, aEncoding);
            aHLC.Response.OutputStream.Write(vBytes, 0, vBytes.Length);
        }
        object GetByIndex(int aIndex, object[] aObject, object aDefault) {
            return aIndex < aObject.Length ? aObject[aIndex] : aDefault;
        }
        void DoRun() {
            m_Listener = new HttpListener();
            foreach (var vItem in ServerPrefixes) {
                if (ServerLog)
                    Console.WriteLine(vItem);
                m_Listener.Prefixes.Add(vItem);
            }
            m_Listener.Start();
            while (true) {
                var vCtx = m_Listener.GetContext();
                ThreadPool.QueueUserWorkItem(delegate (object aState) {
                    var vAPath = vCtx.Request.Url.AbsolutePath;
                    var vPath = ServerPath + vAPath;
                    string vClientIP = vCtx.Request.RemoteEndPoint.Address.ToString();
                    bool vRespCodeAppliedByRule = false;
                    bool vCanProceedByRule = true;
                    foreach (var vItem in PathRules) {
                        // [ip_regex, path_regex, return_code, can_read, error_file]
                        bool vIPMatches = Regex.IsMatch(vClientIP, "" + vItem[0]),
                             vPathMatches = Regex.IsMatch(vAPath, "" + vItem[1]);
                        if (vIPMatches && vPathMatches) {
                            string vRCodeStr = "" + GetByIndex(2, vItem, "");
                            string vCanProceedStr = "" + GetByIndex(3, vItem, "");
                            string vResponseFileStr = "" + GetByIndex(4, vItem, "");
                            int.TryParse(vRCodeStr, out int vCode);
                            bool.TryParse(vCanProceedStr, out vCanProceedByRule);
                            if ("0" == vCanProceedStr) vCanProceedByRule = false;
                            if (0 != vCode) {
                                vRespCodeAppliedByRule = true;
                                vCtx.Response.StatusCode = vCode;
                            }
                            if (!vCanProceedByRule) {
                                vRespCodeAppliedByRule = true;
                                if (!vRespCodeAppliedByRule) vCtx.Response.StatusCode = 403;
                                bool vExistsFlag = File.Exists(vResponseFileStr);
                                if (vExistsFlag)
                                    WriteText(vCtx, File.ReadAllText(vResponseFileStr));
                                else WriteText(vCtx, "403");
                                vCtx.Response.Close();
                            }
                        }
                    }
                    if (!vCanProceedByRule) return;
                    if (!vRespCodeAppliedByRule)
                        vCtx.Response.StatusCode = 404;
                    FileInfo vFI = null;
                    bool vIsFile = false, vIsFound = false;
                    if (File.Exists(vPath) || Directory.Exists(vPath)) {
                        vIsFound = true;
                        if (vIsFile = File.Exists(vPath)) {
                            if (File.Exists(vPath)) {
                                vCtx.Response.StatusCode = 200;
                                vFI = new FileInfo(vPath);
                            }
                        } else {
                            foreach (var vPage in DefaultPages) {
                                string vPathFile = vPath + "/" + vPage;
                                if (File.Exists(vPathFile)) {
                                    vCtx.Response.StatusCode = 200;
                                    vFI = new FileInfo(vPathFile);
                                    break;
                                }
                            }
                        }
                    }
                    if (null != vFI) {
                        vCtx.Response.ContentLength64 = vFI.Length;
                        // Handle MimeType
                        int vMimePos = 0 < vFI.Extension.Length ? Array.IndexOf(MimeTypes, vFI.Extension.Substring(1)) : -1;
                        string vMime = "text/plain";
                        if (-1 != vMimePos)
                            vMime = MimeTypes[-1 + vMimePos];
                        vCtx.Response.ContentType = vMime;
                        // Write logs if defined
                        if (ServerLog)
                            Console.WriteLine(vClientIP + "; " + vCtx.Request.HttpMethod + " [" + vMime + "] " + vAPath);
                        // Write to stream
                        FileStream vFS = new FileStream(vFI.FullName, FileMode.Open, FileAccess.Read);
                        byte[] aBuffer = new byte[ServerBufferSize];
                        int vDataLen = 0;
                        while (0 != (vDataLen = vFS.Read(aBuffer, 0, aBuffer.Length)))
                            vCtx.Response.OutputStream.Write(aBuffer, 0, vDataLen);
                        vFS.Close();
                    } else {
                        // Write logs if defined
                        if (ServerLog)
                            Console.WriteLine(vCtx.Request.HttpMethod + " [*/*] " + vPath.Substring(ServerPath.Length));
                        if (ServerAllowIndexOf && vIsFound && !vIsFile) {
                            vCtx.Response.ContentType = "text/html; charset=utf-8";
                            string vIndexOfFileHContents = "";
                            if (File.Exists(IndexOfFileHeader))
                                vIndexOfFileHContents = File.ReadAllText(IndexOfFileHeader);
                            int vNPos2 = -1,
                                vNPos = vIndexOfFileHContents.IndexOf(IndexOfNeedle1);
                            if (-1 != vNPos) {
                                vNPos2 = vNPos + IndexOfNeedle1.Length;
                                WriteText(vCtx, vIndexOfFileHContents.Substring(0, vNPos));
                            }
                            // Write Dirs/Files
                            foreach (var vItem in Directory.GetDirectories(vPath)) {
                                var vPath2 = vPath.Substring(ServerPath.Length);
                                var vInfo = new DirectoryInfo(vItem);
                                var vURL = vPath2 + "/" + vInfo.Name;
                                WriteText(vCtx, string.Format(IndexOfDirLine, vURL, vInfo.Name));
                            }
                            foreach (var vItem in Directory.GetFiles(vPath)) {
                                var vPath2 = vPath.Substring(ServerPath.Length);
                                var vInfo = new FileInfo(vItem);
                                var vURL = vPath2 + "/" + vInfo.Name;
                                WriteText(vCtx, string.Format(IndexOfDirLine, vURL, vInfo.Name));
                            }
                            // Write remaining header
                            if (-1 != vNPos2)
                                WriteText(vCtx, vIndexOfFileHContents.Substring(vNPos2));
                            // Write footer
                            if (IndexOfFileFooter.Length > 0 && File.Exists(IndexOfFileFooter))
                                WriteText(vCtx, File.ReadAllText(IndexOfFileFooter));
                        }
                    }
                    vCtx.Response.Close();
                });
            }
        }
    }
}
