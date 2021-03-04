using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SFB;
using System.IO;
using itk.simple;
using PixelId = itk.simple.PixelIDValueEnum;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

///<summary>This class provides a method to read in DICOM data and then temporarily write the image to a png.
///Relies on the SimpleImagingToolKit</summary>
public class DicomToTexture2D 
{
    private int targetWidth;
    private int targetHeight;
    public DicomToTexture2D(int targetWidth, int targetHeight){
        this.targetWidth = targetWidth;
        this.targetHeight = targetHeight;
    }
    /*Reads in a DICOM image and writes it to a png. Based on one of the provided example scripts available in the SITK github repo*/
    public void ReadDICOM(string inputfile, string outputfile, List<Texture2D> textures){
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
            SimpleITK.WriteImage(image, outputfile);
            // Texture2D =
            // System.IntPtr bytes = image.GetBufferAsUInt8();
            // byte *bufferPointer = (byte *) bytes.ToPointer();
            // for(int k = 0; k < (int) size[2]; k++){
            //     for(int j = 0; j < (int)size[1]; j++){
            //         for(int i = 0; i < (int) size[0]; i++){
            //             var pixel = bufferPointer[i + j *(int) size[1]+k*(int)size[2]*(int)size[1]];
            //             Color col = new Color(pixel, pixel, pixel, 255);

            //         }
            //     }
            // }
            // for(int i = 0; i < targetWidth; i++){
            //     for(int j = 0; j < targetHeight; j++){
            //         int pixelVal = image.GetPixelAsUInt8(new VectorUInt32( new uint[] {(uint)i ,(uint)j}));
            //         Color col = new Color(pixelVal, pixelVal, pixelVal, 255);
            //         tex.SetPixel(i, j, col);
            //     }
            // }
            // textures.Add(tex);
    }      
}
