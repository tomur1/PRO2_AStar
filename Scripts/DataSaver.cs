using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class DataSaver
{
    private static string savePath = Application.persistentDataPath + "/data.game";
    
    public static void SaveData()
    {
        XmlSerializer formatter = new XmlSerializer(typeof(DataHolder));
        FileStream fs = new FileStream(savePath, FileMode.Create);
        var dataToSave = new DataHolder();
        dataToSave.GetDataToSave();
        formatter.Serialize(fs, dataToSave);
        fs.Close();
    }
    
    public static DataHolder LoadData()
    {
        // Reads the save. Overrides not saved data
        if (!File.Exists(savePath))
        {
            throw new FileNotFoundException();
        }
        XmlSerializer formatter = new XmlSerializer(typeof(DataHolder));
        FileStream fs = new FileStream(savePath, FileMode.Open);
        
        var dataHolder = (DataHolder) formatter.Deserialize(fs);
        
        fs.Close();
        return dataHolder;
    }
}
