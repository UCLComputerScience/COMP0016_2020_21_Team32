using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SFB;
using TMPro;
using System.IO;
using System.Linq;
using System.Globalization;
<<<<<<< HEAD

///<summary>Helper class for dealing with files</summary>
=======
>>>>>>> 82abfe8fcbdad9d7e902c5387123d5828c2ba7d3
public class FileHelper{
    public static string currentModelFileName = "brain.glb";
    public static string currentAnnotationFolder = "brain.glb";
    public static void setCurrentModelFileName(string filename){
        currentModelFileName = filename;
        currentAnnotationFolder = getReadableFileName(filename);
    }
    private DirectoryInfo dir;    
    public FileHelper(string path){
        this.dir = new DirectoryInfo(path);
    }
<<<<<<< HEAD

    /*Return a list of all files in the current directory*/
=======
>>>>>>> 82abfe8fcbdad9d7e902c5387123d5828c2ba7d3
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
<<<<<<< HEAD
    /*Generate a readable file name (relative, no path separators)*/
=======
>>>>>>> 82abfe8fcbdad9d7e902c5387123d5828c2ba7d3
    public static string getReadableFileName(string filename){
        int index = filename.LastIndexOf(Path.DirectorySeparatorChar);
        return (index != -1) ? filename.Substring(index+1) : filename;
    }
<<<<<<< HEAD
    /*Get a list of relative path names with their extensions omitted*/
=======
>>>>>>> 82abfe8fcbdad9d7e902c5387123d5828c2ba7d3
    public List<string> getRelativePathsNoExtensions(string searchPattern="*"){
        TextInfo ti = CultureInfo.CurrentCulture.TextInfo;
        return dir.GetFiles(searchPattern).Select(f => ti.ToTitleCase(f.Name.Substring(0, f.Name.IndexOf(".")))).ToList<string>();
    }
    
    public List<AnnotationData> JsonToAnnotations(){
        return (List<AnnotationData>) dir.GetFiles("*.json").Select(f => JsonUtility.FromJson<AnnotationData>(File.ReadAllText(f.FullName)) as AnnotationData); 
    }

}
<<<<<<< HEAD
=======
/*
FileInfo[] info = dir.GetFiles("*.json");
        
        foreach (FileInfo f in info){
            if(!f.Exists)f.Create();
            Debug.Log(f.FullName);
            String jsonToParse = File.ReadAllText(f.FullName);
            annotations.Add(JsonUtility.FromJson<AnnotationData>(jsonToParse) as AnnotationData);
        }
*/
>>>>>>> 82abfe8fcbdad9d7e902c5387123d5828c2ba7d3
