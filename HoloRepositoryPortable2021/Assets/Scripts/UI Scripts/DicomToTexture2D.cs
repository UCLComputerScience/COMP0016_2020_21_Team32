using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SFB;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using itk.simple;
using PixelId = itk.simple.PixelIDValueEnum;

///<summary>This class provides a method to read in DICOM data and convert it into a texture2D.
///Relies on the SimpleImageToolKit</summary>
public class DicomToTexture2D 
{
    private int targetWidth;
    private int targetHeight;
    public DicomToTexture2D(int targetWidth, int targetHeight){
        this.targetWidth = targetWidth;
        this.targetHeight = targetHeight;
    }

    /*Converts a .dcm into a SITK Image object, which is then temporarily written to a png before being loaded into a texture2D*/
    public Texture2D ReadDICOM(string inputfile){
        var imageFileReader = new itk.simple.ImageFileReader();
        imageFileReader.SetImageIO("GDCMImageIO");
        imageFileReader.SetFileName(inputfile);
        imageFileReader.ReadImageInformation();
        var size = imageFileReader.GetSize();
        if(size.Count == 3 && size[2] == 1){
            size[2] = 0;
        }
        var image = imageFileReader.Execute();
        if(image.GetNumberOfComponentsPerPixel() == 1){
            image = SimpleITK.RescaleIntensity(image, 0, 255);
            if(imageFileReader.GetMetaData("0028|0004").Trim() == "MONOCHROME1"){
                image = SimpleITK.InvertIntensity(image, 255);
            }
            image = SimpleITK.Cast(image, PixelId.sitkUInt8);
        }
        string tempFile = Application.dataPath + Path.DirectorySeparatorChar + "temp.png";
        SimpleITK.WriteImage(image, tempFile);
        Texture2D texture = new Texture2D(this.targetWidth, this.targetHeight);
        texture.LoadImage(File.ReadAllBytes(tempFile));
        File.Delete(tempFile);
        return texture;

    }
}
   