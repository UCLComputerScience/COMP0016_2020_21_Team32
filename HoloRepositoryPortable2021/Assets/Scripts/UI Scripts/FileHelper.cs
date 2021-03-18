using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SFB;
using TMPro;
using System.IO;
using System.Linq;
using System.Globalization;
using UnityEditor;

///<summary>Helper class for dealing with files. Maintains a reference to the path the chosen model was loaded in from, and
///the name of the folder that will hold all the annotations of the model.</summary>
public class FileHelper{
    public static string currentModelFileName = Path.Combine(Application.streamingAssetsPath, "brain.glb");
    public static string currentAnnotationFolder = "brain.glb";
    public static void setCurrentModelFileName(string filename){
        currentModelFileName = filename;
        currentAnnotationFolder = getReadableFileName(filename);
    }
    private DirectoryInfo dir;    
    public FileHelper(string path){
        this.dir = new DirectoryInfo(path);
    }

    /*Return a list of all files in the current directory. Whether these files are relative or not is determined by the boolean
    "relative".*/
    public List<string> getPathsInDir(string searchPattern="*", bool relative=false){
        List<string> paths = (relative) ? getRelativePathsInDir(searchPattern) : getAbsolutePathsInDir(searchPattern);
        return paths;
    }
    /*Generate a readable file name (relative, no path separators)*/
    public static string getReadableFileName(string filename){
        int index = filename.LastIndexOf(Path.DirectorySeparatorChar);
        return (index != -1) ? filename.Substring(index+1) : filename;
    }
    /*Get a list of relative path names with their extensions omitted*/
    public List<string> getRelativePathsNoExtensions(string searchPattern="*"){
        TextInfo ti = CultureInfo.CurrentCulture.TextInfo;
        return dir.GetFiles(searchPattern).Select(f => ti.ToTitleCase(f.Name.Substring(0, f.Name.IndexOf(".")))).ToList<string>();
    }
    private List<string> getRelativePathsInDir(string searchPattern="*"){
        return dir.GetFiles(searchPattern).Select(f => f.Name).ToList<string>();
    }
    private List<string> getAbsolutePathsInDir(string searchPattern="*"){
        return dir.GetFiles(searchPattern).Select(f => f.FullName).ToList<string>();
    }
    


}
