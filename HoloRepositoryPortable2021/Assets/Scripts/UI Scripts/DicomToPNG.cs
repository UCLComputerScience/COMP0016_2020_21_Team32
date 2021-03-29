using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using itk.simple;
using PixelId = itk.simple.PixelIDValueEnum;

///<summary>This class provides a method to read in DICOM data and convert it into a texture2D.
///Relies on the SimpleImageToolKit library.</summary>
public class DicomToPNG{
    private int targetWidth; 
    private int targetHeight; 
    public DicomToPNG(int targetWidth, int targetHeight){
        this.targetWidth = targetWidth;
        this.targetHeight = targetHeight;
    }

    /*Converts a .dcm into a SITKImage object, which is then written to a png. The PNG is then loaded back into the application
    as a Texture2D object and returned. This function is based on an example from the SITK website:
    https://simpleitk.readthedocs.io/en/master/link_DicomConvert_docs.html*/
    public Texture2D ReadDICOM(string inputfile){
        ImageFileReader imageFileReader = new ImageFileReader(); //SITK class
        imageFileReader.SetImageIO("GDCMImageIO");//setting the desired input image type to dcm
        imageFileReader.SetFileName(inputfile);
        imageFileReader.ReadImageInformation();
        var size = imageFileReader.GetSize();
        if(size.Count == 3 && size[2] == 1){
            size[2] = 0;
        }
        itk.simple.Image image = imageFileReader.Execute();
        if(image.GetNumberOfComponentsPerPixel() == 1){
            image = SimpleITK.RescaleIntensity(image, 0, 255);
            if(imageFileReader.GetMetaData("0028|0004").Trim() == "MONOCHROME1"){
                image = SimpleITK.InvertIntensity(image, 255);
            }
            image = SimpleITK.Cast(image, PixelId.sitkUInt8);
        }
        string tempFile = Path.Combine(Application.dataPath, "temp.png");
        SimpleITK.WriteImage(image, tempFile);
        
        Texture2D texture = new Texture2D(this.targetWidth, this.targetHeight);
        texture.LoadImage(File.ReadAllBytes(tempFile));
        File.Delete(tempFile);
        return texture;
    }
}
