using NAudio.Wave;
using System;
namespace mp3_multi_language
{
  class Program
  {
    static void Main(string[] args)
    {

      if (args.Length!=4)
      {
        Console.WriteLine("Usage: mp3-multi-langauge.exe lang1.mp3 lang2.mp3 output.mp3 alternateSeconds" );
        return;
      }
      string strMP3SourceFilename1 = args[0]; 
      string strMP3SourceFilename2 = args[1];
      string strMP3OutputFilename = args[2];
      double secondsBetweenLangauges = double.Parse(args[3]);

      using (Mp3FileReader reader1 = new Mp3FileReader( strMP3SourceFilename1))
      {
        using (Mp3FileReader reader2 = new Mp3FileReader( strMP3SourceFilename2))
        {

          System.IO.FileStream _fs = new System.IO.FileStream(strMP3OutputFilename, System.IO.FileMode.Create, System.IO.FileAccess.Write);

          Mp3Frame mp3Frame1 = reader1.ReadNextFrame();
          Mp3Frame mp3Frame2 = reader1.ReadNextFrame();
       
          
          while (mp3Frame1!=null || mp3Frame2!=null)
          {
            if (mp3Frame1 != null)
            {
              WriteFrames(_fs,reader1, secondsBetweenLangauges, ref mp3Frame1);
            }
            if (mp3Frame2!= null)
            {
             WriteFrames(_fs, reader2, secondsBetweenLangauges, ref mp3Frame2 );
            }
          }

          _fs.Close();
        }
      }
    }

    private static Mp3Frame WriteFrames(System.IO.FileStream _fs, Mp3FileReader reader1, double secondsToWrite,
      ref Mp3Frame mp3Frame)
    {
      double secondsTotal = 0;
      while (secondsTotal < secondsToWrite && mp3Frame!= null)
      {
        secondsTotal += mp3Frame.SampleCount / (double)mp3Frame.SampleRate;
        _fs.Write(mp3Frame.RawData, 0, mp3Frame.RawData.Length);
       
        mp3Frame = reader1.ReadNextFrame();
      }
     // Console.WriteLine(secondsTotal);
      return mp3Frame;
    }
}
}
