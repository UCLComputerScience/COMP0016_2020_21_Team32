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

///<summary>This class provides a method to read in DICOM data and then temporarily write the image to a png.
///Relies on the SimpleImagingToolKit</summary>
public class DicomToTexture2D 
{
    private int targetWidth;
    private int targetHeight;
    public DicomToTexture2D(int targetWidth, int targetHeight){
        this.targetWidth = targetWidth;
        this.targetHeight = targetHeight;
        //MagickAnyCPU.CacheDirectory = Application.dataPath+Path.DirectorySeparatorChar+"Packages";
    }
    // public void ReadDicom2(string inputfile, string outputfile){
    //     using (var image = new MagickImage(inputfile))
    //     {
    //         // Save frame as jpg
    //         image.Write(outputfile);
    //     }    
    // }
    public static byte[] dataToByteArray(object obj){
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        using(MemoryStream memoryStream = new MemoryStream()){
            binaryFormatter.Serialize(memoryStream, obj);
            return memoryStream.ToArray();
        }
    }
    
    /*Reads in a DICOM image and writes it to a png. Based on one of the provided example scripts available in the SITK github repo*/
    // public void ReadDICOM(string inputfile, string outputfile, List<Texture2D> textures){
    //         var imageFileReader = new itk.simple.ImageFileReader();
    //         imageFileReader.SetImageIO("GDCMImageIO");
    //         imageFileReader.SetFileName(inputfile);
    //         imageFileReader.ReadImageInformation();
    //         var size = imageFileReader.GetSize();
    //         if(size.Count == 3 && size[2] == 1){
    //             size[2] = 0;
    //         }
    //         var image = imageFileReader.Execute();
    //         if(image.GetNumberOfComponentsPerPixel() == 1){
    //             image = SimpleITK.RescaleIntensity(image, 0, 255);
    //             if(imageFileReader.GetMetaData("0028|0004").Trim() == "MONOCHROME1"){
    //                 image = SimpleITK.InvertIntensity(image, 255);
    //             }
    //             image = SimpleITK.Cast(image, PixelId.sitkUInt8);
    //         }
    //         SimpleITK.WriteImage(image, outputfile);
    //         Texture2D texture = new Texture2D(this.targetWidth, this.targetHeight);
    //         texture.LoadRawTextureData(dataToByteArray(image));
    //         // System.IntPtr bytes = image.GetBufferAsUInt8();
    //         // byte *bufferPointer = (byte *) bytes.ToPointer();
    //         // for(int k = 0; k < (int) size[2]; k++){
    //         //     for(int j = 0; j < (int)size[1]; j++){
    //         //         for(int i = 0; i < (int) size[0]; i++){
    //         //             var pixel = bufferPointer[i + j *(int) size[1]+k*(int)size[2]*(int)size[1]];
    //         //             Color col = new Color(pixel, pixel, pixel, 255);

    //         //         }
    //         //     }
    //         // }
    //         // for(int i = 0; i < targetWidth; i++){
    //         //     for(int j = 0; j < targetHeight; j++){
    //         //         int pixelVal = image.GetPixelAsUInt8(new VectorUInt32( new uint[] {(uint)i ,(uint)j}));
    //         //         Color col = new Color(pixelVal, pixelVal, pixelVal, 255);
    //         //         tex.SetPixel(i, j, col);
    //         //     }
    //         // }
    //         // textures.Add(tex);
    // }
        public unsafe Texture2D ReadDICOM(string inputfile){
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
            // Texture2D texture = new Texture2D(this.targetWidth, this.targetHeight);
            // System.IntPtr bytes = image.GetBufferAsUInt8();
            // byte *bufferPointer = (byte *) bytes.ToPointer();
            // for(int k = 0; k < (int) size[2]; k++){
            //     Debug.Log("k: "+ size[2]);
            //     for(int j = 0; j < (int)size[1]; j++){
            //         Debug.Log("j: "+size[1]);
            //         for(int i = 0; i < (int) size[0]; i++){
            //             Debug.Log("i: "+size[0]);
            //             var pixel = bufferPointer[i + j *(int) size[1]+k*(int)size[2]*(int)size[1]];
            //             Color col = new Color(pixel, pixel, pixel, 255);
            //             texture.SetPixel(i, j, col);
            //         }
            //     }
            // }
            // texture.Apply();
            // return texture;
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
    // public static void ReadDICOMopenDicom(string input){
    //     DataElementDictionary dem = new DataElementDictionary();
    //     UidDictionary dict = new UidDictionary();
    //     dem.LoadFrom(input, DictionaryFileFormat.BinaryFile);
    //     dict.LoadFrom(input, DictionaryFileFormat.BinaryFile);
    //     DicomFile file;
    //     if(DicomFile.IsDicomFile(input)){
    //         file = new DicomFile(input);
    //         PixelData pixeldata = new PixelData(file.GetJointDataSets());
    //         byte[][] pixels = (byte[][]) pixeldata.ToArray();
    //         Debug.Log(pixels);
    //     }else{
    //         Debug.Log("It's not");
    //     }
        
   // }     
//}
