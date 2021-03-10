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

///<summary>Helper class for dealing with files</summary>
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

    /*Return a list of all files in the current directory*/
    public List<string> getPathsInDir(string searchPattern="*", bool relative=false){
        List<string> paths = (relative) ? getRelativePathsInDir(searchPattern) : getAbsolutePathsInDir(searchPattern);
        return paths;
    }
    private List<string> getRelativePathsInDir(string searchPattern="*"){
        return dir.GetFiles(searchPattern).Select(f => f.Name).ToList<string>();
    }
    private List<string> getAbsolutePathsInDir(string searchPattern="*"){
        return dir.GetFiles(searchPattern).Select(f => f.FullName).ToList<string>();
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
    
    public List<AnnotationData> JsonToAnnotations(){
        return (List<AnnotationData>) dir.GetFiles("*.json").Select(f => JsonUtility.FromJson<AnnotationData>(File.ReadAllText(f.FullName)) as AnnotationData); 
    }

}
